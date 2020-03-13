<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogAnalize
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.RG = New System.Windows.Forms.DataGridView()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LogRowLbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.OName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Succsesfull = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Faild = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Canceled = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FaildCritical = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Total = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.KPD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.RG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RG
        '
        Me.RG.AllowUserToAddRows = False
        Me.RG.AllowUserToDeleteRows = False
        Me.RG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.RG.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.OName, Me.Succsesfull, Me.Faild, Me.Canceled, Me.FaildCritical, Me.Total, Me.KPD})
        Me.RG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RG.Location = New System.Drawing.Point(0, 0)
        Me.RG.Margin = New System.Windows.Forms.Padding(2)
        Me.RG.Name = "RG"
        Me.RG.ReadOnly = True
        Me.RG.RowHeadersWidth = 51
        Me.RG.RowTemplate.Height = 24
        Me.RG.Size = New System.Drawing.Size(1028, 614)
        Me.RG.TabIndex = 4
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.LogRowLbl})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 592)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 10, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(1028, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(109, 17)
        Me.ToolStripStatusLabel1.Text = "Log Rows Analyzed"
        '
        'LogRowLbl
        '
        Me.LogRowLbl.Name = "LogRowLbl"
        Me.LogRowLbl.Size = New System.Drawing.Size(0, 17)
        '
        'OName
        '
        Me.OName.HeaderText = "OpName"
        Me.OName.MinimumWidth = 6
        Me.OName.Name = "OName"
        Me.OName.ReadOnly = True
        Me.OName.Width = 125
        '
        'Succsesfull
        '
        Me.Succsesfull.HeaderText = "Successful"
        Me.Succsesfull.MinimumWidth = 6
        Me.Succsesfull.Name = "Succsesfull"
        Me.Succsesfull.ReadOnly = True
        Me.Succsesfull.Width = 125
        '
        'Faild
        '
        Me.Faild.HeaderText = "Failed"
        Me.Faild.MinimumWidth = 6
        Me.Faild.Name = "Faild"
        Me.Faild.ReadOnly = True
        Me.Faild.Width = 75
        '
        'Canceled
        '
        Me.Canceled.HeaderText = "Canceled"
        Me.Canceled.Name = "Canceled"
        Me.Canceled.ReadOnly = True
        '
        'FaildCritical
        '
        Me.FaildCritical.HeaderText = "Failed Critical"
        Me.FaildCritical.MinimumWidth = 6
        Me.FaildCritical.Name = "FaildCritical"
        Me.FaildCritical.ReadOnly = True
        Me.FaildCritical.Width = 125
        '
        'Total
        '
        Me.Total.HeaderText = "Total"
        Me.Total.MinimumWidth = 6
        Me.Total.Name = "Total"
        Me.Total.ReadOnly = True
        Me.Total.Width = 75
        '
        'KPD
        '
        Me.KPD.HeaderText = "KPD"
        Me.KPD.MinimumWidth = 6
        Me.KPD.Name = "KPD"
        Me.KPD.ReadOnly = True
        Me.KPD.Width = 50
        '
        'LogAnalize
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 614)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.RG)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "LogAnalize"
        Me.Text = "LogAnalize"
        CType(Me.RG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RG As DataGridView
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents LogRowLbl As ToolStripStatusLabel
    Friend WithEvents OName As DataGridViewTextBoxColumn
    Friend WithEvents Succsesfull As DataGridViewTextBoxColumn
    Friend WithEvents Faild As DataGridViewTextBoxColumn
    Friend WithEvents Canceled As DataGridViewTextBoxColumn
    Friend WithEvents FaildCritical As DataGridViewTextBoxColumn
    Friend WithEvents Total As DataGridViewTextBoxColumn
    Friend WithEvents KPD As DataGridViewTextBoxColumn
End Class
