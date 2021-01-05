<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SmartForm
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.GetSMARTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.LoadingLbl = New System.Windows.Forms.ToolStripStatusLabel()
        Me.HddView = New System.Windows.Forms.DataGridView()
        Me.Drive = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ParamName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BeAwereThatYouMakeChangesUnderYourOwnRiskBeVeryCarlefullToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.HddView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetSMARTToolStripMenuItem, Me.BeAwereThatYouMakeChangesUnderYourOwnRiskBeVeryCarlefullToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(800, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'GetSMARTToolStripMenuItem
        '
        Me.GetSMARTToolStripMenuItem.Name = "GetSMARTToolStripMenuItem"
        Me.GetSMARTToolStripMenuItem.Size = New System.Drawing.Size(93, 20)
        Me.GetSMARTToolStripMenuItem.Text = "Get S.M.A.R.T."
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadingLbl})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 530)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(800, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'LoadingLbl
        '
        Me.LoadingLbl.Name = "LoadingLbl"
        Me.LoadingLbl.Size = New System.Drawing.Size(0, 17)
        '
        'HddView
        '
        Me.HddView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.HddView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Drive, Me.ParamName, Me.PValue})
        Me.HddView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HddView.Location = New System.Drawing.Point(0, 24)
        Me.HddView.Name = "HddView"
        Me.HddView.Size = New System.Drawing.Size(800, 506)
        Me.HddView.TabIndex = 2
        '
        'Drive
        '
        Me.Drive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.Drive.HeaderText = "Drive Letter"
        Me.Drive.Name = "Drive"
        Me.Drive.Width = 80
        '
        'ParamName
        '
        Me.ParamName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.ParamName.HeaderText = "Parameter Name"
        Me.ParamName.Name = "ParamName"
        Me.ParamName.Width = 102
        '
        'PValue
        '
        Me.PValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.PValue.HeaderText = "Value"
        Me.PValue.Name = "PValue"
        Me.PValue.Width = 59
        '
        'BeAwereThatYouMakeChangesUnderYourOwnRiskBeVeryCarlefullToolStripMenuItem
        '
        Me.BeAwereThatYouMakeChangesUnderYourOwnRiskBeVeryCarlefullToolStripMenuItem.Name = "BeAwereThatYouMakeChangesUnderYourOwnRiskBeVeryCarlefullToolStripMenuItem"
        Me.BeAwereThatYouMakeChangesUnderYourOwnRiskBeVeryCarlefullToolStripMenuItem.Size = New System.Drawing.Size(386, 20)
        Me.BeAwereThatYouMakeChangesUnderYourOwnRiskBeVeryCarlefullToolStripMenuItem.Text = "Be aware that you make changes under your own risk, be very careful."
        '
        'SmartForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 552)
        Me.Controls.Add(Me.HddView)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "SmartForm"
        Me.Text = "SmartForm"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.HddView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents HddView As DataGridView
    Friend WithEvents Drive As DataGridViewTextBoxColumn
    Friend WithEvents ParamName As DataGridViewTextBoxColumn
    Friend WithEvents PValue As DataGridViewTextBoxColumn
    Friend WithEvents GetSMARTToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoadingLbl As ToolStripStatusLabel
    Friend WithEvents BeAwereThatYouMakeChangesUnderYourOwnRiskBeVeryCarlefullToolStripMenuItem As ToolStripMenuItem
End Class
