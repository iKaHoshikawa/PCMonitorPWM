# PCMonitorPWM

![GitHub license](https://img.shields.io/github/license/iKaHoshikawa/PCMonitorPWM)
![GitHub stars](https://img.shields.io/github/stars/iKaHoshikawa/PCMonitorPWM)
![GitHub issues](https://img.shields.io/github/issues/iKaHoshikawa/PCMonitorPWM)

Physical PC performance monitor with analog gauges — watch your PC hardware usage come to life with moving needles, powered by Arduino &amp; PCA9685.
一款使用物理指针显示数据的PC性能监视器——使用移动的指针观测PC硬件的使用数据，基于Arduino和PCA9685。

## 🛠️ 硬件清单 Hardware List

| 组件 / Component | 型号 / Model | 数量 / Qty | 备注 / Remarks |
|------|----------|------|------|
| 微控制器 / Microprocessor | Arduino Leonardo | 1 | 必须带 USB HID 功能 / Must with USB HID |
| PWM 驱动模块 / PWM Driver | PCA9685（16 通道 / Channels） | 1 | I²C 接口 / interface |
| 电压表 | 85C1 (0-5V DC) | 7 |  |
| USB 数据线 / USB Cable | Micro USB | 1 |  |
| 杜邦线 / Jumper Wires | 母对母、公对母、公对公 / F-F, F-M, M-M | 若干 / Serveral | 用于连接各个模块 / For connections between modules |
| 面包板 / Breadboard | - | 1 | 方便测试接线 / For testing and cable connection |
| DFPlayer mini | - | 1 | 用于连接报警喇叭 / For connections to the alarm speaker |
| 报警喇叭 / Alarm speaker | 5W | 1 | 报警 / For alarm |
| LCD屏幕 / LCD Screen | 1602 | 1 | 部分显示数据 / For part of data displaying |
| 开关 / Switch | - | 1 | 控制喇叭开关 / For controlling the power of the speaker |
