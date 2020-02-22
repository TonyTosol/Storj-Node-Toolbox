Imports System.Configuration.Install
Imports System.Runtime.InteropServices

Module ServiceInstaller
    Private Const STANDARD_RIGHTS_REQUIRED As Integer = &HF0000
    Private Const SERVICE_WIN32_OWN_PROCESS As Integer = &H10

    <StructLayout(LayoutKind.Sequential)>
    Private Class SERVICE_STATUS
        Public dwServiceType As Integer = 0
        Public dwCurrentState As ServiceState = 0
        Public dwControlsAccepted As Integer = 0
        Public dwWin32ExitCode As Integer = 0
        Public dwServiceSpecificExitCode As Integer = 0
        Public dwCheckPoint As Integer = 0
        Public dwWaitHint As Integer = 0
    End Class

    <DllImport("advapi32.dll", EntryPoint:="OpenSCManagerW", ExactSpelling:=True, CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Function OpenSCManager(ByVal machineName As String, ByVal databaseName As String, ByVal dwDesiredAccess As ScmAccessRights) As IntPtr

    End Function
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Function OpenService(ByVal hSCManager As IntPtr, ByVal lpServiceName As String, ByVal dwDesiredAccess As ServiceAccessRights) As IntPtr

    End Function
    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Function CreateService(ByVal hSCManager As IntPtr, ByVal lpServiceName As String, ByVal lpDisplayName As String, ByVal dwDesiredAccess As ServiceAccessRights, ByVal dwServiceType As Integer, ByVal dwStartType As ServiceBootFlag, ByVal dwErrorControl As ServiceError, ByVal lpBinaryPathName As String, ByVal lpLoadOrderGroup As String, ByVal lpdwTagId As IntPtr, ByVal lpDependencies As String, ByVal lp As String, ByVal lpPassword As String) As IntPtr

    End Function
    '<DllImport("advapi32.dll", SetLastError:=True)>
    '<MarshalAs(UnmanagedType.Bool)>
    Private Function CloseServiceHandle(ByVal hSCObject As IntPtr) As Boolean

    End Function
    <DllImport("advapi32.dll")>
    Private Function QueryServiceStatus(ByVal hService As IntPtr, ByVal lpServiceStatus As SERVICE_STATUS) As Integer

    End Function
    '<DllImport("advapi32.dll", SetLastError:=True)>
    '<MarshalAs(UnmanagedType.Bool)>
    Private Function DeleteService(ByVal hService As IntPtr) As Boolean

    End Function
    <DllImport("advapi32.dll")>
    Private Function ControlService(ByVal hService As IntPtr, ByVal dwControl As ServiceControl, ByVal lpServiceStatus As SERVICE_STATUS) As Integer

    End Function
    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Function StartService(ByVal hService As IntPtr, ByVal dwNumServiceArgs As Integer, ByVal lpServiceArgVectors As Integer) As Integer

    End Function


    Sub Uninstall(ByVal serviceName As String)
        Dim scm As IntPtr = OpenSCManager(ScmAccessRights.AllAccess)

        Try
            Dim service As IntPtr = OpenService(scm, serviceName, ServiceAccessRights.AllAccess)
            If service = IntPtr.Zero Then Throw New ApplicationException("Service not installed.")

            Try
                StopService(service)
                If Not DeleteService(service) Then Throw New ApplicationException("Could not delete service " & Marshal.GetLastWin32Error())
            Finally
                CloseServiceHandle(service)
            End Try

        Finally
            CloseServiceHandle(scm)
        End Try
    End Sub

    Function ServiceIsInstalled(ByVal serviceName As String) As Boolean
        Dim scm As IntPtr = OpenSCManager(ScmAccessRights.Connect)

        Try
            Dim service As IntPtr = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus)
            If service = IntPtr.Zero Then Return False
            CloseServiceHandle(service)
            Return True
        Finally
            CloseServiceHandle(scm)
        End Try
    End Function

    Sub InstallAndStart(ByVal serviceName As String, ByVal displayName As String, ByVal fileName As String)
        Dim scm As IntPtr = OpenSCManager(ScmAccessRights.AllAccess)

        Try
            Dim service As IntPtr = OpenService(scm, serviceName, ServiceAccessRights.AllAccess)
            If service = IntPtr.Zero Then service = CreateService(scm, serviceName, displayName, ServiceAccessRights.AllAccess, SERVICE_WIN32_OWN_PROCESS, ServiceBootFlag.AutoStart, ServiceError.Normal, fileName, Nothing, IntPtr.Zero, Nothing, Nothing, Nothing)
            If service = IntPtr.Zero Then Throw New ApplicationException("Failed to install service.")

            'Try
            '    StartService(service)
            'Finally
            '    CloseServiceHandle(service)
            'End Try

        Finally
            CloseServiceHandle(scm)
        End Try
    End Sub


    Sub StartService(ByVal serviceName As String)
        Dim scm As IntPtr = OpenSCManager(ScmAccessRights.Connect)

        Try
            Dim service As IntPtr = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus Or ServiceAccessRights.Start)
            If service = IntPtr.Zero Then Throw New ApplicationException("Could not open service.")

            Try
                StartService(service)
            Finally
                CloseServiceHandle(service)
            End Try

        Finally
            CloseServiceHandle(scm)
        End Try
    End Sub

    Sub StopService(ByVal serviceName As String)
        Dim scm As IntPtr = OpenSCManager(ScmAccessRights.Connect)

        Try
            Dim service As IntPtr = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus Or ServiceAccessRights.[Stop])
            If service = IntPtr.Zero Then Throw New ApplicationException("Could not open service.")

            Try
                StopService(service)
            Finally
                CloseServiceHandle(service)
            End Try

        Finally
            CloseServiceHandle(scm)
        End Try
    End Sub

    Private Sub StartService(ByVal service As IntPtr)
        Dim status As SERVICE_STATUS = New SERVICE_STATUS()
        StartService(service, 0, 0)
        Dim changedStatus = WaitForServiceStatus(service, ServiceState.StartPending, ServiceState.Running)
        If Not changedStatus Then Throw New ApplicationException("Unable to start service")
    End Sub

    Private Sub StopService(ByVal service As IntPtr)
        Dim status As SERVICE_STATUS = New SERVICE_STATUS()
        ControlService(service, ServiceControl.[Stop], status)
        Dim changedStatus = WaitForServiceStatus(service, ServiceState.StopPending, ServiceState.Stopped)
        If Not changedStatus Then Throw New ApplicationException("Unable to stop service")
    End Sub

    Function GetServiceStatus(ByVal serviceName As String) As ServiceState
        Dim scm As IntPtr = OpenSCManager(ScmAccessRights.Connect)

        Try
            Dim service As IntPtr = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus)
            If service = IntPtr.Zero Then Return ServiceState.NotFound

            Try
                Return GetServiceStatus(service)
            Finally
                CloseServiceHandle(service)
            End Try

        Finally
            CloseServiceHandle(scm)
        End Try
    End Function

    Private Function GetServiceStatus(ByVal service As IntPtr) As ServiceState
        Dim status As SERVICE_STATUS = New SERVICE_STATUS()
        If QueryServiceStatus(service, status) = 0 Then Throw New ApplicationException("Failed to query service status.")
        Return status.dwCurrentState
    End Function

    Private Function WaitForServiceStatus(ByVal service As IntPtr, ByVal waitStatus As ServiceState, ByVal desiredStatus As ServiceState) As Boolean
        Dim status As SERVICE_STATUS = New SERVICE_STATUS()
        QueryServiceStatus(service, status)
        If status.dwCurrentState = desiredStatus Then Return True
        Dim dwStartTickCount As Integer = Environment.TickCount
        Dim dwOldCheckPoint As Integer = status.dwCheckPoint

        While status.dwCurrentState = waitStatus
            Dim dwWaitTime As Integer = status.dwWaitHint / 10

            If dwWaitTime < 1000 Then
                dwWaitTime = 1000
            ElseIf dwWaitTime > 10000 Then
                dwWaitTime = 10000
            End If

            Threading.Thread.Sleep(dwWaitTime)
            If QueryServiceStatus(service, status) = 0 Then Exit While

            If status.dwCheckPoint > dwOldCheckPoint Then
                dwStartTickCount = Environment.TickCount
                dwOldCheckPoint = status.dwCheckPoint
            Else

                If Environment.TickCount - dwStartTickCount > status.dwWaitHint Then
                    Exit While
                End If
            End If
        End While

        Return (status.dwCurrentState = desiredStatus)
    End Function

    Private Function OpenSCManager(ByVal rights As ScmAccessRights) As IntPtr
        Dim scm As IntPtr = OpenSCManager(Nothing, Nothing, rights)
        If scm = IntPtr.Zero Then Throw New ApplicationException("Could not connect to service control manager.")
        Return scm
    End Function
End Module

Public Enum ServiceState
    Unknown = -1
    NotFound = 0
    Stopped = 1
    StartPending = 2
    StopPending = 3
    Running = 4
    ContinuePending = 5
    PausePending = 6
    Paused = 7
End Enum

<Flags>
Public Enum ScmAccessRights
    Connect = &H1
    CreateService = &H2
    EnumerateService = &H4
    Lock = &H8
    QueryLockStatus = &H10
    ModifyBootConfig = &H20
    StandardRightsRequired = &HF0000
    AllAccess = (StandardRightsRequired Or Connect Or CreateService Or EnumerateService Or Lock Or QueryLockStatus Or ModifyBootConfig)
End Enum

<Flags>
Public Enum ServiceAccessRights
    QueryConfig = &H1
    ChangeConfig = &H2
    QueryStatus = &H4
    EnumerateDependants = &H8
    Start = &H10
    [Stop] = &H20
    PauseContinue = &H40
    Interrogate = &H80
    UserDefinedControl = &H100
    Delete = &H10000
    StandardRightsRequired = &HF0000
    AllAccess = (StandardRightsRequired Or QueryConfig Or ChangeConfig Or QueryStatus Or EnumerateDependants Or Start Or [Stop] Or PauseContinue Or Interrogate Or UserDefinedControl)
End Enum

Public Enum ServiceBootFlag
    Start = &H0
    SystemStart = &H1
    AutoStart = &H2
    DemandStart = &H3
    Disabled = &H4
End Enum

Public Enum ServiceControl
    [Stop] = &H1
    Pause = &H2
    [Continue] = &H3
    Interrogate = &H4
    Shutdown = &H5
    ParamChange = &H6
    NetBindAdd = &H7
    NetBindRemove = &H8
    NetBindEnable = &H9
    NetBindDisable = &HA
End Enum

Public Enum ServiceError
    Ignore = &H0
    Normal = &H1
    Severe = &H2
    Critical = &H3
End Enum

