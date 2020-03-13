Imports System.IO
Imports System.Net
Imports System.ServiceProcess
Imports System.Text.RegularExpressions

Public Class TrobleForm
    Private Node As NodeProp
    Private running As Boolean = False
    Private Delegate Sub Addrowtext(text As String, colortext As Color)
    Public Sub New(nodelink As NodeProp)

        ' This call is required by the designer.
        InitializeComponent()
        Node = nodelink
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub TrobleForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NameLink.Text = Node.Name
        ListView1.View = View.Details
        ListView2.View = View.Details
        Dim a As Threading.Thread = New Threading.Thread(AddressOf Timedcheck)
        a.IsBackground = True
        a.Start()
        'Dim LogsMon As Threading.Thread = New Threading.Thread(AddressOf ReadLogs)
        'LogsMon.IsBackground = True
        'LogsMon.Start(Node.Path)
    End Sub

    Private Sub NameLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles NameLink.LinkClicked
        Process.Start("Http://" & Node.IP & ":" & Node.Port)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If running Then
            Button1.Text = "Start"
            running = False
        Else
            Button1.Text = "Stop"
            running = True

            running = True
            AddRow1("Trobleshooting started")
            Dim b As Threading.Thread = New Threading.Thread(AddressOf startAnalyzing)
            b.IsBackground = True
            b.Start()
        End If
    End Sub

    Private Sub startAnalyzing()

        If ConfInspect() Then Exit Sub
        StopTroble()
    End Sub
    Private Sub StopTroble()
        If Me.InvokeRequired Then
            Me.Invoke(New MethodInvoker(AddressOf StopTroble))
        Else
            running = False
            Button1.Text = "Start"
            AddRow1("Trobleshooting Complete", "Window")
        End If
    End Sub
    Private Function ConfInspect() As Boolean
        Try
            AddRow1("Inspecting Configuration")
            Getconfig(Node.Path)
        Catch ex As Exception
            MsgBox("Something went wrong with trobleshooting, cant read config" & ex.Message)
        End Try
        AddRow1("Got Configuration")
            AddRow1("Matching IP")
        Try
            Dim domainip As String = Helper.GetIPFromDomain(Address.Split(":").First)
            Dim externalip As String = Helper.GetExternalIp()
            If String.Compare(domainip, externalip) = 0 Then
                AddRow1("Your External IP match your Configuration in contact.external-address:" & Address.Split(":").First & "=" & domainip & "  External IP=" & externalip, "YellowGreen")
            Else
                AddRow1("Your External IP mismatch your Configuration in contact.external-address:" & Address.Split(":").First & "=" & domainip & "  External IP=" & externalip, "Red")
                AddRow1("Please make your configuration ip same as your external IP, If you use service like NOIP or similar, install sofware that will renew your IP to this service.", "Red")

                Return True
            End If
        Catch ex As Exception
            AddRow1("Something went wrong with trobleshooting, cant get ip for maching " & ex.Message, "Red")
        End Try
        Try
            If Helper.CheckWinFirewallOpened(serverAdess.TrimStart(":")) Then
                AddRow1("Your firewall is Enabled and No Exeption added to Used port" & serverAdess, "Red")
                StopTroble()

                Return True
            Else
                AddRow1("Firewall is OFF or Port Exeption aded Port" & serverAdess, "YellowGreen")
            End If
        Catch ex As Exception
            AddRow1("Something went wrong with trobleshooting, error for firewall exeptions " & ex.Message, "Red")
        End Try
        Try
            If Helper.GetFileCountMatch(Indentity.Substring(0, Indentity.Length - 13)) Then
                AddRow1("Indentity is not signd, please sign indentity and restart Node", "Red")
                StopTroble()

                Return True
            Else
                AddRow1("Indentity has all 6 files, looks like signed", "YellowGreen")
            End If
        Catch ex As Exception
            AddRow1("Something went wrong with trobleshooting,error maching indentity " & ex.Message, "Red")
        End Try
        Return False
    End Function
    Private Address As String
    Private Indentity As String = ""
    Private serverAdess As String = ""
    Private serverPrivate As String = ""
    Private Sub Getconfig(path As String)

        Dim conf As String = path.Substring(0, path.Length - 15) & "config.yaml"
        Dim len As Long = 0

        Dim rowlen As Integer = 0

        Using fs As FileStream = New FileStream(conf, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)


            Dim reader As New StreamReader(fs, True) ''System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine
                If line.Contains("contact.external-address:") Then
                    Dim addr As String = line.Split(" ").Last
                    If Address = Nothing Then Address = line.Split(" ").Last
                    If Address.Contains("""") Then Address = Nothing
                    AddRow1("contact.external-address: " & Address)
                ElseIf line.Contains("identity.cert-path:") Then
                    Indentity = line.Split(" ").Last
                ElseIf line.Contains("server.address:") Then
                    serverAdess = line.Split(" ").Last
                ElseIf line.Contains("server.private-address:") Then
                    serverPrivate = line.Split(" ").Last
                ElseIf line.Contains("kademlia.external-address:") Then
                    If Address = Nothing Then Address = line.Split(" ").Last
                    AddRow1("kademlia.external-address: " & Address)
                End If
            End While
            reader.Close()
            fs.Close()
        End Using


    End Sub

    Private Sub Timedcheck()
        Threading.Thread.Sleep(2000)
        Try

            GetServiceStatus()
        Catch ex As Exception

        End Try

        Dim a As Threading.Thread = New Threading.Thread(AddressOf Timedcheck)
            a.IsBackground = True
            a.Start()

    End Sub
    Private Sub GetServiceStatus()
        If Me.InvokeRequired Then
            Me.Invoke(New MethodInvoker(AddressOf GetServiceStatus))
        Else
            If Helper.IsServiceRuning(Node.ServiceName) Then
                StatusLbl.Text = "Running"
                StatusLbl.BackColor = Color.Green
            Else
                StatusLbl.Text = "Stoped"
                StatusLbl.BackColor = Color.Red
            End If
        End If
    End Sub

    Private Sub AddrowList1(row As String, colortext As Color)
        If Me.InvokeRequired Then
            Me.Invoke(New Addrowtext(AddressOf AddrowList1), New Object() {row, colortext})
        Else
            If ListView1.Items.Count > 100 Then
                ListView1.Items.RemoveAt(0)
            End If
            ListView1.Items.Add(row).BackColor = colortext
            ListView1.Items(ListView1.Items.Count - 1).Selected = True
            ListView1.Items(ListView1.Items.Count - 1).EnsureVisible()
        End If
    End Sub
    Private Sub AddRow1(data As String, Optional colortext As String = "Window")


        Dim a As Threading.Thread = New Threading.Thread(Sub() AddrowList1(data, Color.FromName(colortext)))
        a.IsBackground = True
        a.Start()
    End Sub
    Private Sub AddRow2(data As String, Optional colortext As String = "Window")

        Dim a As Threading.Thread = New Threading.Thread(Sub() AddrowList2(data, Color.FromName(colortext)))
        a.IsBackground = True
        a.Start()
    End Sub
    Private Sub AddrowList2(row As String, colortext As Color)
        If Me.InvokeRequired Then
            Me.Invoke(New Addrowtext(AddressOf AddrowList2), New Object() {row, colortext})
        Else
            If ListView2.Items.Count > 300 Then
                ListView2.Items.RemoveAt(0)
            End If
            ListView2.Items.Add(row).BackColor = colortext
            ListView1.Items(ListView1.Items.Count - 1).Selected = True
            ListView1.Items(ListView1.Items.Count - 1).EnsureVisible()
        End If
    End Sub

    Private Sub ArhiveLogs()

        AddRow1("Archiving old logs")
        AddRow1("Stoping Node")
        Dim sc As ServiceController = New ServiceController(Node.ServiceName)
        sc.Stop()
        Threading.Thread.Sleep(5000)
        AddRow1("Rename old log to " & "storagenode" & DateTime.Now.Day & "-" & DateTime.Now.Month & "-" & DateTime.Now.Year & ".log")
        My.Computer.FileSystem.RenameFile(Node.Path, "storagenode" & DateTime.Now.Day & "-" & DateTime.Now.Month & "-" & DateTime.Now.Year & DateTime.Now.Millisecond & ".log")
        AddRow1("Starting Node")
        sc.Start()
        AddRow1("Archiving old logs complete ")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AddRow1("Restarting Node")
        Dim sc As ServiceController = New ServiceController(Node.ServiceName)
        sc.Stop()
        Threading.Thread.Sleep(5000)
        sc.Start()
    End Sub
    Private Sub ReadLogs(filename As String)
        ArhiveLogs()
        Threading.Thread.Sleep(2000)

        Dim file As String = filename
        If file IsNot Nothing Then


            ''IO.File.Copy(file, "file.txt", True)
            Dim len As Long = 0

            Dim rowlen As Integer = 0

            Using fs As FileStream = New FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

                AddRow1("Started logs monitoring")
                Dim reader As New StreamReader(fs, True) ''System.Text.Encoding.UTF8)
                While True
                    While Not reader.EndOfStream
                        Dim line As String = reader.ReadLine
                        If line IsNot Nothing Then
                            AddRow2(line)
                        End If
                    End While
                    Threading.Thread.Sleep(500)
                End While
                reader.Close()
                fs.Close()
            End Using
            AddRow1("Stoped logs monitoring")

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Helper.FlushMyCache()
        AddRow1("Flushing DNS Cache, After need to make windows restart")
    End Sub
End Class