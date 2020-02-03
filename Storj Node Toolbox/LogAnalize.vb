Imports System.IO
Public Class LogAnalize
    Private Sub LogAnalize_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Path As String = ""
    Public rowcount As Long = 0
    Dim auditsSuccess As Long = 0
    Dim auditsFailed As Long = 0
    Dim auditsFailedCritical As Long = 0

    Dim dl_success As Long = 0
    Dim dl_failed As Long = 0

    Dim put_success As Long = 0
    Dim put_failed As Long = 0
    Dim put_rejected As Long = 0

    Dim get_repair_success As Long = 0
    Dim get_repair_failed As Long = 0

    Dim put_repair_success As Long = 0
    Dim put_repair_failed As Long = 0


    Dim DiskIOError As Long = 0
    Dim DBmalforemed As Long = 0
    Dim TelemetryError As Long = 0
    Dim DBLocked As Long = 0
    Dim FileNotFound As Long = 0

    Public Reading = True
    Public ReadEnd As Boolean = False

    Public SleepTime As Integer = 0

    Public Sub StartRead(path As String)
        rowcount = 0

        Dim a As Threading.Thread = New Threading.Thread(AddressOf ReadLogs)
        a.IsBackground = True
        a.Start(path)
        Dim b As Threading.Thread = New Threading.Thread(AddressOf UpdateRowcount)
        b.IsBackground = True
        b.Start()
    End Sub
    Private Sub UpdateRowcount()
        Threading.Thread.Sleep(1000)
        UpdateLastSended()

        If ReadEnd = False Then

            Dim b As Threading.Thread = New Threading.Thread(AddressOf UpdateRowcount)
            b.IsBackground = True
            b.Start()
        End If
    End Sub
    Private Sub UpdateLastSended()
        If Me.InvokeRequired Then
            Me.Invoke(New MethodInvoker(AddressOf UpdateLastSended))
        Else

            LogRowLbl.Text = rowcount
        End If

    End Sub

    Private Sub PrintResults()
        Try


            If Me.InvokeRequired Then
                Me.Invoke(New MethodInvoker(AddressOf PrintResults))
            Else
                RG.Rows.Add({"Audits", auditsSuccess, auditsFailed, auditsFailedCritical, auditsSuccess + auditsFailed + auditsFailedCritical, (auditsSuccess / (auditsSuccess + auditsFailed + auditsFailedCritical)) * 100})
                RG.Rows.Add({"Egress", dl_success, dl_failed, "", dl_success + dl_failed, (dl_success / (dl_success + dl_failed)) * 100})
                RG.Rows.Add({"Ingress", put_success, put_failed, put_rejected, put_success + put_failed + put_rejected, put_success / (put_success + put_failed + put_rejected) * 100})
                RG.Rows.Add({"Repair Egress", get_repair_success, get_repair_failed, "", get_repair_success + get_repair_failed, get_repair_success / (get_repair_success + get_repair_failed) * 100})
                RG.Rows.Add({"Repair Ingress", put_repair_success, put_repair_failed, "", put_repair_success + put_repair_failed, put_repair_success / (put_repair_success + put_repair_failed) * 100})
                RG.Rows.Add({"Disk I/O Error", "", "", DiskIOError, DiskIOError, ""})
                RG.Rows.Add({"DB malformed", "", "", DBmalforemed, DBmalforemed, ""})
                RG.Rows.Add({"Telemetry Send Error", "", TelemetryError, "", TelemetryError, ""})
                RG.Rows.Add({"Database Locked", "", DBLocked, "", DBLocked, ""})
                RG.Rows.Add({"File Not Exist", "", "", FileNotFound, FileNotFound, ""})
                Me.Show()
            End If
        Catch ex As Exception
            MsgBox("Error during rending")
        End Try
    End Sub
    Private Sub ReadLogs(filename As String)
        Dim file As String = filename
        If file IsNot Nothing Then

            Try
                ''IO.File.Copy(file, "file.txt", True)
                Dim len As Long = 0

                Dim rowlen As Integer = 0

                Using fs As FileStream = New FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)


                    Dim reader As New StreamReader(fs, True) ''System.Text.Encoding.UTF8)
                    While Not reader.EndOfStream
                        Dim line As String = reader.ReadLine
                        Dim a As Threading.Thread = New Threading.Thread(AddressOf ProcessRow)
                        a.Start(line)
                        Threading.Thread.Sleep(SleepTime)

                    End While
                    reader.Close()
                    fs.Close()
                End Using

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try


        End If
        ReadEnd = True
        PrintResults()
    End Sub
    Private Sub ProcessRow(line As String)
        If line IsNot Nothing Then
            rowcount = rowcount + 1
            If line.Contains("GET_AUDIT") And line.Contains("downloaded") Then
                auditsSuccess += 1
            ElseIf line.Contains("GET_AUDIT") And line.Contains("failed") And line.Contains("open -NotMatch") Then
                auditsFailed += 1
            ElseIf line.Contains("GET_AUDIT") And line.Contains("failed") And line.Contains("open") Then
                auditsFailedCritical += 1
            ElseIf line.Contains("GET_REPAIR") And line.Contains("downloaded") Then
                get_repair_success += 1
            ElseIf line.Contains("GET_REPAIR") And line.Contains("failed") Then
                get_repair_failed += 1
            ElseIf line.Contains("PUT_REPAIR") And line.Contains("uploaded") Then
                put_repair_success += 1
            ElseIf line.Contains("PUT_REPAIR") And line.Contains("failed") Then
                put_repair_failed += 1
            ElseIf line.Contains("GET") And line.Contains("downloaded") Then
                dl_success += 1
            ElseIf line.Contains("GET") And line.Contains("failed") Then
                dl_failed += 1
            ElseIf line.Contains("PUT") And line.Contains("uploaded") Then
                put_success += 1
            ElseIf line.Contains("PUT") And line.Contains("rejected") Then
                put_rejected += 1
            ElseIf line.Contains("PUT") And line.Contains("failed") Then
                put_failed += 1
            ElseIf line.Contains("disk I/O error") Then
                DiskIOError += 1
            ElseIf line.Contains("malformed") Then
                DBmalforemed += 1
            ElseIf line.Contains("telemetry failed") Then
                TelemetryError += 1
            ElseIf line.Contains("database is locked") Then
                DBLocked += 1
            ElseIf line.Contains("file does not exist") Then
                FileNotFound += 1
            End If
        End If
    End Sub


End Class