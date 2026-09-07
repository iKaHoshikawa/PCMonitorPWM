Imports System.Diagnostics
Imports System.IO
Imports System.IO.Ports
Imports System.Management
Imports System.Text.Json
Imports System.Threading
Imports System.Windows.Forms.Design.AxImporter
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports LibreHardwareMonitor.Hardware
Public Class Form
    Dim PriSp As System.IO.Ports.SerialPort
    Dim CPULoad As Single = 0, CPUTemp As Single = 0, CPUPower As Single = 0
    Dim GPULoad As Single = 0, GPUTemp As Single = 0, GPUPower As Single = 0
    Dim RAMLoad As Single = 0
    Dim PortUsing As String = ""
    Dim CL As Integer, CT As Integer, CP As Integer, GL As Integer, GT As Integer, GP As Integer, RL As Integer, CTHigh As Integer = 0, GTHigh As Integer = 0
    Dim SavedGPU As String, SavedCPU As String
    Private ReadOnly configPath As String = Path.Combine(Application.StartupPath, "config.json")
    Private ReadOnly PC As New LibreHardwareMonitor.Hardware.Computer() With
        {
            .IsCpuEnabled = True,
            .IsGpuEnabled = True,
            .IsMemoryEnabled = True,
            .IsMotherboardEnabled = True,
            .IsControllerEnabled = True,
            .IsStorageEnabled = False
        }
    Private WithEvents trayIcon As New NotifyIcon()
    Private WithEvents trayMenu As New ContextMenuStrip()
    Private Sub InitTrayIcon()
        trayIcon.Icon = Me.Icon
        trayIcon.Text = "监视器运行中"
        trayIcon.Visible = True
        Dim exitMenuItem As New ToolStripMenuItem("退出")
        AddHandler exitMenuItem.Click, Sub(s, ev)
                                           trayIcon.Visible = False
                                           PC.Close()
                                           If PriSp IsNot Nothing AndAlso PriSp.IsOpen Then
                                               PriSp.Close()
                                           End If
                                           Application.Exit()
                                       End Sub
        trayMenu.Items.Add(exitMenuItem)
        trayIcon.ContextMenuStrip = trayMenu
        AddHandler trayIcon.DoubleClick, Sub(s, ev)
                                             Me.Show()
                                             Me.WindowState = FormWindowState.Normal
                                             Me.Activate()
                                         End Sub
    End Sub
    Private Sub connect(portNumber As String)
        Label20.Text = "连接中……"
        Button2.Enabled = False
        Dim ConResult As Boolean = False
        Try
            If PriSp IsNot Nothing AndAlso PriSp.IsOpen Then
                PriSp.Close()
            End If
            PriSp = New System.IO.Ports.SerialPort(portNumber, 9600)
            PriSp.DtrEnable = True
            PriSp.RtsEnable = True
            PriSp.ReadTimeout = 500
            PriSp.WriteTimeout = 500
            Dim resp As String = ""
            Dim lockObj As New Object()

            AddHandler PriSp.DataReceived, Sub(s, args)
                                               Try
                                                   Dim spInst = CType(s, System.IO.Ports.SerialPort)
                                                   Dim text As String = spInst.ReadExisting()
                                                   SyncLock lockObj
                                                       resp &= text
                                                   End SyncLock
                                               Catch
                                               End Try
                                           End Sub

            PriSp.Open()
            System.Threading.Thread.Sleep(1500)
            PriSp.WriteLine("CONNECTING")
            Dim startTime As DateTime = DateTime.Now
            While (DateTime.Now - startTime).TotalMilliseconds < 2000
                SyncLock lockObj
                    If resp.Contains("CON_DONE") Then
                        ConResult = True
                        Exit While
                    End If
                End SyncLock
                System.Threading.Thread.Sleep(50)
            End While
            If ConResult Then
                Label20.Text = "连接成功"
                Button2.Enabled = True
                Button2.Text = "断开连接"
                ComboBox1.Enabled = False
                Timer1.Enabled = True
            Else
                PriSp.Close()
                Label20.Text = "连接超时"
                Button2.Enabled = True
                Button2.Text = "连接"
                ComboBox1.Enabled = True
            End If
        Catch ex As Exception
            If PriSp IsNot Nothing AndAlso PriSp.IsOpen Then
                PriSp.Close()
            End If
            Label20.Text = "连接失败"
            Button2.Enabled = True
            Button2.Text = "连接"
            ComboBox1.Enabled = True
        End Try
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "连接" AndAlso (Label20.Text = "未连接" OrElse Label20.Text = "连接失败" OrElse Label20.Text = "连接超时") Then
            Dim pn As String = ComboBox1.SelectedItem?.ToString.Split(":"c)(0)
            connect(pn)
        ElseIf Button2.Text = "断开连接" AndAlso Label20.Text = "连接成功" Then
            PriSp.Close()
            ComboBox1.Enabled = True
            Button2.Text = "连接"
            Label20.Text = "未连接"
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        CPULoad = 0 : CPUTemp = 0 : CPUPower = 0
        GPULoad = 0 : GPUTemp = 0 : GPUPower = 0

        Dim selectedCpuStr As String = ComboBox2.SelectedItem?.ToString()
        Dim selectedGpuStr As String = ComboBox3.SelectedItem?.ToString()

        For Each hw In PC.Hardware
            hw.Update()
            If selectedGpuStr <> "未检测到可用GPU" AndAlso hw.Name = selectedGpuStr Then
                If hw.HardwareType = HardwareType.GpuNvidia OrElse
                   hw.HardwareType = HardwareType.GpuAmd OrElse
                   hw.HardwareType = HardwareType.GpuIntel Then
                    For Each sensor0 In hw.Sensors
                        If sensor0.SensorType = SensorType.Load AndAlso sensor0.Name.Contains("Core") Then
                            GPULoad = sensor0.Value.GetValueOrDefault()
                        ElseIf sensor0.SensorType = SensorType.Temperature AndAlso sensor0.Name.Contains("Core") Then
                            GPUTemp = sensor0.Value.GetValueOrDefault()
                        ElseIf sensor0.SensorType = SensorType.Power Then
                            If Not sensor0.Name.Contains("Limit") AndAlso
                           Not sensor0.Name.Contains("Target") AndAlso
                           Not sensor0.Name.Contains("Max") Then
                                If sensor0.Name.Contains("Package") OrElse
                               sensor0.Name.Contains("ASIC") OrElse
                               sensor0.Name.Contains("GPU Power") OrElse
                               sensor0.Name.Equals("Power") OrElse
                               sensor0.Name.Contains("Board") Then
                                    GPUPower = sensor0.Value.GetValueOrDefault()
                                End If
                            End If
                        End If
                    Next
                End If
            End If
            If hw.HardwareType = HardwareType.Cpu AndAlso
           (hw.Name = selectedCpuStr OrElse selectedCpuStr = "同时监测多个CPU") Then
                hw.Update()
                For Each subHw In hw.SubHardware
                    subHw.Update()
                Next
                For Each sensor1 In hw.Sensors
                    If sensor1.SensorType = SensorType.Load AndAlso sensor1.Name.Contains("Total") Then
                        If selectedCpuStr <> "同时监测多个CPU" Then
                            CPULoad = sensor1.Value.GetValueOrDefault()
                        Else
                            CPULoad = Math.Max(CPULoad, sensor1.Value.GetValueOrDefault())
                        End If
                    ElseIf sensor1.SensorType = SensorType.Temperature AndAlso
                       (sensor1.Name.Contains("Core") OrElse
                        sensor1.Name.Contains("Package") OrElse
                        sensor1.Name.Contains("Tctl") OrElse
                        sensor1.Name.Contains("CPU")) Then
                        If selectedCpuStr <> "同时监测多个CPU" Then
                            CPUTemp = sensor1.Value.GetValueOrDefault()
                        Else
                            CPUTemp = Math.Max(CPUTemp, sensor1.Value.GetValueOrDefault())
                        End If
                    ElseIf sensor1.SensorType = SensorType.Power AndAlso sensor1.Name.Contains("Package") Then
                        If selectedCpuStr <> "同时监测多个CPU" Then
                            CPUPower = sensor1.Value.GetValueOrDefault()
                        Else
                            CPUPower += sensor1.Value.GetValueOrDefault()
                        End If
                    End If
                Next
            End If
            If hw.HardwareType = HardwareType.Memory Then
                For Each sensor2 In hw.Sensors
                    If sensor2.SensorType = SensorType.Load Then
                        RAMLoad = sensor2.Value.GetValueOrDefault()
                    End If
                Next
            End If
        Next
        Label15.Text = $"{GPULoad:F0} %"
        Label16.Text = $"{GPUTemp:F0} ℃"
        Label17.Text = $"{GPUPower:F0} W"
        Label12.Text = $"{CPULoad:F0} %"
        Label13.Text = $"{CPUTemp:F0} ℃"
        Label14.Text = $"{CPUPower:F0} W"
        Label18.Text = $"{RAMLoad:F0} %"
        '下面开始发送数据包。
        If Label20.Text = "连接成功" AndAlso Button2.Text = "断开连接" Then
            Dim activePorts As String() = System.IO.Ports.SerialPort.GetPortNames()
            Dim portsString As String = "," & String.Join(",", activePorts) & ","
            If portsString.Contains("," & PriSp.PortName & ",") Then '先判定监视器没有断开再发包。
                CL = CInt(Math.Min(Math.Max((CPULoad / 100.0F) * 511, 0), 511))
                If CPUTemp <= 85 Then
                    CT = CInt(Math.Min(Math.Max((CPUTemp - 20.0F) / (85.0F - 20.0F) * 511, 0), 511))
                    CTHigh = 0
                ElseIf CPUTemp > 85 Then
                    CT = 511
                    CTHigh = CPUTemp
                End If
                If CPUPower <= 100 Then
                    CP = CInt(Math.Min(Math.Max((CPUPower / 100.0F) * 511, 0), 511))
                ElseIf CPUPower > 100 Then
                    CP = 511
                End If
                GL = CInt(Math.Min(Math.Max((GPULoad / 100.0F) * 511, 0), 511))
                If GPUTemp <= 85 Then
                    GT = CInt(Math.Min(Math.Max((GPUTemp - 20.0F) / (85.0F - 20.0F) * 511, 0), 511))
                    GTHigh = 0
                ElseIf GPUTemp > 85 Then
                    GT = 511
                    GTHigh = GPUTemp
                End If
                If GPUPower <= 160 Then
                    GP = CInt(Math.Min(Math.Max((GPUPower / 160.0F) * 511, 0), 511))
                ElseIf GPUPower > 160 Then
                    GP = 511
                End If
                RL = CInt(Math.Min(Math.Max((RAMLoad / 100.0F) * 511, 0), 511))
                Dim T As String = DateTime.Now.ToString("HHmm")
                PriSp.DtrEnable = True
                PriSp.RtsEnable = True
                PriSp.ReadTimeout = 500
                PriSp.WriteTimeout = 500
                PriSp.WriteLine($"#{T},{CL},{CT},{CP},{GL},{GT},{GP},{RL},{CTHigh},{GTHigh}$")
                Timer1.Enabled = True
            ElseIf Not portsString.Contains("," & PriSp.PortName & ",") Then '先判定监视器没有断开再发包。
                PriSp.Close()
                ComboBox1.Enabled = True
                Button2.Text = "连接"
                Label20.Text = "未连接"
                MessageBox.Show("监视器断开，请重新连接。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not File.Exists(configPath) Then
            Dim config As New Conf()
            Dim options As New JsonSerializerOptions With
            {
                .WriteIndented = True
            }
            config.Port = ""
            config.SelectedCPU = ""
            config.SelectedGPU = ""
            config.startup = False
            Dim jsonText As String = JsonSerializer.Serialize(config, options)
            File.WriteAllText(configPath, jsonText)
            SavedCPU = ""
            SavedGPU = ""
        Else
            Try
                Dim jsonText As String = File.ReadAllText(configPath)
                Dim config As Conf = JsonSerializer.Deserialize(Of Conf)(jsonText)
                If config IsNot Nothing Then
                    ComboBox1.SelectedItem = config.Port
                    PortUsing = config.Port
                    SavedCPU = config.SelectedCPU
                    SavedGPU = config.SelectedGPU
                    CheckBox1.Checked = config.startup
                End If
            Catch ex As Exception
            End Try
        End If
        ComboBox3.Items.Clear()
        ComboBox2.Items.Clear()
        PC.Open()
        For Each hw In PC.Hardware
            hw.Update()
            For Each subHw In hw.SubHardware
                subHw.Update()
            Next
        Next
        For Each hardware In PC.Hardware
            If hardware.HardwareType = HardwareType.GpuNvidia OrElse
               hardware.HardwareType = HardwareType.GpuAmd OrElse
               hardware.HardwareType = HardwareType.GpuIntel Then
                ComboBox3.Items.Add(hardware.Name)
            End If
        Next
        If ComboBox3.Items.Count = 0 Then
            ComboBox3.Items.Add("未检测到可用GPU")
            ComboBox3.SelectedIndex = 0
            ComboBox3.Enabled = False
        ElseIf SavedGPU <> "" AndAlso ComboBox3.Items.Contains(SavedGPU) Then
            ComboBox3.SelectedItem = SavedGPU?.ToString()
        Else
            ComboBox3.SelectedIndex = 0
        End If
        For Each hardware In PC.Hardware
            If hardware.HardwareType = HardwareType.Cpu Then
                ComboBox2.Items.Add(hardware.Name)
            End If
        Next
        If ComboBox2.Items.Count >= 2 Then
            ComboBox2.Items.Add("同时监测多个CPU")
            If SavedCPU = "" Then
                ComboBox2.SelectedItem = "同时监测多个CPU"
            ElseIf SavedCPU = "Both" Then
                ComboBox2.SelectedItem = "同时监测多个CPU"
            Else
                ComboBox2.SelectedItem = SavedCPU?.ToString()
            End If
        ElseIf SavedCPU <> "" AndAlso SavedCPU <> "Both" AndAlso ComboBox2.Items.Contains(SavedCPU) Then
            ComboBox2.SelectedItem = SavedCPU?.ToString()
        Else
            ComboBox2.SelectedIndex = 0
        End If
        '被监测对象选择：结束。
        '加载串口列表。
        ComboBox1.Items.Clear()
        Dim ports As String() = SerialPort.GetPortNames()
        Dim defaultPort As String = ""
        For Each portName As String In ports
            Dim HSSuccess As Boolean = False
            Using sp As New System.IO.Ports.SerialPort(portName, 9600)
                sp.DtrEnable = True
                sp.RtsEnable = True
                sp.ReadTimeout = 300
                sp.WriteTimeout = 300
                Dim resp As String = ""
                Dim lockObj As New Object()
                AddHandler sp.DataReceived, Sub(s, args)
                                                Try
                                                    Dim serialPortInstance = CType(s, System.IO.Ports.SerialPort)
                                                    Dim text As String = serialPortInstance.ReadExisting()
                                                    SyncLock lockObj
                                                        resp &= text
                                                    End SyncLock
                                                Catch
                                                End Try
                                            End Sub
                Try
                    sp.Open()
                    System.Threading.Thread.Sleep(1500)
                    sp.WriteLine("WHERERU")
                    Dim startTime As DateTime = DateTime.Now
                    While (DateTime.Now - startTime).TotalMilliseconds < 1000
                        SyncLock lockObj
                            If resp.Contains("IMHERE") Then
                                HSSuccess = True
                                Exit While
                            End If
                        End SyncLock
                        System.Threading.Thread.Sleep(50)
                    End While
                Catch ex As Exception
                    HSSuccess = False
                End Try
            End Using
            Dim ComboItem As String = portName
            If HSSuccess Then
                ComboItem = portName & ": 发现监视器"
                If String.IsNullOrEmpty(defaultPort) Then
                    defaultPort = ComboItem
                End If
            End If
            ComboBox1.Items.Add(ComboItem)
        Next
        If Not String.IsNullOrEmpty(defaultPort) Then
            ComboBox1.SelectedItem = defaultPort
        ElseIf ComboBox1.Items.Count > 0 Then
            If PortUsing IsNot Nothing Then
                ComboBox1.SelectedItem = PortUsing?.ToString.Split(":"c)(0)
            Else
                ComboBox1.SelectedIndex = 0
            End If
        End If
        If CheckBox1.Checked = True Then
            If ComboBox1.SelectedItem?.ToString.Contains(":") Then
                Dim pn As String = ComboBox1.SelectedItem?.ToString.Split(":"c)(0)
                connect(pn)
            End If
        End If
        InitTrayIcon()
    End Sub

    Private Sub Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Me.Hide()
        Else
            PC.Close()
            If PriSp IsNot Nothing AndAlso PriSp.IsOpen Then
                PriSp.Close()
            End If
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Label20.Text <> "连接成功" AndAlso Label20.Text <> "未连接" AndAlso Label20.Text <> "连接超时" Then
            Label20.Text = "连接超时"
            Button2.Enabled = True
            Button2.Text = "连接"
            Timer2.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Label20.Text = "未连接" OrElse Label20.Text = "连接失败" Then
            MessageBox.Show("请先连接监视器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf Label20.Text = "连接成功" Then
            Timer1.Enabled = False
            Dim pn As String = ComboBox1.SelectedItem?.ToString.Split(":"c)(0)
            Using sp As New System.IO.Ports.SerialPort(pn, 9600)
                sp.DtrEnable = True
                sp.RtsEnable = True
                sp.ReadTimeout = 500
                sp.WriteTimeout = 500
                Dim lockObj As New Object()
                Try
                    sp.Open()
                    System.Threading.Thread.Sleep(1500)
                    sp.WriteLine("TEST")
                Catch
                End Try
            End Using
            Timer1.Enabled = True
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim config As New Conf()
        Dim options As New JsonSerializerOptions With
            {
                .WriteIndented = True
            }
        Dim taskName As String = "PCMonitorbyiKaH"
        Dim exePath As String = Application.ExecutablePath
        If CheckBox1.Checked = False AndAlso config.startup = True Then
            Dim cmd As String = $"schtasks /delete /tn ""{taskName}"" /f"
            Dim psi As New ProcessStartInfo("cmd.exe", "/c " & cmd) With {
                .CreateNoWindow = True,
                .UseShellExecute = False,
                .Verb = "runas"
            }
            Process.Start(psi)
        ElseIf CheckBox1.Checked = True AndAlso config.startup = False Then
            Dim cmd As String = $"schtasks /create /tn ""{taskName}"" /tr ""'{exePath}'"" /sc ONSTART /rl HIGHEST /f"
            Dim psi As New ProcessStartInfo("cmd.exe", "/c " & cmd) With {
                .CreateNoWindow = True,
                .UseShellExecute = False,
                .Verb = "runas"
            }
            Process.Start(psi)
        End If
        Try
            config.Port = ComboBox1.SelectedItem?.ToString()
            If ComboBox2.SelectedItem <> "同时监测多个CPU" Then
                config.SelectedCPU = ComboBox2.SelectedItem?.ToString()
            Else
                config.SelectedCPU = "Both"
            End If
            config.SelectedGPU = ComboBox3.SelectedItem?.ToString()
            config.startup = CheckBox1.Checked
            Dim jsonText As String = JsonSerializer.Serialize(config, options)
            File.WriteAllText(configPath, jsonText)
            MessageBox.Show("配置保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
