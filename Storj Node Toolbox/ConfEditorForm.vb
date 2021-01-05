Imports System.IO
Public Class ConfEditorForm
    Private path As String = ""
    Private conf As List(Of String)
    Public Sub New(ConfigPath As String)

        ' This call is required by the designer.
        InitializeComponent()
        path = ConfigPath
        ' Add any initialization after the InitializeComponent() call.
        conf = New List(Of String)
    End Sub
    Private Sub ConfEditorForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists(path) Then
            Dim objReader As New System.IO.StreamReader(path)

            Do While objReader.Peek() <> -1

                conf.Add(objReader.ReadLine())

            Loop
            For Each line As String In conf
                ConfGrid.Rows.Add(line)
            Next
            For Each row As DataGridViewRow In ConfGrid.Rows
                If row.Cells.Count > 0 Then
                    If row.Cells(0).Value IsNot Nothing Then
                        If row.Cells(0).Value.ToString.Length > 0 Then
                            If row.Cells(0).Value.ToString.First = "#" Then
                                row.DefaultCellStyle.ForeColor = Color.Gray

                            Else
                                row.DefaultCellStyle.ForeColor = Color.Black

                            End If
                        End If
                    End If
                End If
            Next
        Else
            MsgBox("Path error, cant find config on path: " & path)
        End If
    End Sub

    Private Sub cellCahnged(sender As Object, e As DataGridViewCellEventArgs) Handles ConfGrid.CellValueChanged
        If ConfGrid.Rows(e.RowIndex).Cells(0).Value.ToString.First = "#" Then
            ConfGrid.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Color.Gray

        Else
            ConfGrid.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Color.Black

        End If


    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Status1.Text = "Saving data"
        Me.Enabled = False

        Try


            Dim objWriter As New System.IO.StreamWriter(path, False)

            For Each row As DataGridViewRow In ConfGrid.Rows
                If row.Cells.Count > 0 Then
                    If row.Cells(0).Value IsNot Nothing Then
                        objWriter.WriteLine(row.Cells(0).Value.ToString)
                    End If
                End If
            Next
            objWriter.Flush()
            objWriter.Close()

            objWriter.Dispose()
            Threading.Thread.Sleep(7000)
            MsgBox("Configuration Saved")
        Catch ex As Exception
            MsgBox("There is error during file save " & ex.Message)
        End Try
        Status1.Text = "Data Saved"
        Me.Enabled = True
    End Sub

End Class