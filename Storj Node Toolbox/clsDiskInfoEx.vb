Imports Microsoft.Win32.SafeHandles
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Text

Public Class clsDiskInfoEx

    Private Const GenericRead As Integer = &H80000000
    Private Const FileShareRead As Integer = 1
    Private Const Filesharewrite As Integer = 2
    Private Const OpenExisting As Integer = 3
    Private Const IoctlVolumeGetVolumeDiskExtents As Integer = &H560000
    Private Const IncorrectFunction As Integer = 1
    Private Const ErrorInsufficientBuffer As Integer = 122
    Private Const MoreDataIsAvailable As Integer = 234

    Private currentDriveMappings As List(Of String)
    Private errorMessage As String

    Public Enum RESOURCE_SCOPE
        RESOURCE_CONNECTED = &H1
        RESOURCE_GLOBALNET = &H2
        RESOURCE_REMEMBERED = &H3
        RESOURCE_RECENT = &H4
        RESOURCE_CONTEXT = &H5
    End Enum

    Public Enum RESOURCE_TYPE
        RESOURCETYPE_ANY = &H0
        RESOURCETYPE_DISK = &H1
        RESOURCETYPE_PRINT = &H2
        RESOURCETYPE_RESERVED = &H8
    End Enum

    Public Enum RESOURCE_USAGE
        RESOURCEUSAGE_CONNECTABLE = &H1
        RESOURCEUSAGE_CONTAINER = &H2
        RESOURCEUSAGE_NOLOCALDEVICE = &H4
        RESOURCEUSAGE_SIBLING = &H8
        RESOURCEUSAGE_ATTACHED = &H10
        RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE Or RESOURCEUSAGE_CONTAINER Or RESOURCEUSAGE_ATTACHED)
    End Enum

    Public Enum RESOURCE_DISPLAYTYPE
        RESOURCEDISPLAYTYPE_GENERIC = &H0
        RESOURCEDISPLAYTYPE_DOMAIN = &H1
        RESOURCEDISPLAYTYPE_SERVER = &H2
        RESOURCEDISPLAYTYPE_SHARE = &H3
        RESOURCEDISPLAYTYPE_FILE = &H4
        RESOURCEDISPLAYTYPE_GROUP = &H5
        RESOURCEDISPLAYTYPE_NETWORK = &H6
        RESOURCEDISPLAYTYPE_ROOT = &H7
        RESOURCEDISPLAYTYPE_SHAREADMIN = &H8
        RESOURCEDISPLAYTYPE_DIRECTORY = &H9
        RESOURCEDISPLAYTYPE_TREE = &HA
        RESOURCEDISPLAYTYPE_NDSCONTAINER = &HB
    End Enum

    Public Enum NERR
        NERR_Success = 0
        ERROR_MORE_DATA = 234
        ERROR_NO_BROWSER_SERVERS_FOUND = 6118
        ERROR_INVALID_LEVEL = 124
        ERROR_ACCESS_DENIED = 5
        ERROR_INVALID_PARAMETER = 87
        ERROR_NOT_ENOUGH_MEMORY = 8
        ERROR_NETWORK_BUSY = 54
        ERROR_BAD_NETPATH = 53
        ERROR_NO_NETWORK = 1222
        ERROR_INVALID_HANDLE_STATE = 1609
        ERROR_EXTENDED_ERROR = 1208
    End Enum

    Public Structure NETRESOURCE
        Public dwScope As RESOURCE_SCOPE
        Public dwType As RESOURCE_TYPE
        Public dwDisplayType As RESOURCE_DISPLAYTYPE
        Public dwUsage As RESOURCE_USAGE
        <MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpLocalName As String
        <MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpRemoteName As String
        <MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpComment As String
        <MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> Public lpProvider As String
    End Structure

    Private Declare Auto Function QueryDosDevice Lib "kernel32.dll" (ByVal lpDeviceName As String, _
                                                                     ByVal lpTargetPath As IntPtr, _
                                                                     ByVal ucchMax As UInteger) As UInteger

    Private Class NativeMethods
        <DllImport("kernel32", CharSet:=CharSet.Unicode, SetLastError:=True)> _
        Public Shared Function CreateFile( _
            ByVal fileName As String, _
            ByVal desiredAccess As Integer, _
            ByVal shareMode As Integer, _
            ByVal securityAttributes As IntPtr, _
            ByVal creationDisposition As Integer, _
            ByVal flagsAndAttributes As Integer, _
            ByVal hTemplateFile As IntPtr) As SafeFileHandle
        End Function

        <DllImport("kernel32", SetLastError:=True)> _
        Public Shared Function DeviceIoControl( _
            ByVal hVol As SafeFileHandle, _
            ByVal controlCode As Integer, _
            ByVal inBuffer As IntPtr, _
            ByVal inBufferSize As Integer, _
            ByRef outBuffer As DiskExtents, _
            ByVal outBufferSize As Integer, _
            ByRef bytesReturned As Integer, _
            ByVal overlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("kernel32", SetLastError:=True)> _
        Public Shared Function DeviceIoControl( _
            ByVal hVol As SafeFileHandle, _
            ByVal controlCode As Integer, _
            ByVal inBuffer As IntPtr, _
            ByVal inBufferSize As Integer, _
            ByVal outBuffer As IntPtr, _
            ByVal outBufferSize As Integer, _
            ByRef bytesReturned As Integer, _
            ByVal overlapped As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("mpr.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function WNetEnumResource( _
            ByVal hEnum As IntPtr, _
            ByRef lpcCount As Integer, _
            ByVal lpBuffer As IntPtr, _
            ByRef lpBufferSize As Integer) As Integer
        End Function

        <DllImport("mpr.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function WNetOpenEnum( _
            ByVal dwScope As RESOURCE_SCOPE, _
            ByVal dwType As RESOURCE_TYPE, _
            ByVal dwUsage As RESOURCE_USAGE, _
            ByRef lpNetResource As NETRESOURCE, _
            ByRef lphEnum As IntPtr) As Integer
        End Function

        <DllImport("mpr.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function WNetCloseEnum( _
        ByVal hEnum As IntPtr) As Integer
        End Function
    End Class

    ' DISK_EXTENT in the msdn.
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure DiskExtent
        Public DiskNumber As Integer
        Public StartingOffset As Long
        Public ExtentLength As Long
    End Structure

    ' DISK_EXTENTS
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure DiskExtents
        Public numberOfExtents As Integer
        Public first As DiskExtent ' We can't marhsal an array if we don't know its size.
    End Structure

    Public Sub New()
        Refresh()
    End Sub

    Public Sub Refresh()
        errorMessage = ""
        currentDriveMappings = Nothing
        currentDriveMappings = New List(Of String)
        GetPhysicalDisks(currentDriveMappings)
    End Sub

    ' A Volume could be on many physical drives.
    ' Returns a list of string containing each physical drive the volume uses.
    ' For CD Drives with no disc in it will return an empty list.
    Private Function GetPhysicalDriveStrings(ByVal driveInfo As DriveInfo) As List(Of String)
        Dim sfh As SafeFileHandle = Nothing
        Dim physicalDrives As New List(Of String)(1)
        Dim path As String = "\\.\" & driveInfo.RootDirectory.ToString.TrimEnd("\"c)
        Try
            sfh = NativeMethods.CreateFile(path, 0, FileShareRead Or Filesharewrite, IntPtr.Zero, _
                                                           OpenExisting, 0, IntPtr.Zero)
            Dim bytesReturned As Integer
            Dim de1 As DiskExtents = Nothing
            Dim numDiskExtents As Integer = 0
            Dim result As Boolean = NativeMethods.DeviceIoControl(sfh, IoctlVolumeGetVolumeDiskExtents, IntPtr.Zero, _
                                                                  0, de1, Marshal.SizeOf(de1), bytesReturned, IntPtr.Zero)
            If result = True Then
                ' there was only one disk extent. So the volume lies on 1 physical drive.
                physicalDrives.Add("\\.\PhysicalDrive" & de1.first.DiskNumber.ToString)
                Return physicalDrives
            End If
            If Marshal.GetLastWin32Error = IncorrectFunction Then
                ' The drive is removable and removed, like a CDRom with nothing in it.
                Return physicalDrives
            End If
            If Marshal.GetLastWin32Error = MoreDataIsAvailable Then
                ' This drive is part of a mirror or volume - handle it below. 
            ElseIf Marshal.GetLastWin32Error <> ErrorInsufficientBuffer Then
                Throw New Win32Exception
            End If
            ' Houston, we have a spanner. The volume is on multiple disks.
            ' Untested...
            ' We need a blob of memory for the DISK_EXTENTS structure, and all the DISK_EXTENTS
            Dim blobSize As Integer = Marshal.SizeOf(GetType(DiskExtents)) + _
                                      (de1.numberOfExtents - 1) * Marshal.SizeOf(GetType(DiskExtent))
            Dim pBlob As IntPtr = Marshal.AllocHGlobal(blobSize)
            result = NativeMethods.DeviceIoControl(sfh, IoctlVolumeGetVolumeDiskExtents, IntPtr.Zero, 0, pBlob, _
                                                   blobSize, bytesReturned, IntPtr.Zero)
            If result = False Then Throw New Win32Exception
            ' Read them out one at a time.
            Dim pNext As New IntPtr(pBlob.ToInt64 + 8) ' is this always ok on 64 bit OSes? ToInt64?
            For i As Integer = 0 To de1.numberOfExtents - 1
                Dim diskExtentN As DiskExtent = DirectCast(Marshal.PtrToStructure(pNext, GetType(DiskExtent)), DiskExtent)
                physicalDrives.Add("\\.\PhysicalDrive" & diskExtentN.DiskNumber.ToString)
                pNext = New IntPtr(pNext.ToInt32 + Marshal.SizeOf(GetType(DiskExtent)))
            Next
            Return physicalDrives
        Finally
            If sfh IsNot Nothing Then
                If sfh.IsInvalid = False Then
                    sfh.Close()
                End If
                sfh.Dispose()
            End If
        End Try
    End Function

    Private Function QueryDosDevice(ByVal device As String) As List(Of String)

        Dim returnSize As Integer = 0
        Dim maxSize As UInteger = 65536
        Dim allDevices As String = Nothing
        Dim mem As IntPtr
        Dim retval() As String = Nothing
        Dim results As New List(Of String)

        ' Convert an empty string into Nothing, so 
        ' QueryDosDevice will return everything available.
        If device.Trim = "" Then device = Nothing

        While returnSize = 0
            mem = Marshal.AllocHGlobal(CInt(maxSize))
            If mem <> IntPtr.Zero Then
                Try
                    returnSize = CInt(QueryDosDevice(device, mem, maxSize))
                    If returnSize <> 0 Then
                        allDevices = Marshal.PtrToStringAuto(mem, returnSize)
                        retval = allDevices.Split(ControlChars.NullChar)
                        Exit Try
                    Else
                        ' This query produced no results. Exit the loop.
                        returnSize = -1
                    End If
                Finally
                    Marshal.FreeHGlobal(mem)
                End Try
            Else
                Throw New OutOfMemoryException()
            End If
        End While

        If retval IsNot Nothing Then
            For Each result As String In retval
                If result.Trim <> "" Then results.Add(result)
            Next
        End If

        Return results
    End Function

    Public Function GetPhysicalDiskParentFor(ByVal logicalDisk As String) As String

        Dim parts() As String = Nothing

        If logicalDisk.Length > 0 Then
            For Each driveMapping As String In currentDriveMappings
                If logicalDisk.Substring(0, 2).ToUpper = _
                    driveMapping.Substring(0, 2).ToUpper Then
                    parts = Split(driveMapping, "=")
                    Return parts(parts.Length - 1)
                End If
            Next
        End If

        Return ""
    End Function

    Public Function GetPhysicalDisks(ByRef theList As List(Of String)) As Boolean

        Dim drivesList As List(Of String)
        Dim tmpList As List(Of String)
        Dim parts() As String = Nothing
        Dim drives As New StringBuilder

        For Each logicalDrive As DriveInfo In DriveInfo.GetDrives
            Try
                drives.Remove(0, drives.Length)
                drives.Append(logicalDrive.RootDirectory.ToString)
                drives.Append("=")

                If logicalDrive.DriveType = DriveType.Network Then
                    ' Handle network drives here.
                    drives.Append(GetUncPathOfMappedDrive(logicalDrive.RootDirectory.ToString))
                ElseIf logicalDrive.DriveType = DriveType.CDRom Then
                    ' Attempt to get the CDRom's dos name from QueryDosDevice
                    tmpList = QueryDosDevice(logicalDrive.RootDirectory.ToString.Replace("\", ""))
                    If tmpList.Count > 0 Then
                        parts = Split(tmpList.Item(0).Trim, "\")
                        If parts(parts.Length - 1).Length > 5 Then
                            If parts(parts.Length - 1).Substring(0, 5) = "CdRom" Then _
                                parts(parts.Length - 1) = parts(parts.Length - 1).Replace("CdRom", "CD/DVD Rom ")
                        End If
                        drives.Append(parts(parts.Length - 1))
                    Else
                        drives.Append("n/a")
                    End If
                Else
                    drivesList = GetPhysicalDriveStrings(logicalDrive)

                    If drivesList.Count > 0 Then
                        For Each drive As String In drivesList
                            ' Handle the spanners
                            drive = drive.Replace("\\.\", "")
                            drive = drive.Replace("PhysicalDrive", "Physical Drive ")
                            drives.Append(drive)
                            drives.Append(", ")
                        Next
                        drives.Remove(drives.Length - 2, 2)
                    Else
                        drives.Append("n/a")
                    End If
                End If
                theList.Add(drives.ToString)
            Catch ex As Exception
                errorMessage = ex.Message
                MsgBox("" & ex.Message & vbCrLf & vbCrLf & drives.ToString)
            End Try
        Next

        If errorMessage <> "" Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function GetUncPathOfMappedDrive(ByVal driveLetter As String) As String

        If driveLetter.Substring(driveLetter.Length - 1, 1) = "\" Then driveLetter = driveLetter.Replace("\", "")
        GetUncPathOfMappedDrive = ""

        Dim nwDrives As New List(Of String)
        Dim parts() As String

        If GetNetworkDrives(Nothing, nwDrives) Then
            For Each driveMapping As String In nwDrives
                parts = Split(driveMapping, "=")
                If parts(0).Trim.ToLower = driveLetter.Trim.ToLower Then
                    Return parts(1)
                End If
            Next
        End If

    End Function

    ' Usage:
    'Dim nwDrives As New List(Of String)
    'GetNetworkDrives(Nothing, nwDrives)

    'For Each item As String In nwDrives
    '    'ListBox1.Items.Add(item)
    'Next
    Public Function GetNetworkDrives(ByRef o As NETRESOURCE, ByRef networkDriveCollection As List(Of String)) As Boolean

        Dim iRet As Integer
        Dim ptrHandle As IntPtr = New IntPtr()

        Try
            iRet = NativeMethods.WNetOpenEnum(RESOURCE_SCOPE.RESOURCE_REMEMBERED, RESOURCE_TYPE.RESOURCETYPE_ANY, RESOURCE_USAGE.RESOURCEUSAGE_ATTACHED, o, ptrHandle)
            If iRet <> 0 Then Exit Function

            Dim entries As Integer
            Dim buffer As Integer = 16384
            Dim ptrBuffer As IntPtr = Marshal.AllocHGlobal(buffer)
            Dim nr As NETRESOURCE

            Do
                entries = -1
                buffer = 16384
                iRet = NativeMethods.WNetEnumResource(ptrHandle, entries, ptrBuffer, buffer)
                If iRet <> 0 Or entries < 1 Then Exit Do

                Dim ptr As Int32 = ptrBuffer.ToInt32
                For count As Integer = 0 To entries - 1
                    nr = CType(Marshal.PtrToStructure(New IntPtr(ptr), GetType(NETRESOURCE)), NETRESOURCE)
                    If (RESOURCE_USAGE.RESOURCEUSAGE_CONTAINER = (nr.dwUsage And RESOURCE_USAGE.RESOURCEUSAGE_CONTAINER)) Then
                        If Not GetNetworkDrives(nr, networkDriveCollection) Then
                            Throw New Exception("")
                        End If
                    End If

                    ptr += Marshal.SizeOf(nr)
                    networkDriveCollection.Add(String.Format(nr.lpLocalName & "=" & nr.lpRemoteName))
                Next
            Loop
            Marshal.FreeHGlobal(ptrBuffer)
            iRet = NativeMethods.WNetCloseEnum(ptrHandle)
        Catch ex As Exception
            If ex.Message <> "" Then networkDriveCollection.Add(ex.Message)
            Return False
        End Try

        Return True
    End Function

    ' Usage:
    'Dim nwComputers As New List(Of String)
    'GetNetworkComputers(Nothing, nwComputers)

    'For Each item As String In nwComputers
    '    ListBox1.Items.Add(item)
    'Next
    Public Function GetNetworkComputers(ByRef o As NETRESOURCE, ByRef networkComputersCollection As List(Of String)) As Boolean

        Dim iRet As Integer
        Dim ptrHandle As IntPtr = New IntPtr()

        Try
            iRet = NativeMethods.WNetOpenEnum(RESOURCE_SCOPE.RESOURCE_GLOBALNET, RESOURCE_TYPE.RESOURCETYPE_ANY, RESOURCE_USAGE.RESOURCEUSAGE_CONTAINER, o, ptrHandle)
            If iRet <> 0 Then Exit Function

            Dim entries As Integer
            Dim buffer As Integer = 16384
            Dim ptrBuffer As IntPtr = Marshal.AllocHGlobal(buffer)
            Dim nr As NETRESOURCE

            Do
                entries = -1
                buffer = 16384
                iRet = NativeMethods.WNetEnumResource(ptrHandle, entries, ptrBuffer, buffer)
                If iRet <> 0 Or entries < 1 Then Exit Do

                Dim ptr As Int32 = ptrBuffer.ToInt32
                For count As Integer = 0 To entries - 1
                    nr = CType(Marshal.PtrToStructure(New IntPtr(ptr), GetType(NETRESOURCE)), NETRESOURCE)
                    If (RESOURCE_USAGE.RESOURCEUSAGE_CONTAINER = (nr.dwUsage And RESOURCE_USAGE.RESOURCEUSAGE_CONTAINER)) Then
                        If Not GetNetworkComputers(nr, networkComputersCollection) Then
                            Throw New Exception("")
                        End If
                    End If

                    ptr += Marshal.SizeOf(nr)
                    If nr.lpRemoteName.Length > 2 Then
                        If nr.lpRemoteName.Substring(0, 2) = "\\" Then
                            networkComputersCollection.Add(String.Format(nr.lpRemoteName.Remove(0, 2)))
                        End If
                    End If

                Next
            Loop
            Marshal.FreeHGlobal(ptrBuffer)
            iRet = NativeMethods.WNetCloseEnum(ptrHandle)
        Catch ex As Exception
            If ex.Message <> "" Then networkComputersCollection.Add(ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function WNETOE(ByRef o As NETRESOURCE, ByRef resourceCollection As List(Of String)) As Boolean

        Dim iRet As Integer
        Dim ptrHandle As IntPtr = New IntPtr()

        Try
            iRet = NativeMethods.WNetOpenEnum(RESOURCE_SCOPE.RESOURCE_GLOBALNET, RESOURCE_TYPE.RESOURCETYPE_ANY, RESOURCE_USAGE.RESOURCEUSAGE_CONTAINER, o, ptrHandle)
            If iRet <> 0 Then Exit Function

            Dim entries As Integer
            Dim buffer As Integer = 16384
            Dim ptrBuffer As IntPtr = Marshal.AllocHGlobal(buffer)
            Dim nr As NETRESOURCE

            Do
                entries = -1
                buffer = 16384
                iRet = NativeMethods.WNetEnumResource(ptrHandle, entries, ptrBuffer, buffer)
                If iRet <> 0 Or entries < 1 Then Exit Do

                Dim ptr As Int32 = ptrBuffer.ToInt32
                For count As Integer = 0 To entries - 1
                    nr = CType(Marshal.PtrToStructure(New IntPtr(ptr), GetType(NETRESOURCE)), NETRESOURCE)
                    If (RESOURCE_USAGE.RESOURCEUSAGE_CONTAINER = (nr.dwUsage And RESOURCE_USAGE.RESOURCEUSAGE_CONTAINER)) Then
                        If Not WNETOE(nr, resourceCollection) Then
                            Throw New Exception("")
                        End If
                    End If

                    ptr += Marshal.SizeOf(nr)
                    resourceCollection.Add(String.Format(nr.lpLocalName & " = " & nr.lpRemoteName))
                Next
            Loop
            Marshal.FreeHGlobal(ptrBuffer)
            iRet = NativeMethods.WNetCloseEnum(ptrHandle)
        Catch ex As Exception
            If ex.Message <> "" Then resourceCollection.Add(ex.Message)
            Return False
        End Try

        Return True
    End Function

End Class