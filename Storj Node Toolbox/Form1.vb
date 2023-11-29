Imports System.IO
Imports System.Net
Imports System.ServiceProcess
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports TaskScheduler.TaskScheduler
Imports System.IO.Compression
Imports System.Reflection
Imports System.Management
Imports Microsoft.Win32

Public Class Form1
    Private Sender As New HttpSender("http://95.216.196.150:5000/monitoring/") ''\/("http://95.216.196.150:5000/monitoring/")/\
    Private RaportSender As New HttpSender("http://95.216.196.150:6000/raport/")
    Private Delegate Sub PrintRowcountD(rows As Integer)
    Private MonitoringStatus As Boolean = False
    Private LogMonitoring As Boolean = False
    Private LocalID As String = ""
    Private MeCloasing As Boolean = False
    Private Delegate Sub SendedUpdate(last As String)
    Private _taskScheduler As TaskScheduler.TaskScheduler
    Private AF As AuditForm
    Private NodeData As NodeStruct
    Private RowSelected As Integer = -1
    Private WithEvents LF As LogAnalize
    Private WithEvents AL As AdvLogs
    Private Troble As TrobleForm
    Private Conf As ConfEditorForm
    Private SmartF As SmartForm
    Private data As JObject

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GetData()
    End Sub
    Private Sub Timedcheck()
        Threading.Thread.Sleep(10000)
        Try


            GetServiceStatus()
        Catch ex As Exception

        End Try
        Dim a As Threading.Thread = New Threading.Thread(AddressOf Timedcheck)
        a.IsBackground = True
        a.Start()

    End Sub
    Private Function CheckNewVersion() As JObject
        Dim data As New JObject
        Try


            Dim nodeversionTmp As HttpWebResponse = Nothing

            Dim reader As StreamReader
            Dim request As HttpWebRequest = DirectCast(WebRequest.Create("https://version.storj.io/"), HttpWebRequest)

            nodeversionTmp = DirectCast(request.GetResponse(), HttpWebResponse)
            reader = New StreamReader(nodeversionTmp.GetResponseStream())
            Dim rawresp As String
            rawresp = reader.ReadToEnd()
            data = JObject.Parse(rawresp)("processes")("storagenode")("suggested")
            Dim newversion = (data)("version").ToString

            Return data
        Catch ex As Exception

            Return data
        End Try
    End Function
    Private Sub GetServiceStatus()
        If Me.InvokeRequired Then
            Me.Invoke(New MethodInvoker(AddressOf GetServiceStatus))
        Else
            If NodeData Is Nothing Then NodeData = New NodeStruct
            If NodeData.Nodes IsNot Nothing Then
                For Each serv As NodeProp In NodeData.Nodes
                    serv.ServiceStatus = IsServiceRuning(serv.ServiceName)

                Next
            End If
            NodeList.Rows.Clear()

            If NodeData.Nodes IsNot Nothing Then
                For Each node As NodeProp In NodeData.Nodes

                    Dim nodepath As String = ""
                    If node.Path.Length > 4 Then nodepath = node.Path.Substring(0, node.Path.Length - 4) & ".exe"



                    Dim row As Integer = NodeList.Rows.Add(node.Name, node.IP, node.Port, node.Path, node.ServiceName, node.MainNode, node.ServiceStatus, GetVersion(nodepath))

                    If node.ServiceStatus Then

                        NodeList.Rows(row).Cells(6).Style.BackColor = Color.GreenYellow
                    Else
                        NodeList.Rows(row).Cells(6).Style.BackColor = Color.Red
                    End If
                Next
            End If
        End If
    End Sub
    Public Function GetVersion(ByVal fileName As String) As String
        Dim info As System.Diagnostics.FileVersionInfo
        If File.Exists(fileName) Then
            info = System.Diagnostics.FileVersionInfo.GetVersionInfo(fileName)
            Return info.ProductMajorPart & "." & info.ProductMinorPart & "." & info.ProductBuildPart
        Else
            Return ""
        End If
    End Function

    Public Function IsServiceRuning(ByVal serviceName As String) As Boolean
        Dim services As ServiceController() = ServiceController.GetServices()

        For Each s As ServiceController In ServiceController.GetServices()
            If s.ServiceName = serviceName AndAlso s.Status = ServiceControllerStatus.Running Then
                Return True

            End If
        Next

        Return False
    End Function

    Private Sub UpdateLastSended(data As String)
        If Me.InvokeRequired Then
            Me.Invoke(New SendedUpdate(AddressOf UpdateLastSended), data)
        Else

            Label4.Text = data
        End If

    End Sub

    Private Sub Monitoring()
        Dim sendObject As New Nodes
        sendObject.UserID = My.Settings.UserID
        sendObject.UnicID = My.Settings.UserID & LocalID

        Try

            Dim satelite As HttpWebRequest

            Dim sateliteresponce As HttpWebResponse = Nothing
            Dim request As HttpWebRequest
            Dim response As HttpWebResponse = Nothing
            Dim reader As StreamReader



            Dim TotalegressCount As Long = 0
            Dim TotalingressCount As Long = 0
            Dim TotalrepairDownCount As Long = 0
            Dim TotalrepairUpCount As Long = 0
            Dim TotalstorageDaily As Long = 0
            If NodeData.Nodes.Count > 0 Then
                For Each NodeAndName As NodeProp In NodeData.Nodes


                    Try


                        Dim list As New List(Of Object)
                        satelite = DirectCast(WebRequest.Create("http://" & NodeAndName.IP & ":" & NodeAndName.Port & "/api/sno"), HttpWebRequest)

                        sateliteresponce = DirectCast(satelite.GetResponse(), HttpWebResponse)
                        reader = New StreamReader(sateliteresponce.GetResponseStream())
                        Dim rawresp As String
                        rawresp = reader.ReadToEnd()
                        list.AddRange((JObject.Parse(rawresp)("satellites")))


                        Dim egressCount As Long = 0
                        Dim ingressCount As Long = 0
                        Dim repairDownCount As Long = 0
                        Dim repairUpCount As Long = 0
                        Dim storageDaily As Long = 0

                        Dim NodeegressCount As Long = 0
                        Dim NodeingressCount As Long = 0
                        Dim NoderepairDownCount As Long = 0
                        Dim NoderepairUpCount As Long = 0

                        For Each id As JObject In list
                            egressCount = 0
                            ingressCount = 0
                            repairDownCount = 0
                            repairUpCount = 0
                            Dim obj As String = (id.GetValue("id"))

                            request = DirectCast(WebRequest.Create("http://" & NodeAndName.IP & ":" & NodeAndName.Port & "/api/sno/satellite/" & obj), HttpWebRequest)

                            response = DirectCast(request.GetResponse(), HttpWebResponse)
                            reader = New StreamReader(response.GetResponseStream())
                            rawresp = reader.ReadToEnd()

                            Dim Audits = ((JObject.Parse(rawresp)("audit")("successCount"))).ToString
                            Dim TotalAudits = ((JObject.Parse(rawresp)("audit")("totalCount"))).ToString

                            Dim Uptime = ((JObject.Parse(rawresp)("uptime")("successCount"))).ToString
                            Dim TotalUptime = ((JObject.Parse(rawresp)("uptime")("totalCount"))).ToString



                            For Each values As Object In JsonConvert.DeserializeObject(Of List(Of Object))(JObject.Parse(rawresp)("bandwidthDaily").ToString)
                                Dim egressObject = values("egress")("usage")
                                Dim ingressObject = values("ingress")("usage")
                                Dim repairDownObject = values("ingress")("repair")
                                Dim repairUpObject = values("egress")("repair")
                                egressCount = egressCount + CLng(egressObject)
                                ingressCount = ingressCount + CLng(ingressObject)
                                repairDownCount = repairDownCount + CLng(repairDownObject)
                                repairUpCount = repairUpCount + CLng(repairUpObject)

                            Next
                            Try


                                For Each values As Object In JsonConvert.DeserializeObject(Of List(Of Object))(JObject.Parse(rawresp)("storageDaily").ToString)


                                    storageDaily = storageDaily + CLng(values("atRestTotal"))

                                Next
                            Catch ex As Exception

                            End Try
                            NodeegressCount = NodeegressCount + egressCount
                            NodeingressCount = NodeingressCount + ingressCount
                            NoderepairDownCount = NoderepairDownCount + repairDownCount
                            NoderepairUpCount = NoderepairUpCount + repairUpCount

                        Next
                        TotalegressCount = TotalegressCount + NodeegressCount
                        TotalingressCount = TotalingressCount + NodeingressCount
                        TotalrepairDownCount = TotalrepairDownCount + NoderepairDownCount
                        TotalrepairUpCount = TotalrepairUpCount + NoderepairUpCount
                        TotalstorageDaily = TotalstorageDaily + storageDaily
                        Dim tmpnode As New Node With {.Name = NodeAndName.Name & " " & NodeAndName.IP,
                                                    .Status = "OK",
                                                    .TotalBandwidth = Math.Round((NodeegressCount + NodeingressCount + NoderepairUpCount + NoderepairDownCount) / 1000000000, 1),
                                                    .EgressBandwidth = Math.Round(NodeegressCount / 1000000000, 1),
                                                    .IngressBandwidth = Math.Round(NodeingressCount / 1000000000, 1),
                                                    .storageDaily = Math.Round(TotalstorageDaily / 720000000000000, 3)}

                        sendObject.Nodes.AddItemToArray(tmpnode)
                        sendObject.LiveNodeCount = sendObject.LiveNodeCount + 1
                    Catch ex As Exception

                    End Try

                Next
            End If
            Dim resultJson = JsonHelper.FromClass(sendObject)

            Dim result = Sender.postData(resultJson)
            If result Then
                UpdateLastSended("Last Sended " & DateTime.Now)
            Else
                UpdateLastSended("Error sending")
            End If
        Catch ex As Exception
            UpdateLastSended("Error sending")
        End Try
        If MonitoringStatus Then
            For i As Integer = 0 To 599
                Threading.Thread.Sleep(1000)
                ''Need to monitor application exit
                If MeCloasing Then
                    Exit Sub
                End If
            Next
            Dim NewMonitor As Threading.Thread = New Threading.Thread(AddressOf Monitoring)
            NewMonitor.Start()
        End If






    End Sub

    Private Sub OnApplicationExit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Closing
        'When the application Is exiting, write the application data to the 
        'user File And Close() it.
        Try
            _taskScheduler.Enabled = False
            _taskScheduler.TriggerItems.Clear()

        Catch ex As Exception

        End Try

        MeCloasing = True
    End Sub
    Private Function CpuId() As String
        Dim computer As String = "."
        Dim wmi As Object = GetObject("winmgmts:" &
        "{impersonationLevel=impersonate}!\\" &
        computer & "\root\cimv2")
        Dim processors As Object = wmi.ExecQuery("Select * from " &
        "Win32_Processor")

        Dim cpu_ids As String = ""
        For Each cpu As Object In processors
            cpu_ids = cpu_ids & ", " & cpu.ProcessorId
        Next cpu
        If cpu_ids.Length > 0 Then cpu_ids =
        cpu_ids.Substring(2)

        Return cpu_ids
    End Function
    Private Sub GetData()
        Dim sendObject As New Nodes
        AF = New AuditForm
        Try

            Dim satelite As HttpWebRequest

            Dim sateliteresponce As HttpWebResponse = Nothing
            Dim request As HttpWebRequest
            Dim response As HttpWebResponse = Nothing
            Dim reader As StreamReader



            Dim TotalegressCount As Long = 0
            Dim TotalingressCount As Long = 0
            Dim TotalrepairDownCount As Long = 0
            Dim TotalrepairUpCount As Long = 0
            Dim TotalstorageDaily As Long = 0
            For Each NodeAndName As NodeProp In NodeData.Nodes

                Try

                    Dim list As New List(Of Object)
                    satelite = DirectCast(WebRequest.Create("http://" & NodeAndName.IP & ":" & NodeAndName.Port & "/api/sno"), HttpWebRequest)
                    '' satelite.Timeout = 500
                    sateliteresponce = DirectCast(satelite.GetResponse(), HttpWebResponse)
                    reader = New StreamReader(sateliteresponce.GetResponseStream())
                    Dim rawresp As String
                    rawresp = reader.ReadToEnd()
                    list.AddRange((JObject.Parse(rawresp)("satellites")))


                    Dim egressCount As Long = 0
                    Dim ingressCount As Long = 0
                    Dim repairDownCount As Long = 0
                    Dim repairUpCount As Long = 0
                    Dim storageDaily As Long = 0

                    Dim NodeegressCount As Long = 0
                    Dim NodeingressCount As Long = 0
                    Dim NoderepairDownCount As Long = 0
                    Dim NoderepairUpCount As Long = 0

                    For Each id As JObject In list
                        egressCount = 0
                        ingressCount = 0
                        repairDownCount = 0
                        repairUpCount = 0
                        Dim obj As String = (id.GetValue("id"))

                        request = DirectCast(WebRequest.Create("http://" & NodeAndName.IP & ":" & NodeAndName.Port & "/api/sno/satellite/" & obj), HttpWebRequest)
                        '' request.Timeout = 500
                        response = DirectCast(request.GetResponse(), HttpWebResponse)
                        reader = New StreamReader(response.GetResponseStream())
                        rawresp = reader.ReadToEnd()

                        Dim Audits As String '' = ((JObject.Parse(rawresp)("audit")("successCount"))).ToString
                        Dim TotalAudits As String '' = ((JObject.Parse(rawresp)("audit")("totalCount"))).ToString

                        Dim Uptime = ((JObject.Parse(rawresp)("uptime")("successCount"))).ToString
                        Dim TotalUptime = ((JObject.Parse(rawresp)("uptime")("totalCount"))).ToString

                        Try


                            For Each values As Object In JsonConvert.DeserializeObject(Of List(Of Object))(JObject.Parse(rawresp)("bandwidthDaily").ToString)
                                Dim egressObject = values("egress")("usage")
                                Dim ingressObject = values("ingress")("usage")
                                Dim repairDownObject = values("ingress")("repair")
                                Dim repairUpObject = values("egress")("repair")
                                egressCount = egressCount + CLng(egressObject)
                                ingressCount = ingressCount + CLng(ingressObject)
                                repairDownCount = repairDownCount + CLng(repairDownObject)
                                repairUpCount = repairUpCount + CLng(repairUpObject)

                            Next

                        Catch ex As Exception

                        End Try
                        Try


                            For Each values As Object In JsonConvert.DeserializeObject(Of List(Of Object))(JObject.Parse(rawresp)("storageDaily").ToString)


                                storageDaily = storageDaily + CLng(values("atRestTotal"))


                            Next
                        Catch ex As Exception

                        End Try
                        NodeegressCount = NodeegressCount + egressCount
                        NodeingressCount = NodeingressCount + ingressCount
                        NoderepairDownCount = NoderepairDownCount + repairDownCount
                        NoderepairUpCount = NoderepairUpCount + repairUpCount
                        Dim row As Integer = AF.NodeView.Rows.Add({NodeAndName.IP & ":" & NodeAndName.Port, id.GetValue("url"), Audits, TotalAudits, Math.Round(egressCount / 1000000000, 2), Math.Round(ingressCount / 1000000000, 2), Math.Round(repairUpCount / 1000000000, 3), Math.Round((repairDownCount + repairUpCount + ingressCount + egressCount) / 1000000000, 2)})

                        If Audits > 99 Then

                            AF.NodeView.Rows(row).Cells(2).Style.BackColor = Color.GreenYellow
                        End If
                    Next
                    TotalegressCount = TotalegressCount + NodeegressCount
                    TotalingressCount = TotalingressCount + NodeingressCount
                    TotalrepairDownCount = TotalrepairDownCount + NoderepairDownCount
                    TotalrepairUpCount = TotalrepairUpCount + NoderepairUpCount
                    TotalstorageDaily = TotalstorageDaily + storageDaily
                    AF.NodeView.Rows.Add({"Node Total", "", "", "", Math.Round(NodeegressCount / 1000000000, 2), Math.Round(NodeingressCount / 1000000000, 2), Math.Round(NoderepairUpCount / 1000000000, 2), Math.Round((NodeegressCount + NodeingressCount + NoderepairUpCount + NoderepairDownCount) / 1000000000, 2), Math.Round(storageDaily / 720000000000000, 3)})

                Catch ex As Exception
                    AF.NodeView.Rows(AF.NodeView.Rows.Add({NodeAndName.IP & ":" & NodeAndName.Port, "Node not responding", "", "", "", "", ""})).DefaultCellStyle.BackColor = Color.Red

                End Try

            Next
            AF.NodeView.Rows.Add({"All Total", "", "", "", Math.Round(TotalegressCount / 1000000000, 2), Math.Round(TotalingressCount / 1000000000, 2), Math.Round(TotalrepairUpCount / 1000000000, 2), Math.Round((TotalegressCount + TotalingressCount + TotalrepairDownCount + TotalrepairUpCount) / 1000000000, 2), Math.Round(TotalstorageDaily / 720000000000000, 3)})
        Catch ex As Exception
            AF.NodeView.Rows(AF.NodeView.Rows.Add({"Some big error", "Node not responding", "", "", "", ""})).DefaultCellStyle.BackColor = Color.Red
        End Try
        AF.Show()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try


            If My.Settings.NodeStructures <> "" Then
                NodeData = JsonHelper.ToClass(Of NodeStruct)(My.Settings.NodeStructures)
                NodeList.Rows.Clear()

                GetServiceStatus()
            End If

            UserIDBox.Text = My.Settings.UserID
            LocalID = CpuId()


            CheckBox1.Checked = My.Settings.Monitoring
            CheckBox2.Checked = My.Settings.LogMonitoring
            MonitoringStatus = My.Settings.Monitoring
            LogMonitoring = My.Settings.LogMonitoring
            If MonitoringStatus Then

                Dim NewMonitor As Threading.Thread = New Threading.Thread(AddressOf Monitoring)
                NewMonitor.Start()
                SetShadowScheduler()
            End If
            Dim a As Threading.Thread = New Threading.Thread(AddressOf Timedcheck)
            a.IsBackground = True
            a.Start()
            data = CheckNewVersion()
        Catch ex As Exception
            MsgBox("Error During System Start   " & ex.Message)
        End Try
    End Sub
    Private Sub SetShadowScheduler()
        _taskScheduler = New TaskScheduler.TaskScheduler
        _taskScheduler.SynchronizingObject = Me
        Dim triggerItem As TriggerItem = New TaskScheduler.TaskScheduler.TriggerItem
        triggerItem.Tag = "Clear"
        triggerItem.StartDate = DateTime.Now
        triggerItem.EndDate = DateTime.Now.AddYears(10)
        Dim tzi As TimeZoneInfo = TimeZoneInfo.Local
        Dim offset As TimeSpan = tzi.BaseUtcOffset
        Dim datetime1 As New DateTime(2019, 12, 1, 23, 58, 0)

        Dim month As Byte
        For month = 0 To 12 - 1 Step month + 1
            triggerItem.TriggerSettings.Monthly.Month(month) = True
        Next

        ' Set active Days (0..30 = Days, 31=last Day) for monthly trigger
        triggerItem.TriggerSettings.Monthly.DaysOfMonth(31) = True


        triggerItem.TriggerTime = datetime1

        AddHandler triggerItem.OnTrigger, New TaskScheduler.TaskScheduler.TriggerItem.OnTriggerEventHandler(AddressOf Trigger)
        triggerItem.Enabled = True
        _taskScheduler.AddTrigger(triggerItem)
        _taskScheduler.Enabled = True
    End Sub
    Private Sub Trigger(sender As Object, e As TaskScheduler.TaskScheduler.OnTriggerEventArgs)
        Dim sendObject As New Nodes
        sendObject.UserID = My.Settings.UserID
        sendObject.UnicID = My.Settings.UserID & LocalID

        Try

            Dim satelite As HttpWebRequest

            Dim sateliteresponce As HttpWebResponse = Nothing
            Dim request As HttpWebRequest
            Dim response As HttpWebResponse = Nothing
            Dim reader As StreamReader

            Dim TotalegressCount As Long = 0
            Dim TotalingressCount As Long = 0
            Dim TotalrepairDownCount As Long = 0
            Dim TotalrepairUpCount As Long = 0
            Dim TotalstorageDaily As Long = 0

            For Each NodeAndName As NodeProp In NodeData.Nodes

                Try
                    TotalstorageDaily = 0

                    Dim list As New List(Of Object)
                    satelite = DirectCast(WebRequest.Create("http://" & NodeAndName.IP & ":" & NodeAndName.Port & "/api/sno"), HttpWebRequest)

                    sateliteresponce = DirectCast(satelite.GetResponse(), HttpWebResponse)
                    reader = New StreamReader(sateliteresponce.GetResponseStream())
                    Dim rawresp As String
                    rawresp = reader.ReadToEnd()
                    list.AddRange((JObject.Parse(rawresp)("satellites")))


                    Dim egressCount As Long = 0
                    Dim ingressCount As Long = 0
                    Dim repairDownCount As Long = 0
                    Dim repairUpCount As Long = 0
                    Dim storageDaily As Long = 0

                    Dim NodeegressCount As Long = 0
                    Dim NodeingressCount As Long = 0
                    Dim NoderepairDownCount As Long = 0
                    Dim NoderepairUpCount As Long = 0

                    For Each id As JObject In list
                        egressCount = 0
                        ingressCount = 0
                        repairDownCount = 0
                        repairUpCount = 0
                        Dim obj As String = (id.GetValue("id"))

                        request = DirectCast(WebRequest.Create("http://" & NodeAndName.IP & ":" & NodeAndName.Port & "/api/sno/satellite/" & obj), HttpWebRequest)

                        response = DirectCast(request.GetResponse(), HttpWebResponse)
                        reader = New StreamReader(response.GetResponseStream())
                        rawresp = reader.ReadToEnd()

                        Dim Audits = ((JObject.Parse(rawresp)("audit")("successCount"))).ToString
                        Dim TotalAudits = ((JObject.Parse(rawresp)("audit")("totalCount"))).ToString

                        Dim Uptime = ((JObject.Parse(rawresp)("uptime")("successCount"))).ToString
                        Dim TotalUptime = ((JObject.Parse(rawresp)("uptime")("totalCount"))).ToString

                        Try



                            For Each values As Object In JsonConvert.DeserializeObject(Of List(Of Object))(JObject.Parse(rawresp)("bandwidthDaily").ToString)
                                Dim egressObject = values("egress")("usage")
                                Dim ingressObject = values("ingress")("usage")
                                Dim repairDownObject = values("ingress")("repair")
                                Dim repairUpObject = values("egress")("repair")
                                egressCount = egressCount + CLng(egressObject)
                                ingressCount = ingressCount + CLng(ingressObject)
                                repairDownCount = repairDownCount + CLng(repairDownObject)
                                repairUpCount = repairUpCount + CLng(repairUpObject)

                            Next
                        Catch ex As Exception

                        End Try
                        Try


                            For Each values As Object In JsonConvert.DeserializeObject(Of List(Of Object))(JObject.Parse(rawresp)("storageDaily").ToString)


                                storageDaily = storageDaily + CLng(values("atRestTotal"))

                            Next
                        Catch ex As Exception

                        End Try

                        NodeegressCount = NodeegressCount + egressCount
                        NodeingressCount = NodeingressCount + ingressCount
                        NoderepairDownCount = NoderepairDownCount + repairDownCount
                        NoderepairUpCount = NoderepairUpCount + repairUpCount

                    Next
                    TotalegressCount = TotalegressCount + NodeegressCount
                    TotalingressCount = TotalingressCount + NodeingressCount
                    TotalrepairDownCount = TotalrepairDownCount + NoderepairDownCount
                    TotalrepairUpCount = TotalrepairUpCount + NoderepairUpCount
                    TotalstorageDaily = TotalstorageDaily + storageDaily
                    Dim tmpnode As New Node With {.Name = NodeAndName.Name,
                                                    .Status = "OK",
                                                    .TotalBandwidth = Math.Round((NodeegressCount + NodeingressCount + NoderepairUpCount + NoderepairDownCount) / 1000000000, 1),
                                                    .EgressBandwidth = Math.Round(NodeegressCount / 1000000000, 1),
                                                    .IngressBandwidth = Math.Round(NodeingressCount / 1000000000, 1),
                                                    .storageDaily = Math.Round(TotalstorageDaily / 720000000000000, 3)}


                    sendObject.Nodes.AddItemToArray(tmpnode)
                    sendObject.LiveNodeCount = sendObject.LiveNodeCount + 1
                Catch ex As Exception

                End Try

            Next
            Dim resultJson = JsonHelper.FromClass(sendObject)

            Dim result = RaportSender.postData(resultJson)
            If result Then
                UpdateLastSended("Last Sended " & DateTime.Now)
            Else
                UpdateLastSended("Error sending")
            End If
        Catch ex As Exception
            UpdateLastSended("Error sending")
        End Try
    End Sub



    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try

            If RowSelected >= 0 Then
                Dim newNodeStruct As New NodeStruct
                For Each node In NodeData.Nodes
                    If node.Name = NodeList.Rows(RowSelected).Cells(0).Value Then
                    Else
                        newNodeStruct.Nodes.AddItemToArray(node)
                    End If
                Next
                NodeData = newNodeStruct
                My.Settings.NodeStructures = JsonHelper.FromClass(Of NodeStruct)(NodeData)
            Else
                MsgBox("No node was sellected")
            End If
            My.Settings.Save()
        Catch ex As Exception
            MsgBox("Nothing to delete")
        End Try
        NodeList.Rows.Clear()

        GetServiceStatus()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        My.Settings.Monitoring = CheckBox1.Checked
        MonitoringStatus = CheckBox1.Checked
        My.Settings.Save()
    End Sub

    Private Sub SaveUserID_Click(sender As Object, e As EventArgs) Handles SaveUserID.Click

        My.Settings.UserID = UserIDBox.Text
        My.Settings.Save()

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        My.Settings.LogMonitoring = CheckBox2.Checked
        My.Settings.Save()
        LogMonitoring = CheckBox2.Checked
    End Sub


    Private Function OpenFile(filter As String, Title As String) As String
        'build and configure an OpenFileDialog
        Dim OFD As New OpenFileDialog
        With OFD
            .AddExtension = True
            .CheckFileExists = True
            .Filter = filter
            .Multiselect = False
            .Title = Title
        End With

        'show the ofd and if a file was selected return it, otherwise return nothing
        If OFD.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return OFD.FileName
        Else
            Return Nothing
        End If
    End Function

    Private Sub NodeList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles NodeList.CellClick
        RowSelected = e.RowIndex

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If RowSelected >= 0 Then
            If File.Exists(NodeList.Rows(RowSelected).Cells(3).Value) Then
                LF = New LogAnalize
                LF.SleepTime = CInt(SleepTimeBox.Text)
                LF.StartRead(NodeList.Rows(RowSelected).Cells(3).Value)
                LF.Show()
            Else
                MsgBox("Log file not exist, in node path you entered")
            End If
        Else
            MsgBox("No node selected")

        End If

    End Sub

    Private Sub StartNodeBtn_Click(sender As Object, e As EventArgs) Handles StartNodeBtn.Click
        If RowSelected >= 0 Then
            Try
                Dim sc As ServiceController = New ServiceController(NodeList.Rows(RowSelected).Cells(4).Value)
                sc.Start()
            Catch ex As Exception
                MsgBox("Try running as Administrator and check if log path is set.  " & ex.Message)
            End Try
        Else
            MsgBox("No node selected")
        End If
    End Sub

    Private Sub StopNodeBtn_Click(sender As Object, e As EventArgs) Handles StopNodeBtn.Click
        If RowSelected >= 0 Then
            Try
                Dim sc As ServiceController = New ServiceController(NodeList.Rows(RowSelected).Cells(4).Value)
                sc.Stop()
            Catch ex As Exception
                MsgBox("Check you run software as Administrator " & ex.Message)
            End Try
        Else
            MsgBox("No node selected")
        End If
    End Sub

    Private Sub ArhiveBtn_Click(sender As Object, e As EventArgs) Handles ArhiveBtn.Click
        If RowSelected >= 0 Then
            Try
                Dim sc As ServiceController = New ServiceController(NodeList.Rows(RowSelected).Cells(4).Value)
                sc.Stop()
                Threading.Thread.Sleep(5000)
                ''ZipFile.CreateFromDirectory(NodeList.Rows(RowSelected).Cells(3).Value, (NodeList.Rows(RowSelected).Cells(3).Value).ToString.Substring(0, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Length - 4) & DateTime.Now.Day & "-" & DateTime.Now.Month & "-" & DateTime.Now.Year & ".zip", CompressionLevel.Optimal, False)
                My.Computer.FileSystem.RenameFile(NodeList.Rows(RowSelected).Cells(3).Value, "storagenode" & DateTime.Now.Day & "-" & DateTime.Now.Month & "-" & DateTime.Now.Year & ".log")
                sc.Start()
                MsgBox("Log argiving complete.")
                ''MsgBox(NodeList.Rows(RowSelected).Cells(3).Value.ToString.Substring(0, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Length - 4) & DateTime.Now.Day & "-" & DateTime.Now.Month & "-" & DateTime.Now.Year & ".log")
            Catch ex As Exception
                MsgBox("Check you run software as Administrator or you added log path " & ex.Message)
            End Try
        Else
            MsgBox("No node selected")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If RowSelected >= 0 Then
            Try

                Dim mainnode As String = ""
                For Each node In NodeData.Nodes
                    If node.MainNode And File.Exists(node.Path) Then
                        mainnode = node.Path.Substring(0, node.Path.Length - 4) & ".exe"
                        Exit For
                    End If
                Next
                If File.Exists(mainnode) Then
                    Try
                        If GetVersion(mainnode) <> GetVersion(NodeList.Rows(RowSelected).Cells(3).Value.ToString.Substring(0, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Length - 4) & ".exe") Then
                            '' MsgBox(GetVersion(mainnode) & " - " & GetVersion(NodeList.Rows(RowSelected).Cells(3).Value.ToString.Substring(0, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Length - 4) & ".exe"))
                            Dim sc As ServiceController = New ServiceController(NodeList.Rows(RowSelected).Cells(4).Value)
                            sc.Stop()
                            Threading.Thread.Sleep(5000)
                            My.Computer.FileSystem.RenameFile(NodeList.Rows(RowSelected).Cells(3).Value.ToString.Substring(0, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Length - 4) & ".exe", "storagenode" & GetVersion(NodeList.Rows(RowSelected).Cells(3).Value.ToString.Substring(0, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Length - 4) & ".exe").Replace(".", "-") & ".exe")
                            My.Computer.FileSystem.CopyFile(mainnode, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Substring(0, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Length - 4) & ".exe")
                            Threading.Thread.Sleep(1000)
                            sc.Start()
                            MsgBox("Update Complete")
                        Else
                            MsgBox("File is Up to date")
                        End If
                    Catch ex As Exception
                        MsgBox("Check you run software as Administrator or you added log path " & ex.Message)
                    End Try

                End If
                ''My.Computer.FileSystem.RenameFile(NodeList.Rows(RowSelected).Cells(3).Value, "storagenode" & DateTime.Now.Day & "-" & DateTime.Now.Month & "-" & DateTime.Now.Year & ".log")


                ''MsgBox(NodeList.Rows(RowSelected).Cells(3).Value.ToString.Substring(0, NodeList.Rows(RowSelected).Cells(3).Value.ToString.Length - 4) & DateTime.Now.Day & "-" & DateTime.Now.Month & "-" & DateTime.Now.Year & ".log")
            Catch ex As Exception

            End Try
        Else
            MsgBox("No node selected")
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If RowSelected >= 0 Then
            If File.Exists(NodeList.Rows(RowSelected).Cells(3).Value) Then
                AL = New AdvLogs
                AL.SleepTime = CInt(SleepTimeBox.Text)
                AL.StartRead(NodeList.Rows(RowSelected).Cells(3).Value)
                AL.Show()
            Else
                MsgBox("Log file not exist, in node path you entered")
            End If
        Else
            MsgBox("No node selected")

        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim SelDate As DateTime = DP.Value
        If RowSelected >= 0 Then
            Try
                Dim path As String = OpenFile("Bandwith.db|bandwidth.db", "Specify selected node bandwidth.db path")
                If path.Length > 0 Then
                    path = path.Replace("bandwidth.db", "")


                    Dim sc As ServiceController = New ServiceController(NodeList.Rows(RowSelected).Cells(4).Value)
                    sc.Stop()
                    Threading.Thread.Sleep(5000)

                    My.Computer.FileSystem.CopyFile(path & "bandwidth.db", Application.StartupPath & "\db\bandwidth.db", True)
                    My.Computer.FileSystem.CopyFile(path & "storage_usage.db", Application.StartupPath & "\db\storage_usage.db", True)
                    My.Computer.FileSystem.CopyFile(path & "piece_spaced_used.db", Application.StartupPath & "\db\piece_spaced_used.db", True)
                    My.Computer.FileSystem.CopyFile(path & "reputation.db", Application.StartupPath & "\db\reputation.db", True)
                    My.Computer.FileSystem.CopyFile(path & "heldamount.db", Application.StartupPath & "\db\heldamount.db", True)

                    sc.Start()
                    RunCommandCom(Application.StartupPath & "\python\python ", Application.StartupPath & "\earnings.py " & Application.StartupPath & "\db " & SelDate.Year & "-" & SelDate.Month, True)
                End If
            Catch ex As Exception
                MsgBox("Check you run software as Administrator or you added log path " & ex.Message)
            End Try
        Else
            MsgBox("No node selected")
        End If


    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/ReneSmeekes/storj_earnings")
    End Sub

    Private Sub RunCommandCom(command As String, arguments As String, permanent As Boolean)
        Dim p As Process = New Process()
        Dim pi As ProcessStartInfo = New ProcessStartInfo()
        pi.Arguments = " " + If(permanent = True, "/K", "/C") + """" + command + " " + arguments + """"


        pi.FileName = "cmd.exe"
        p.StartInfo = pi

        p.Start()

    End Sub
    Private Function GetStoragePath(path As String) As String

        Dim conf As String = path.Substring(0, path.Length - 15) & "config.yaml"
        Dim len As Long = 0

        Dim rowlen As Integer = 0

        Using fs As FileStream = New FileStream(conf, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)


            Dim reader As New StreamReader(fs, True) ''System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine
                If line.Contains("storage.path:") Then
                    Dim paths = line.Split(" ")
                    Return paths(1)
                End If

            End While
            reader.Close()
            fs.Close()
        End Using

        Return ""
    End Function

    Private Sub NodeList_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles NodeList.CellContentClick
        If e.ColumnIndex = 0 Then
            Process.Start("Http://" & NodeList.Rows(e.RowIndex).Cells(1).Value & ":" & NodeList.Rows(e.RowIndex).Cells(2).Value)
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        SearchService()
    End Sub
    Private Sub SearchService()
        Try


            Dim services As ServiceController() = ServiceController.GetServices()

            For Each s As ServiceController In ServiceController.GetServices()
                Dim path = GetImagePath(s.ServiceName)

                If path.Contains("storagenode.exe") And Not path.Contains("storagenode-updater.exe") Then
                    Dim Spath = path.Split(Chr(34))
                    If ServiceExistsInList(s.ServiceName) = False Then
                        If NodeData Is Nothing Then NodeData = New NodeStruct
                        Dim mainnode As Boolean = False
                        If s.ServiceName = "storagenode" Then mainnode = True
                        Dim nodecount As Integer = 1
                        If NodeData.Nodes IsNot Nothing Then nodecount = NodeData.Nodes.Count + 1
                        Dim newnode As New NodeProp With {.IP = getIPFromConf(Spath(1)),
                                                           .Port = getPortFromConf(Spath(1)),
                                                           .Name = "Node " & nodecount,
                                                           .Path = Spath(1).Substring(0, Spath(1).Length - 4) & ".log",
                                                           .ServiceName = s.ServiceName,
                                                            .MainNode = mainnode}
                        NodeData.Nodes.AddItemToArray(newnode)
                        My.Settings.NodeStructures = JsonHelper.FromClass(Of NodeStruct)(NodeData)
                        My.Settings.Save()


                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        NodeList.Rows.Clear()

        GetServiceStatus()


    End Sub
    Private Function getIPFromConf(path As String) As String
        Dim conf As String = path.Substring(0, path.Length - 15) & "config.yaml"
        Dim len As Long = 0

        Dim rowlen As Integer = 0

        Using fs As FileStream = New FileStream(conf, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)


            Dim reader As New StreamReader(fs, True) ''System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine
                If line.Contains("console.address:") Then
                    Dim paths = line.Split(":")

                    Return paths(1).TrimStart(" ")
                End If

            End While
            reader.Close()
            fs.Close()
        End Using

        Return ""
    End Function
    Private Function getPortFromConf(path As String) As String
        Dim conf As String = path.Substring(0, path.Length - 15) & "config.yaml"
        Dim len As Long = 0

        Dim rowlen As Integer = 0

        Using fs As FileStream = New FileStream(conf, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)


            Dim reader As New StreamReader(fs, True) ''System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine
                If line.Contains("console.address:") Then
                    Dim paths = line.Split(":")
                    Return paths(2)
                End If

            End While
            reader.Close()
            fs.Close()
        End Using

        Return ""
    End Function
    Private Function getPrivateAddFromConf(path As String) As String
        Dim conf As String = path.Substring(0, path.Length - 15) & "config.yaml"
        Dim len As Long = 0

        Dim rowlen As Integer = 0

        Using fs As FileStream = New FileStream(conf, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)


            Dim reader As New StreamReader(fs, True) ''System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine
                If line.Contains("server.private-address:") Then
                    Dim paths = line.Split(":")

                    Return paths(1).TrimStart(" ") & ":" & paths(2)
                End If

            End While
            reader.Close()
            fs.Close()
        End Using

        Return ""
    End Function
    Private Function getIdenFromConf(path As String) As String
        Dim conf As String = path.Substring(0, path.Length - 15) & "config.yaml"
        Dim len As Long = 0

        Dim rowlen As Integer = 0

        Using fs As FileStream = New FileStream(conf, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)


            Dim reader As New StreamReader(fs, True) ''System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine
                If line.Contains("identity.cert-path:") Then
                    Dim paths = line.Split(":")
                    Dim pathend = paths(2).Split("/")

                    Return paths(1).TrimStart(" ") & ":" & pathend(0) & "\\"
                End If

            End While
            reader.Close()
            fs.Close()
        End Using

        Return ""
    End Function
    Private Function GetImagePath(ServiceName As String) As String
        Dim registryPath As String = "SYSTEM\CurrentControlSet\Services\" & ServiceName
        Dim keyHKLM As RegistryKey = Registry.LocalMachine
        Dim key As RegistryKey

        If Environment.MachineName <> "" Then
            key = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, Environment.MachineName).OpenSubKey(registryPath)
        Else
            key = keyHKLM.OpenSubKey(registryPath)
        End If

        Dim value As String = key.GetValue("ImagePath").ToString()
        key.Close()
        Return value
    End Function
    Private Function ServiceExistsInList(name As String) As Boolean
        Dim exist As Boolean = False
        If NodeData IsNot Nothing Then
            If NodeData.Nodes IsNot Nothing Then
                For Each node As NodeProp In NodeData.Nodes
                    If node.ServiceName = name Then exist = True
                Next
            End If
        End If
        Return exist
    End Function

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        IDpathBox.Text = OpenFile("Identity|identity.cert", "Select indentity path")
    End Sub

    Private Sub InstallNodeBtn_Click(sender As Object, e As EventArgs) Handles InstallNodeBtn.Click

        If File.Exists(IDpathBox.Text) Then
                If ExIpBox.Text.Length > 10 Then
                    If DashIpBox.Text.Length > 10 Then
                        If WallBox.Text.Length > 10 Then
                            If EmailBox.Text.Length > 5 Then
                                If DataBox.Text.Length > 2 Then
                                    If BanBox.Text.Length > 2 Then
                                        If testSelectedPort("127.0.0.1", PrivateIpBox.Text.Split(":")(1).ToString) Then
                                            MsgBox("server.private-address:" & PrivateIpBox.Text & " Already in use")
                                        Else
                                            If testSelectedPort(DashIpBox.Text.Split(":")(0).ToString, DashIpBox.Text.Split(":")(1).ToString) Then
                                                MsgBox("Node Dashboard:" & DashIpBox.Text & " Already in use")
                                            Else
                                                InstallNode()
                                            End If
                                        End If
                                    Else
                                            MsgBox("Add database path, usualy it is the same as Storage path")
                                    End If

                                Else
                                    MsgBox("Add Storage path")
                                End If
                            Else
                                MsgBox("Add Email")
                            End If
                        Else
                            MsgBox("Add wallet")
                        End If
                    Else
                        MsgBox("Dashboard IP:Port")
                    End If
                Else
                    MsgBox("External Ip:Port")
                End If

            Else
                MsgBox("Indentity not exist")
            End If

    End Sub
    Private Function testSelectedPort(ip As String, port As Integer) As Boolean
        ' Function to open a socket to the specified port to see if it is listening

        ' Connect to socket
        Dim testSocket As New System.Net.Sockets.TcpClient()

        Try
            testSocket.Connect(ip, port)
            ' The socket is accepting connections
            testSocket.Close()
            Return True

        Catch ex As Exception
            ' The port is not accepting connections
            Return False
        End Try

        Return False
    End Function
    Private Sub InstallNode()
        'Try

        Dim nodeNumber As Integer = 0
        Try
            nodeNumber = NodeData.Nodes.Count
        Catch ex As Exception

        End Try
        Directory.CreateDirectory("C:\Program Files\Storj" & nodeNumber & "\Storage Node\")
        Dim objWriter As New System.IO.StreamWriter("C:\Program Files\Storj" & nodeNumber & "\Storage Node\config.yaml")
        Dim config As New NodeConfig
        objWriter.Write(config.GetConfig(ExIpBox.Text, DashIpBox.Text, IDpathBox.Text, StorBox.Text, BanBox.Text, DataBox.Text, PrivateIpBox.Text, EmailBox.Text, WallBox.Text, nodeNumber))
        objWriter.Close()
        DownloadNewFile()
        Dim mainnode As String = My.Application.Info.DirectoryPath & "\storagenode.exe"
        'For Each node In NodeData.Nodes
        '    If node.MainNode And File.Exists(node.Path) Then
        '        mainnode = node.Path.Substring(0, node.Path.Length - 4) & ".exe"
        '        Exit For
        '    End If
        'Next

        My.Computer.FileSystem.CopyFile(mainnode, "C:\Program Files\Storj" & nodeNumber & "\Storage Node\storagenode.exe")
        ServiceInstaller.InstallAndStart("storagenode" & nodeNumber, "storagenode" & nodeNumber, "C:\Program Files\Storj" & nodeNumber & "\Storage Node\storagenode.exe")
        SetImagePath(nodeNumber)

        RunCommandCom("""C:\Program Files\Storj1\Storage Node\storagenode.exe""", "setup   --storage.path " & DataBox.Text & " --identity-dir " & IDpathBox.Text.Replace("identity.cert", "") & " --config-dir " & DataBox.Text & "\tempconf", False)
        Threading.Thread.Sleep(4000)
        Dim sc As ServiceController = New ServiceController("storagenode" & nodeNumber)
        sc.Start()
        SearchService()

        MsgBox("Install Complete")

        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub
    Private Sub DownloadNewFile()
        Dim downloadlinc As String = (Data)("url").ToString
        downloadlinc = downloadlinc.Replace("{os}", "windows")
        downloadlinc = downloadlinc.Replace("{arch}", "amd64")
        Dim saveAs As String = My.Application.Info.DirectoryPath & "\storagenode.exe.zip"
        Dim theResponse As HttpWebResponse
        Dim theRequest As HttpWebRequest
        Try 'Checks if the file exist
            theRequest = WebRequest.Create(downloadlinc) 'fileUrl is your zip url
            theResponse = theRequest.GetResponse
        Catch ex As Exception
            'could not be found on the server (network delay maybe)
            Exit Sub 'Exit sub or function, because if not found can't be downloaded
        End Try
        Dim length As Long = theResponse.ContentLength
        Dim writeStream As New IO.FileStream(saveAs, IO.FileMode.Create)
        Dim nRead As Integer
        Do
            Dim readBytes(4095) As Byte
            Dim bytesread As Integer = theResponse.GetResponseStream.Read(readBytes, 0, 4096)
            nRead += bytesread
            If bytesread = 0 Then Exit Do
            writeStream.Write(readBytes, 0, bytesread)
        Loop
        theResponse.GetResponseStream.Close()
        writeStream.Close()

        'File downloaded 100%
        If File.Exists(saveAs) Then
            extractFile(saveAs, My.Application.Info.DirectoryPath)

        End If
    End Sub
    Private Sub extractFile(zipPath As String, ExtractPath As String)
        ZipFile.ExtractToDirectory(zipPath, ExtractPath)
    End Sub
    Private Sub SetImagePath(nodenumber As String)
        Dim regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\storagenode" & nodenumber, True)
        regKey.SetValue("ImagePath", """C:\Program Files\Storj" & nodenumber & "\Storage Node\storagenode.exe"" run --config-dir ""C:\Program Files\Storj" & nodenumber & "\Storage Node\\""")

    End Sub

    Private Sub TrobleBtn_Click(sender As Object, e As EventArgs) Handles TrobleBtn.Click
        If RowSelected >= 0 Then
            Dim prop As NodeProp = GetNode(RowSelected)
            If prop IsNot Nothing Then
                Troble = New TrobleForm(prop)
                Troble.Show()
            Else
                MsgBox("Node Selecting Error")
            End If
        Else
                MsgBox("Select Node from List")
        End If
    End Sub
    Private Function GetNode(selected As Integer) As NodeProp
        Dim prop As NodeProp
        If NodeData.Nodes IsNot Nothing Then
            For Each node As NodeProp In NodeData.Nodes
                If node.Name = NodeList.Rows(RowSelected).Cells(0).Value Then
                    prop = node
                End If
            Next
        End If
        Return prop
    End Function

    Private Sub ConfEdit_Click(sender As Object, e As EventArgs) Handles ConfEdit.Click
        If RowSelected >= 0 Then
            Dim path As String = NodeList.Rows(RowSelected).Cells(3).Value
            conf = New ConfEditorForm(path.Substring(0, path.Length - 15) & "config.yaml")
            conf.Show()
        End If
    End Sub

    Private Sub Smart_Click(sender As Object, e As EventArgs) Handles Smart.Click
        SmartF = New SmartForm
        SmartF.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If NodeData.Nodes IsNot Nothing Then
            If File.Exists(IDpathBox.Text) Then
                If ExIpBox.Text.Length > 10 Then
                    If DashIpBox.Text.Length > 10 Then
                        If WallBox.Text.Length > 10 Then
                            If EmailBox.Text.Length > 5 Then
                                If DataBox.Text.Length > 2 Then
                                    If BanBox.Text.Length > 2 Then

                                        RunCommandCom("""C:\Program Files\Storj1\Storage Node\storagenode.exe""", "setup   --storage.path " & DataBox.Text & " --identity-dir " & IDpathBox.Text.Replace("identity.cert", "") & " --config-dir " & DataBox.Text & "\tempconf", False)



                                    Else
                                        MsgBox("Add database path, usualy it is the same as Storage path")
                                    End If

                                Else
                                    MsgBox("Add Storage path")
                                End If
                            Else
                                MsgBox("Add Email")
                            End If
                        Else
                            MsgBox("Add wallet")
                        End If
                    Else
                        MsgBox("Dashboard IP:Port")
                    End If
                Else
                    MsgBox("External Ip:Port")
                End If

            Else
                MsgBox("Indentity not exist")
            End If
        Else
            MsgBox("Please install first node by Storj installation file, and add it to dashboard.")
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim services As ServiceController() = ServiceController.GetServices()
        Dim Nodes As NodeStruct = New NodeStruct
        Try
            For Each s As ServiceController In ServiceController.GetServices()
                Dim path = GetImagePath(s.ServiceName)

                If path.Contains("storagenode.exe") And Not path.Contains("storagenode-updater.exe") Then
                    Dim Spath = path.Split(Chr(34))
                    Dim Iden As String = getIdenFromConf(Spath(1))
                    Dim conf As String = Spath(1).Substring(0, Spath(1).Length - 15) & "\"
                    Dim PrivateAdd As String = getPrivateAddFromConf(Spath(1))
                    RunCommandCom("""" & Spath(1) & """", "forget-satellite --force 12rfG3sh9NCWiX3ivPjq2HtdLmbqCrvHVEzJubnzFzosMuawymB 12tRQrMTWUWwzwGh18i7Fqs67kmdhH9t6aToeiwbo5mfS2rUmo --identity-dir """ & Iden & """ --config-dir """ & conf & """", True)

                End If
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try



    End Sub
End Class
