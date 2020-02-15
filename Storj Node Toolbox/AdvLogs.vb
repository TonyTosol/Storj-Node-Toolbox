Imports System.IO
Public Class AdvLogs
    Private Sub AdvLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Structure LogStruct
        Dim time As DateTime
        Dim auditsSuccess As Long
        Dim auditsFailed As Long
        Dim auditsFailedCritical As Long
        Dim dl_success As Long
        Dim dl_failed As Long
        Dim put_success As Long
        Dim put_failed As Long
        Dim put_rejected As Long
        Dim get_repair_success As Long
        Dim get_repair_failed As Long
        Dim put_repair_success As Long
        Dim put_repair_failed As Long
        Dim DiskIOError As Long
        Dim DBmalforemed As Long
        Dim TelemetryError As Long
        Dim DBLocked As Long
        Dim FileNotFound As Long
        Dim delete As Long
    End Structure

    Public Path As String = ""
    Public rowcount As Long = 0
    Private data As New List(Of LogStruct)
    Private startTime As DateTime
    Private presentrow As LogStruct


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
                For Each t As LogStruct In data


                    RG.Rows.Add({t.time, "Audits", t.auditsSuccess, t.auditsFailed, t.auditsFailedCritical, t.auditsSuccess + t.auditsFailed + t.auditsFailedCritical, (t.auditsSuccess / (t.auditsSuccess + t.auditsFailed + t.auditsFailedCritical)) * 100})
                    RG.Rows.Add({t.time, "Egress", t.dl_success, t.dl_failed, "", t.dl_success + t.dl_failed, (t.dl_success / (t.dl_success + t.dl_failed)) * 100})
                    RG.Rows.Add({t.time, "Ingress", t.put_success, t.put_failed, t.put_rejected, t.put_success + t.put_failed + t.put_rejected, t.put_success / (t.put_success + t.put_failed + t.put_rejected) * 100})
                    RG.Rows.Add({t.time, "Repair Egress", t.get_repair_success, t.get_repair_failed, "", t.get_repair_success + t.get_repair_failed, t.get_repair_success / (t.get_repair_success + t.get_repair_failed) * 100})
                    RG.Rows.Add({t.time, "Repair Ingress", t.put_repair_success, t.put_repair_failed, "", t.put_repair_success + t.put_repair_failed, t.put_repair_success / (t.put_repair_success + t.put_repair_failed) * 100})
                    RG.Rows.Add({t.time, "Delete Operation", t.delete, "", "", t.delete})
                    RG.Rows.Add({t.time, "Disk I/O Error", "", "", t.DiskIOError, t.DiskIOError, ""})
                    RG.Rows.Add({t.time, "DB malformed", "", "", t.DBmalforemed, t.DBmalforemed, ""})
                    RG.Rows.Add({t.time, "Telemetry Send Error", "", t.TelemetryError, "", t.TelemetryError, ""})
                    RG.Rows.Add({t.time, "Database Locked", "", t.DBLocked, "", t.DBLocked, ""})
                    RG.Rows.Add({t.time, "File Not Exist", "", "", t.FileNotFound, t.FileNotFound, ""})
                    RG.Rows(RG.Rows.Add("", "Total Operations", t.auditsSuccess + t.dl_success + t.delete + t.get_repair_success + t.put_success, t.auditsFailed + t.auditsFailedCritical + t.dl_failed + t.get_repair_failed + t.put_failed + t.put_rejected + t.put_repair_failed, "", t.auditsFailed + t.auditsFailedCritical + t.auditsSuccess + t.delete + t.dl_failed + t.dl_success + t.get_repair_failed + t.get_repair_success + t.put_failed + t.put_rejected + t.put_repair_failed + t.put_repair_success + t.put_success, (t.auditsSuccess + t.dl_success + t.delete + t.get_repair_success + t.put_success) / (t.auditsFailed + t.auditsFailedCritical + t.auditsSuccess + t.delete + t.dl_failed + t.dl_success + t.get_repair_failed + t.get_repair_success + t.put_failed + t.put_rejected + t.put_repair_failed + t.put_repair_success + t.put_success) * 100)).DefaultCellStyle.BackColor = Color.YellowGreen
                Next
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
                        ProcessRow(line)
                        'Dim a As Threading.Thread = New Threading.Thread(AddressOf ProcessRow)
                        'a.Start(line)
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
            Try


                If startTime < DateTime.Now.AddYears(-2) Then
                    Dim timestring As String = line.Substring(0, 23)
                    Dim temp = Convert.ToDateTime(timestring)
                    temp = temp.AddMinutes(-temp.Minute)
                    temp = temp.AddSeconds(-temp.Second)
                    startTime = temp
                    presentrow = New LogStruct

                    presentrow.time = startTime
                End If
            Catch ex As Exception
                MsgBox("start time err " & ex.Message)
            End Try
            Try
                Dim timestring As String = line.Substring(0, 23)
                Dim logrow As DateTime = Convert.ToDateTime(timestring)
                If logrow > startTime.AddHours(1) Then
                    data.Add(presentrow)
                    logrow = logrow.AddMinutes(-logrow.Minute)
                    logrow = logrow.AddSeconds(-logrow.Second)
                    startTime = logrow
                    presentrow = New LogStruct

                    presentrow.time = startTime
                End If
            Catch ex As Exception
                MsgBox("new time err " & ex.Message)
            End Try
            rowcount = rowcount + 1

            If line.Contains("GET_AUDIT") And line.Contains("downloaded") Then
                presentrow.auditsSuccess += 1
            ElseIf line.Contains("GET_AUDIT") And line.Contains("failed") And line.Contains("open -NotMatch") Then
                presentrow.auditsFailed += 1
            ElseIf line.Contains("GET_AUDIT") And line.Contains("failed") And line.Contains("open") Then
                presentrow.auditsFailedCritical += 1
            ElseIf line.Contains("GET_REPAIR") And line.Contains("downloaded") Then
                presentrow.get_repair_success += 1
            ElseIf line.Contains("GET_REPAIR") And line.Contains("failed") Then
                presentrow.get_repair_failed += 1
            ElseIf line.Contains("PUT_REPAIR") And line.Contains("uploaded") Then
                presentrow.put_repair_success += 1
            ElseIf line.Contains("PUT_REPAIR") And line.Contains("failed") Then
                presentrow.put_repair_failed += 1
            ElseIf line.Contains("GET") And line.Contains("downloaded") Then
                presentrow.dl_success += 1
            ElseIf line.Contains("GET") And line.Contains("failed") Then
                presentrow.dl_failed += 1
            ElseIf line.Contains("PUT") And line.Contains("uploaded") Then
                presentrow.put_success += 1
            ElseIf line.Contains("PUT") And line.Contains("rejected") Then
                presentrow.put_rejected += 1
            ElseIf line.Contains("PUT") And line.Contains("failed") Then
                presentrow.put_failed += 1
            ElseIf line.Contains("disk I/O error") Then
                presentrow.DiskIOError += 1
            ElseIf line.Contains("malformed") Then
                presentrow.DBmalforemed += 1
            ElseIf line.Contains("telemetry failed") Then
                presentrow.TelemetryError += 1
            ElseIf line.Contains("database is locked") Then
                presentrow.DBLocked += 1
            ElseIf line.Contains("file does not exist") Then
                presentrow.FileNotFound += 1
            ElseIf line.Contains("deleted") Then
                presentrow.delete += 1
            End If
        End If
    End Sub


End Class