<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LogPathBox = New System.Windows.Forms.TextBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SaveUserID = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.UserIDBox = New System.Windows.Forms.TextBox()
        Me.NodeName = New System.Windows.Forms.TextBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.AddNodeBtn = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.IPBox = New System.Windows.Forms.TextBox()
        Me.PortBox = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.SleepTimeBox = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ServiceText = New System.Windows.Forms.TextBox()
        Me.MainNodeCheck = New System.Windows.Forms.CheckBox()
        Me.NodeList = New System.Windows.Forms.DataGridView()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.NodeNameG = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NodeIP = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NodePort = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NodePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ServiceName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MainNodeCol = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NodeStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StartNodeBtn = New System.Windows.Forms.Button()
        Me.StopNodeBtn = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ArhiveBtn = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.NodeList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(252, 126)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(91, 25)
        Me.Button3.TabIndex = 39
        Me.Button3.Text = "LogPath"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 107)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 17)
        Me.Label5.TabIndex = 38
        Me.Label5.Text = "LogPath"
        '
        'LogPathBox
        '
        Me.LogPathBox.Location = New System.Drawing.Point(6, 127)
        Me.LogPathBox.Name = "LogPathBox"
        Me.LogPathBox.Size = New System.Drawing.Size(225, 22)
        Me.LogPathBox.TabIndex = 37
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(22, 95)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(226, 21)
        Me.CheckBox2.TabIndex = 36
        Me.CheckBox2.Text = "Log Monitoring(for this pc only)"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(154, 17)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "Last Sended: Unknown"
        '
        'SaveUserID
        '
        Me.SaveUserID.Location = New System.Drawing.Point(176, 145)
        Me.SaveUserID.Name = "SaveUserID"
        Me.SaveUserID.Size = New System.Drawing.Size(75, 23)
        Me.SaveUserID.TabIndex = 34
        Me.SaveUserID.Text = "Save"
        Me.SaveUserID.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(22, 124)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 17)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "Discord User ID"
        '
        'UserIDBox
        '
        Me.UserIDBox.Location = New System.Drawing.Point(22, 145)
        Me.UserIDBox.Name = "UserIDBox"
        Me.UserIDBox.Size = New System.Drawing.Size(148, 22)
        Me.UserIDBox.TabIndex = 32
        '
        'NodeName
        '
        Me.NodeName.Location = New System.Drawing.Point(122, 21)
        Me.NodeName.Name = "NodeName"
        Me.NodeName.Size = New System.Drawing.Size(109, 22)
        Me.NodeName.TabIndex = 31
        Me.NodeName.Text = "Node 1"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(22, 57)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(202, 21)
        Me.CheckBox1.TabIndex = 30
        Me.CheckBox1.Text = "Perform Discord Monitoring"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(349, 21)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(91, 23)
        Me.Button4.TabIndex = 29
        Me.Button4.Text = "Delete Node"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'AddNodeBtn
        '
        Me.AddNodeBtn.Location = New System.Drawing.Point(252, 21)
        Me.AddNodeBtn.Name = "AddNodeBtn"
        Me.AddNodeBtn.Size = New System.Drawing.Size(91, 23)
        Me.AddNodeBtn.TabIndex = 28
        Me.AddNodeBtn.Text = "Add Node"
        Me.AddNodeBtn.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 17)
        Me.Label2.TabIndex = 25
        Me.Label2.Text = "Node Port"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 439)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 17)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Node IP"
        '
        'IPBox
        '
        Me.IPBox.Location = New System.Drawing.Point(6, 21)
        Me.IPBox.Name = "IPBox"
        Me.IPBox.Size = New System.Drawing.Size(110, 22)
        Me.IPBox.TabIndex = 23
        Me.IPBox.Text = "192.168.88.240"
        '
        'PortBox
        '
        Me.PortBox.Location = New System.Drawing.Point(6, 76)
        Me.PortBox.Name = "PortBox"
        Me.PortBox.Size = New System.Drawing.Size(133, 22)
        Me.PortBox.TabIndex = 22
        Me.PortBox.Text = "14002"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(22, 74)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(107, 27)
        Me.Button1.TabIndex = 21
        Me.Button1.Text = "Get Audits"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.UserIDBox)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.CheckBox2)
        Me.GroupBox1.Controls.Add(Me.SaveUserID)
        Me.GroupBox1.Location = New System.Drawing.Point(1084, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(275, 190)
        Me.GroupBox1.TabIndex = 40
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Monitoring"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ArhiveBtn)
        Me.GroupBox2.Controls.Add(Me.StopNodeBtn)
        Me.GroupBox2.Controls.Add(Me.StartNodeBtn)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.SleepTimeBox)
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Location = New System.Drawing.Point(1084, 208)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(275, 245)
        Me.GroupBox2.TabIndex = 41
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Tools"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(145, 18)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(130, 17)
        Me.Label7.TabIndex = 47
        Me.Label7.Text = "Sleep Time 0-10ms"
        '
        'SleepTimeBox
        '
        Me.SleepTimeBox.Location = New System.Drawing.Point(148, 37)
        Me.SleepTimeBox.Name = "SleepTimeBox"
        Me.SleepTimeBox.Size = New System.Drawing.Size(100, 22)
        Me.SleepTimeBox.TabIndex = 25
        Me.SleepTimeBox.Text = "0"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(22, 35)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(107, 27)
        Me.Button2.TabIndex = 22
        Me.Button2.Text = "Analize Logs"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 160)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(182, 17)
        Me.Label6.TabIndex = 43
        Me.Label6.Text = "Storagenode Service Name"
        '
        'ServiceText
        '
        Me.ServiceText.Location = New System.Drawing.Point(6, 180)
        Me.ServiceText.Name = "ServiceText"
        Me.ServiceText.Size = New System.Drawing.Size(225, 22)
        Me.ServiceText.TabIndex = 42
        Me.ServiceText.Text = "storagenode"
        '
        'MainNodeCheck
        '
        Me.MainNodeCheck.AutoSize = True
        Me.MainNodeCheck.Location = New System.Drawing.Point(252, 180)
        Me.MainNodeCheck.Name = "MainNodeCheck"
        Me.MainNodeCheck.Size = New System.Drawing.Size(98, 21)
        Me.MainNodeCheck.TabIndex = 44
        Me.MainNodeCheck.Text = "Main Node"
        Me.MainNodeCheck.UseVisualStyleBackColor = True
        '
        'NodeList
        '
        Me.NodeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.NodeList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NodeNameG, Me.NodeIP, Me.NodePort, Me.NodePath, Me.ServiceName, Me.MainNodeCol, Me.NodeStatus})
        Me.NodeList.Location = New System.Drawing.Point(10, 12)
        Me.NodeList.Name = "NodeList"
        Me.NodeList.RowHeadersWidth = 51
        Me.NodeList.RowTemplate.Height = 24
        Me.NodeList.Size = New System.Drawing.Size(1068, 441)
        Me.NodeList.TabIndex = 45
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.IPBox)
        Me.GroupBox3.Controls.Add(Me.PortBox)
        Me.GroupBox3.Controls.Add(Me.MainNodeCheck)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.AddNodeBtn)
        Me.GroupBox3.Controls.Add(Me.ServiceText)
        Me.GroupBox3.Controls.Add(Me.Button4)
        Me.GroupBox3.Controls.Add(Me.NodeName)
        Me.GroupBox3.Controls.Add(Me.LogPathBox)
        Me.GroupBox3.Controls.Add(Me.Button3)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Location = New System.Drawing.Point(10, 459)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(489, 210)
        Me.GroupBox3.TabIndex = 46
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Node List Managment"
        '
        'NodeNameG
        '
        Me.NodeNameG.HeaderText = "Node Name"
        Me.NodeNameG.MinimumWidth = 6
        Me.NodeNameG.Name = "NodeNameG"
        Me.NodeNameG.Width = 125
        '
        'NodeIP
        '
        Me.NodeIP.HeaderText = "Node IP"
        Me.NodeIP.MinimumWidth = 6
        Me.NodeIP.Name = "NodeIP"
        Me.NodeIP.ReadOnly = True
        Me.NodeIP.Width = 85
        '
        'NodePort
        '
        Me.NodePort.HeaderText = "Node Port"
        Me.NodePort.MinimumWidth = 6
        Me.NodePort.Name = "NodePort"
        Me.NodePort.ReadOnly = True
        Me.NodePort.Width = 40
        '
        'NodePath
        '
        Me.NodePath.HeaderText = "Node Path"
        Me.NodePath.MinimumWidth = 6
        Me.NodePath.Name = "NodePath"
        Me.NodePath.ReadOnly = True
        Me.NodePath.Width = 170
        '
        'ServiceName
        '
        Me.ServiceName.HeaderText = "Node Service Name"
        Me.ServiceName.MinimumWidth = 6
        Me.ServiceName.Name = "ServiceName"
        Me.ServiceName.ReadOnly = True
        Me.ServiceName.Width = 125
        '
        'MainNodeCol
        '
        Me.MainNodeCol.HeaderText = "Main Node"
        Me.MainNodeCol.MinimumWidth = 6
        Me.MainNodeCol.Name = "MainNodeCol"
        Me.MainNodeCol.ReadOnly = True
        Me.MainNodeCol.Width = 40
        '
        'NodeStatus
        '
        Me.NodeStatus.HeaderText = "Node Service Running"
        Me.NodeStatus.MinimumWidth = 6
        Me.NodeStatus.Name = "NodeStatus"
        Me.NodeStatus.ReadOnly = True
        Me.NodeStatus.Width = 125
        '
        'StartNodeBtn
        '
        Me.StartNodeBtn.Location = New System.Drawing.Point(22, 117)
        Me.StartNodeBtn.Name = "StartNodeBtn"
        Me.StartNodeBtn.Size = New System.Drawing.Size(107, 27)
        Me.StartNodeBtn.TabIndex = 48
        Me.StartNodeBtn.Text = "Start Node"
        Me.StartNodeBtn.UseVisualStyleBackColor = True
        '
        'StopNodeBtn
        '
        Me.StopNodeBtn.Location = New System.Drawing.Point(144, 117)
        Me.StopNodeBtn.Name = "StopNodeBtn"
        Me.StopNodeBtn.Size = New System.Drawing.Size(107, 27)
        Me.StopNodeBtn.TabIndex = 49
        Me.StopNodeBtn.Text = "Stop Node"
        Me.StopNodeBtn.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(1, 675)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(1358, 226)
        Me.TextBox1.TabIndex = 47
        '
        'ArhiveBtn
        '
        Me.ArhiveBtn.Location = New System.Drawing.Point(22, 161)
        Me.ArhiveBtn.Name = "ArhiveBtn"
        Me.ArhiveBtn.Size = New System.Drawing.Size(107, 27)
        Me.ArhiveBtn.TabIndex = 50
        Me.ArhiveBtn.Text = "Arhive Logs"
        Me.ArhiveBtn.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1367, 903)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.NodeList)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Storj Node Toolbox"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.NodeList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button3 As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents LogPathBox As TextBox
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents Label4 As Label
    Friend WithEvents SaveUserID As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents UserIDBox As TextBox
    Friend WithEvents NodeName As TextBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Button4 As Button
    Friend WithEvents AddNodeBtn As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents IPBox As TextBox
    Friend WithEvents PortBox As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents ServiceText As TextBox
    Friend WithEvents MainNodeCheck As CheckBox
    Friend WithEvents NodeList As DataGridView
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents SleepTimeBox As TextBox
    Friend WithEvents NodeNameG As DataGridViewTextBoxColumn
    Friend WithEvents NodeIP As DataGridViewTextBoxColumn
    Friend WithEvents NodePort As DataGridViewTextBoxColumn
    Friend WithEvents NodePath As DataGridViewTextBoxColumn
    Friend WithEvents ServiceName As DataGridViewTextBoxColumn
    Friend WithEvents MainNodeCol As DataGridViewTextBoxColumn
    Friend WithEvents NodeStatus As DataGridViewTextBoxColumn
    Friend WithEvents StopNodeBtn As Button
    Friend WithEvents StartNodeBtn As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ArhiveBtn As Button
End Class
