Imports System.Management
Imports Simplified.IO

Public Class SmartForm
    Private drives As DriveCollection

    Private Sub GetSMARTToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetSMARTToolStripMenuItem.Click
        LoadingLbl.Text = "Loading data"
        Me.Enabled = False
        drives = Smart.GetDrives
        For Each dr As Drive In drives
            Dim Letters As String = ""
            For Each driveleter As String In dr.DriveLetters
                Letters = Letters & driveleter & " "
            Next

            HddView.Rows.Add({Letters, "Drive Model", dr.Model})
            For Each attr In dr.SmartAttributes
                If attr.HasData Then HddView.Rows.Add({"", attr.Name, attr.Data.ToString})
            Next
        Next
        For Each row As DataGridViewRow In HddView.Rows
            If String.Compare(row.Cells(1).Value, "Reallocated sector count") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Yellow
            End If
            If String.Compare(row.Cells(1).Value, "Reported Uncorrectable Errors") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Yellow
            End If
            If String.Compare(row.Cells(1).Value, "Current Pending Sector Count") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Red
            End If
            If String.Compare(row.Cells(1).Value, "Uncorrectable Sector Count") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Red
            End If
            If String.Compare(row.Cells(1).Value, "End-to-End error") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Red
            End If
            If String.Compare(row.Cells(1).Value, "Reallocation Event Count") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Red
            End If
            If String.Compare(row.Cells(1).Value, "Drive Model") = 0 Then
                row.DefaultCellStyle.BackColor = Color.AliceBlue
            End If
            If String.Compare(row.Cells(1).Value, "Offline scan uncorrectable count") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Red
            End If
            If String.Compare(row.Cells(1).Value, "Current pending sector count") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Red
            End If
            If String.Compare(row.Cells(1).Value, "Temperature") = 0 Then
                If (row.Cells(2).Value > 45) And (row.Cells(2).Value < 500) Then row.DefaultCellStyle.BackColor = Color.Yellow
            End If
            If String.Compare(row.Cells(1).Value, "Temperature") = 0 Then
                If (row.Cells(2).Value > 50) And (row.Cells(2).Value < 500) Then row.DefaultCellStyle.BackColor = Color.Red
            End If
            If String.Compare(row.Cells(1).Value, "Raw read error rate") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Yellow
            End If
            If String.Compare(row.Cells(1).Value, "Hardware ECC recovered") = 0 Then
                If row.Cells(2).Value > 0 Then row.DefaultCellStyle.BackColor = Color.Yellow
            End If
            If String.Compare(row.Cells(1).Value, "Airflow Temperature") = 0 Then
                If row.Cells(2).Value > 45 Then row.DefaultCellStyle.BackColor = Color.Yellow
            End If
            If String.Compare(row.Cells(1).Value, "Airflow Temperature") = 0 Then
                If row.Cells(2).Value > 50 Then row.DefaultCellStyle.BackColor = Color.Red
            End If
        Next
        LoadingLbl.Text = "Loading End"
        Me.Enabled = True
    End Sub




End Class
