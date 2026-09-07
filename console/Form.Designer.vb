<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Label1 = New Label()
        ComboBox1 = New ComboBox()
        Timer1 = New Timer(components)
        Label2 = New Label()
        GroupBox1 = New GroupBox()
        Label14 = New Label()
        Label13 = New Label()
        Label12 = New Label()
        Label5 = New Label()
        Label4 = New Label()
        Label3 = New Label()
        GroupBox2 = New GroupBox()
        Label17 = New Label()
        Label16 = New Label()
        Label15 = New Label()
        Label8 = New Label()
        Label7 = New Label()
        Label6 = New Label()
        Label9 = New Label()
        CheckBox1 = New CheckBox()
        Label10 = New Label()
        ComboBox2 = New ComboBox()
        Label11 = New Label()
        ComboBox3 = New ComboBox()
        Label18 = New Label()
        Button1 = New Button()
        Button2 = New Button()
        Button3 = New Button()
        Label19 = New Label()
        Label20 = New Label()
        Timer2 = New Timer(components)
        NotifyIcon1 = New NotifyIcon(components)
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(73, 20)
        Label1.TabIndex = 0
        Label1.Text = "串口选择"
        ' 
        ' ComboBox1
        ' 
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(91, 6)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(151, 28)
        ComboBox1.TabIndex = 1
        ' 
        ' Timer1
        ' 
        Timer1.Enabled = True
        Timer1.Interval = 1000
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(248, 9)
        Label2.Name = "Label2"
        Label2.Size = New Size(73, 20)
        Label2.TabIndex = 2
        Label2.Text = "连接状态"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(Label14)
        GroupBox1.Controls.Add(Label13)
        GroupBox1.Controls.Add(Label12)
        GroupBox1.Controls.Add(Label5)
        GroupBox1.Controls.Add(Label4)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Location = New Point(12, 114)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(481, 58)
        GroupBox1.TabIndex = 3
        GroupBox1.TabStop = False
        GroupBox1.Text = "CPU"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(360, 23)
        Label14.Name = "Label14"
        Label14.Size = New Size(95, 20)
        Label14.TabIndex = 5
        Label14.Text = "初始化中……"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(215, 23)
        Label13.Name = "Label13"
        Label13.Size = New Size(95, 20)
        Label13.TabIndex = 4
        Label13.Text = "初始化中……"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(69, 23)
        Label12.Name = "Label12"
        Label12.Size = New Size(95, 20)
        Label12.TabIndex = 3
        Label12.Text = "初始化中……"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(313, 23)
        Label5.Name = "Label5"
        Label5.Size = New Size(41, 20)
        Label5.TabIndex = 2
        Label5.Text = "功耗"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(168, 23)
        Label4.Name = "Label4"
        Label4.Size = New Size(41, 20)
        Label4.TabIndex = 1
        Label4.Text = "温度"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(6, 23)
        Label3.Name = "Label3"
        Label3.Size = New Size(57, 20)
        Label3.TabIndex = 0
        Label3.Text = "占用率"
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(Label17)
        GroupBox2.Controls.Add(Label16)
        GroupBox2.Controls.Add(Label15)
        GroupBox2.Controls.Add(Label8)
        GroupBox2.Controls.Add(Label7)
        GroupBox2.Controls.Add(Label6)
        GroupBox2.Location = New Point(12, 178)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(481, 58)
        GroupBox2.TabIndex = 4
        GroupBox2.TabStop = False
        GroupBox2.Text = "GPU"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(360, 23)
        Label17.Name = "Label17"
        Label17.Size = New Size(95, 20)
        Label17.TabIndex = 6
        Label17.Text = "初始化中……"
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(215, 23)
        Label16.Name = "Label16"
        Label16.Size = New Size(95, 20)
        Label16.TabIndex = 5
        Label16.Text = "初始化中……"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(69, 23)
        Label15.Name = "Label15"
        Label15.Size = New Size(95, 20)
        Label15.TabIndex = 4
        Label15.Text = "初始化中……"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(313, 23)
        Label8.Name = "Label8"
        Label8.Size = New Size(41, 20)
        Label8.TabIndex = 3
        Label8.Text = "功耗"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(168, 23)
        Label7.Name = "Label7"
        Label7.Size = New Size(41, 20)
        Label7.TabIndex = 3
        Label7.Text = "温度"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(6, 23)
        Label6.Name = "Label6"
        Label6.Size = New Size(57, 20)
        Label6.TabIndex = 3
        Label6.Text = "占用率"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(12, 239)
        Label9.Name = "Label9"
        Label9.Size = New Size(89, 20)
        Label9.TabIndex = 4
        Label9.Text = "RAM占用率"
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(12, 264)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(95, 24)
        CheckBox1.TabIndex = 4
        CheckBox1.Text = "开机自启"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(12, 44)
        Label10.Name = "Label10"
        Label10.Size = New Size(68, 20)
        Label10.TabIndex = 6
        Label10.Text = "CPU选择"
        ' 
        ' ComboBox2
        ' 
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.FormattingEnabled = True
        ComboBox2.Location = New Point(91, 41)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(402, 28)
        ComboBox2.TabIndex = 2
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(12, 80)
        Label11.Name = "Label11"
        Label11.Size = New Size(69, 20)
        Label11.TabIndex = 8
        Label11.Text = "GPU选择"
        ' 
        ' ComboBox3
        ' 
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.FormattingEnabled = True
        ComboBox3.Location = New Point(91, 77)
        ComboBox3.Name = "ComboBox3"
        ComboBox3.Size = New Size(402, 28)
        ComboBox3.TabIndex = 3
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(125, 239)
        Label18.Name = "Label18"
        Label18.Size = New Size(95, 20)
        Label18.TabIndex = 10
        Label18.Text = "初始化中……"
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(399, 259)
        Button1.Name = "Button1"
        Button1.Size = New Size(94, 29)
        Button1.TabIndex = 11
        Button1.Text = "自检"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(399, 294)
        Button2.Name = "Button2"
        Button2.Size = New Size(94, 29)
        Button2.TabIndex = 12
        Button2.Text = "连接"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(399, 329)
        Button3.Name = "Button3"
        Button3.Size = New Size(94, 29)
        Button3.TabIndex = 13
        Button3.Text = "保存设置"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Location = New Point(12, 329)
        Label19.Name = "Label19"
        Label19.Size = New Size(146, 20)
        Label19.TabIndex = 14
        Label19.Text = "By 星川伊夏 / iKa H."
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(325, 9)
        Label20.Name = "Label20"
        Label20.Size = New Size(57, 20)
        Label20.TabIndex = 15
        Label20.Text = "未连接"
        ' 
        ' Timer2
        ' 
        Timer2.Interval = 5000
        ' 
        ' NotifyIcon1
        ' 
        NotifyIcon1.Text = "NotifyIcon1"
        NotifyIcon1.Visible = True
        ' 
        ' Form
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(505, 370)
        Controls.Add(Label20)
        Controls.Add(Label19)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(Label18)
        Controls.Add(ComboBox3)
        Controls.Add(Label11)
        Controls.Add(ComboBox2)
        Controls.Add(Label10)
        Controls.Add(CheckBox1)
        Controls.Add(Label9)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
        Controls.Add(Label2)
        Controls.Add(ComboBox1)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        MinimizeBox = False
        Name = "Form"
        ShowIcon = False
        StartPosition = FormStartPosition.CenterScreen
        Text = "监视器控制台"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents ComboBox3 As ComboBox
    Friend WithEvents Label18 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Label19 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Timer2 As Timer
    Friend WithEvents NotifyIcon1 As NotifyIcon

End Class
