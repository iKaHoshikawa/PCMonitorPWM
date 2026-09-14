# PCMonitorPWM

![GitHub license](https://img.shields.io/github/license/iKaHoshikawa/PCMonitorPWM)
![GitHub stars](https://img.shields.io/github/stars/iKaHoshikawa/PCMonitorPWM)
![GitHub issues](https://img.shields.io/github/issues/iKaHoshikawa/PCMonitorPWM)

一款使用物理指针显示数据的PC性能监视器——使用移动的指针观测PC硬件的使用数据，基于Arduino和PCA9685。

Physical PC performance monitor with analog gauges — watch your PC hardware usage come to life with moving needles, powered by Arduino &amp; PCA9685.

## 🛠️ 硬件清单 / Hardware List

| 组件 / Component | 型号 / Model | 数量 / Qty | 备注 / Remarks |
|------|----------|------|------|
| 微控制器 / Microcontroller | Arduino Leonardo | 1 | - |
| PWM驱动模块 / PWM Driver | PCA9685（16 通道 / Channels） | 1 | I²C 接口 / interface |
| 电压表 / Voltmeter | 85C1 (0-5V DC) | 7 |  |
| USB 数据线 / Cable | Micro USB | 1 |  |
| 杜邦线 / Jumper Wires | 母对母、公对母、公对公 / F-F, F-M, M-M | 若干 / Several | 用于连接各个模块 / For connections between modules |
| 面包板 / Breadboard | - | 1 | 方便测试接线 / For testing and wire connection |
| DFPlayer mini | - | 1 | 用于连接报警喇叭 / For connections to the alarm speaker |
| 报警喇叭 / Alarm Speaker | 5W | 1 | 报警 / For alarm |
| LCD 屏幕 / Screen | 1602 | 1 | 显示部分数据 / For displaying part of the data |
| 开关 / Switch | - | 1 | 控制喇叭开关 / For controlling the power of the speaker |
| I²C转1602转接板 / I²C LCD1602 Adapter | PCF8574 | 1 | 驱动LCD屏幕 / For driving the LCD screen |
| TF 卡 / Card | 32MB ~ 32GB | 1 | 存储报警音频 / For saving the alarm audio files |
| 发光二极管 / LED | 红、黄、绿各一 / One each of red, yellow and green | 3 | 状态显示 / For status displaying |

## 🔌 线缆连接 / Wire Connection

### Arduino Leonardo

| 将 / Connect | 接到 / to |
| --------- | ---------|
| PCA9685.SDA | SDA |
| PCA9685.SCL | SCL |
| 绿色发光二极管 / Green LED | 4 |
| 黄色发光二极管 / Yellow LED | 7 |
| 红色发光二极管 / Red LED | 8 |
| DFPlayer mini.RX | 1 |
| DFPlayer mini.TX | 0 |
| I2C转1602转接板.SDA / I2C LCD1602 adapter.SDA | 2 |
| I2C转1602转接板.SCL / I2C LCD1602 adapter.SCL | 3 |

所有元件共地。

All components share a common GND.

### PCA9685（全部接到VCC而非V+引脚 / All wires should be connected to VCC instead of V+）

| 将 / Connect | 接到 / to |
| --------- | ---------|
| 电压表：CPU占用率 / Voltmeter: CPU Utilization | 0 |
| 电压表：CPU温度 / Voltmeter: CPU Temperature | 2 |
| 电压表：CPU功耗 / Voltmeter: CPU Power | 4 |
| 电压表：GPU占用率 / Voltmeter: GPU Utilization | 6 |
| 电压表：GPU温度 / Voltmeter: GPU Temperature | 8 |
| 电压表：GPU功耗 / Voltmeter: GPU Power | 10 |
| 电压表：内存占用率 / Voltmeter: RAM Utilization | 12 |
| LCD蓝色背光 / LCD Blue Backlight | 13 |
| LCD绿色背光 / LCD Green Backlight | 14 |
| LCD红色背光 / LCD Red Backlight | 15 |

所有元件共地。

All components share a common GND.

### I²C转1602转接板 / I²C LCD1602 Adapter

转接板大多不用丝印展示引脚定义。请详询商家，按对应位置连接1602屏幕。

Most adapter boards do not use silkscreen to show the pin definitions. Please consult the seller for details, and connect the 1602 screen according to the corresponding positions.

### DFPlayer mini

| 将 / Connect | 接到 / to |
| --------- | ---------|
| 喇叭.正极 / Speaker.Positive | SPK_0 |
| 喇叭.负极 / Speaker.Negative | SPK_1 |

建议将开关串联在正极上。

It is recommended to connect the switch in series on the positive terminal.

### 关于TF卡 / About the TF Card

请将欲使用的表示CPU和GPU同时温度过高、表示GPU温度过高和表示CPU温度过高的音频分别保存为`0001.mp3`、`0002.mp3`和`0003.mp3`，存储于格式化为FAT16或FAT32的TF卡中，并将其插入DFPlayer mini。

Please save the audio files to be used for indicating that both the CPU and GPU temperatures are too high, that the GPU temperature is too high, and that the CPU temperature is too high as `0001.mp3`, `0002.mp3`, and `0003.mp3` respectively, store them on a TF card formatted as FAT16 or FAT32, and insert it into the DFPlayer mini.

**电压表表盘和亚克力外壳位于`image`文件夹，可自行取用，亦可重新设计。**

**The voltmeter dials acrylic enclosure and are located in the `image` folder. You can use them as needed, or redesign them yourself.**

## 💻 使用方法 / Usage

1. 下载Release中发布的安装包安装上位机软件，因涉及系统信息读取，可能会报告为病毒，忽略即可；亦可从`console`文件夹获取源码自行编译使用。

2. 使用Arduino IDE将`controller`文件夹中的`.ino`文件烧录到Arduino Leonardo中。

3. 在上位机软件中选择正确的串口（通常会自动识别），单击“连接”即可。如果CPU数据显示为0，请先安装PawnIO。部分显卡（如部分品牌RTX 4060）功耗数据不会正常显示，请先使用厂商驱动确认能否正确读取数据，如果不能则无解；如能，请提交issue。



1. Download the installer package published in Releases and install the console software. Because it involves reading system information, it may be reported as a virus — just ignore it. Alternatively, you can get the source code from the `console` folder and compile it yourself.
   
2. Use the Arduino IDE to flash the `.ino` file in the `controller` folder to the Arduino Leonardo.
   
3. In the console software, select the correct serial port (it is usually detected automatically), then click "连接". If the CPU data shows 0, please install PawnIO first. On some graphics cards (such as certain branded RTX 4060), the power consumption data will not display properly. Please first use the manufacturer's driver to check whether the data can be read correctly. If it cannot, there is no solution; if it can, please submit an issue.
