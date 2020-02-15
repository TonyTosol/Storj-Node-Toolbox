<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AuditForm
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
        Me.NodeView = New System.Windows.Forms.DataGridView()
        Me.Node = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Satellite = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Audits = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalAudits = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Egress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Ingress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RepeirEgress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalBandwidth = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StorageUsed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.NodeView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NodeView
        '
        Me.NodeView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.NodeView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Node, Me.Satellite, Me.Audits, Me.TotalAudits, Me.Egress, Me.Ingress, Me.RepeirEgress, Me.TotalBandwidth, Me.StorageUsed})
        Me.NodeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NodeView.Location = New System.Drawing.Point(0, 0)
        Me.NodeView.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.NodeView.Name = "NodeView"
        Me.NodeView.RowHeadersWidth = 51
        Me.NodeView.RowTemplate.Height = 24
        Me.NodeView.Size = New System.Drawing.Size(1037, 624)
        Me.NodeView.TabIndex = 11
        '
        'Node
        '
        Me.Node.HeaderText = "Node"
        Me.Node.MinimumWidth = 6
        Me.Node.Name = "Node"
        Me.Node.ReadOnly = True
        Me.Node.Width = 125
        '
        'Satellite
        '
        Me.Satellite.HeaderText = "Satellite"
        Me.Satellite.MinimumWidth = 6
        Me.Satellite.Name = "Satellite"
        Me.Satellite.ReadOnly = True
        Me.Satellite.Width = 175
        '
        'Audits
        '
        Me.Audits.HeaderText = "Successful Audits"
        Me.Audits.MinimumWidth = 6
        Me.Audits.Name = "Audits"
        Me.Audits.ReadOnly = True
        Me.Audits.Width = 125
        '
        'TotalAudits
        '
        Me.TotalAudits.HeaderText = "Total Audits"
        Me.TotalAudits.MinimumWidth = 6
        Me.TotalAudits.Name = "TotalAudits"
        Me.TotalAudits.ReadOnly = True
        Me.TotalAudits.Width = 125
        '
        'Egress
        '
        Me.Egress.HeaderText = "Egress"
        Me.Egress.MinimumWidth = 6
        Me.Egress.Name = "Egress"
        Me.Egress.ReadOnly = True
        Me.Egress.Width = 125
        '
        'Ingress
        '
        Me.Ingress.HeaderText = "Ingress"
        Me.Ingress.MinimumWidth = 6
        Me.Ingress.Name = "Ingress"
        Me.Ingress.ReadOnly = True
        Me.Ingress.Width = 125
        '
        'RepeirEgress
        '
        Me.RepeirEgress.HeaderText = "Repair Egress"
        Me.RepeirEgress.MinimumWidth = 6
        Me.RepeirEgress.Name = "RepeirEgress"
        Me.RepeirEgress.ReadOnly = True
        Me.RepeirEgress.Width = 125
        '
        'TotalBandwidth
        '
        Me.TotalBandwidth.HeaderText = "Total Bandwidth"
        Me.TotalBandwidth.MinimumWidth = 6
        Me.TotalBandwidth.Name = "TotalBandwidth"
        Me.TotalBandwidth.ReadOnly = True
        Me.TotalBandwidth.Width = 125
        '
        'StorageUsed
        '
        Me.StorageUsed.HeaderText = "TB*Month"
        Me.StorageUsed.MinimumWidth = 6
        Me.StorageUsed.Name = "StorageUsed"
        Me.StorageUsed.ReadOnly = True
        Me.StorageUsed.Width = 125
        '
        'AuditForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1037, 624)
        Me.Controls.Add(Me.NodeView)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "AuditForm"
        Me.Text = "AuditForm"
        CType(Me.NodeView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents NodeView As DataGridView
    Friend WithEvents Node As DataGridViewTextBoxColumn
    Friend WithEvents Satellite As DataGridViewTextBoxColumn
    Friend WithEvents Audits As DataGridViewTextBoxColumn
    Friend WithEvents TotalAudits As DataGridViewTextBoxColumn
    Friend WithEvents Egress As DataGridViewTextBoxColumn
    Friend WithEvents Ingress As DataGridViewTextBoxColumn
    Friend WithEvents RepeirEgress As DataGridViewTextBoxColumn
    Friend WithEvents TotalBandwidth As DataGridViewTextBoxColumn
    Friend WithEvents StorageUsed As DataGridViewTextBoxColumn
End Class
