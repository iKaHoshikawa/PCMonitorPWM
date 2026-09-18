#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>
#include <DFRobotDFPlayerMini.h>
#include <SoftwareSerial.h>
#include <SPI.h>
#include <LiquidCrystal_I2C.h>

#define LED_R 8
#define LED_Y 7
#define LED_G 4
#define LCD_B 13
#define LCD_G 14
#define LCD_R 15

DFRobotDFPlayerMini DFPlayer;
Adafruit_PWMServoDriver pca = Adafruit_PWMServoDriver(0x40);
LiquidCrystal_I2C lcd(0x27, 16, 2);

unsigned long lastResponseTime = 0;
bool handshaking = true;
bool connectedornot = false;
int playing = 0;
int HH = 0, MM = 0;

int T = 0, CL = 0, CT = 0, CP = 0, GL = 0, GT = 0, GP = 0, RL = 0, CTH = 0, GTH = 0;
String dataBuffer = "";
String cmdBuffer = "";
int CLHContinue = 0, GLHContinue = 0;

byte degc[8] = {
  0b01000,
  0b10100,
  0b01000,
  0b00110,
  0b01001,
  0b01000,
  0b01001,
  0b00110
};

byte cpu[8] = {
  0b00000,
  0b01010,
  0b11111,
  0b10001,
  0b10001,
  0b11111,
  0b01010,
  0b00000
};

byte gpu[8] = {
  0b00001,
  0b11111,
  0b01010,
  0b11010,
  0b11010,
  0b11010,
  0b11010,
  0b01110
};

void test() {
  for (int i = 0; i < 13; i++) {
    pca.setPWM(i, 0, 4095);
  }
  digitalWrite(LED_G, HIGH);
  digitalWrite(LED_Y, HIGH);
  digitalWrite(LED_R, HIGH);
  delay(500);
  for (int i = 0; i < 13; i++) {
    pca.setPWM(i, 0, 0);
  }
  digitalWrite(LED_G, LOW);
  digitalWrite(LED_Y, LOW);
  digitalWrite(LED_R, LOW);
}

int getValue(String data, int index) {
  int start = 0;
  for (int i = 0; i < index; i++) {
    start = data.indexOf(',', start) + 1;
  }
  int end = data.indexOf(',', start);
  if (end == -1) end = data.length();
  return data.substring(start, end).toInt();
}

void setup() {
  delay(100);
  Serial1.begin(9600);
  Serial.begin(9600);
  lcd.init();
  lcd.createChar(0, degc);
  lcd.createChar(1, cpu);
  lcd.createChar(2, gpu);
  lcd.clear();
  pinMode(LED_G, OUTPUT);
  pinMode(LED_Y, OUTPUT);
  pinMode(LED_R, OUTPUT);
  digitalWrite(LED_G, LOW);
  digitalWrite(LED_Y, LOW);
  digitalWrite(LED_R, LOW);
  pca.begin();
  pca.setPWMFreq(1000);
  pca.setPWM(LCD_R, 0, 1024);
  pca.setPWM(LCD_G, 0, 1024);
  pca.setPWM(LCD_B, 0, 1024);
  test();
  delay(500);
  if (DFPlayer.begin(Serial1)) {
    DFPlayer.volume(20);
    digitalWrite(LED_G, HIGH);
    delay(1000);
    digitalWrite(LED_G, LOW);
  } else {
    digitalWrite(LED_Y, HIGH);
    delay(1000);
    digitalWrite(LED_Y, LOW);
  }
  lcd.setCursor(0, 0);
  lcd.print("PCMonitor by iKa");
  lcd.setCursor(0, 1);
  lcd.print("Starting up...  ");
}

void loop() {
  while (Serial.available() > 0) {
    char c = Serial.read();
    if (c == '\n' || c == '\r') {
      if (cmdBuffer.length() > 0) {
        cmdBuffer.trim();
        if (cmdBuffer == "WHERERU") {
          Serial.println("IMHERE");
        } else if (cmdBuffer == "CONNECTING") {
          Serial.println("CON_DONE");
          handshaking = false;
          connectedornot = true;
        } else if (cmdBuffer == "TEST") {
          test();
        }
        cmdBuffer = "";
      }
      continue;
    }
    if (c == '#') {
      dataBuffer = "#";
      continue;
    }
    if (c == '$') {
      if (dataBuffer.startsWith("#")) {
        String raw = dataBuffer.substring(1);
        T = getValue(raw, 0);
        CL = getValue(raw, 1);
        CT = getValue(raw, 2);
        CP = getValue(raw, 3);
        GL = getValue(raw, 4);
        GT = getValue(raw, 5);
        GP = getValue(raw, 6);
        RL = getValue(raw, 7);
        CTH = getValue(raw, 8);
        GTH = getValue(raw, 9);
        lastResponseTime = millis();
        connectedornot = true;
        handshaking = false;
      }
      dataBuffer = "";
      continue;
    }
    if (handshaking) {
      cmdBuffer += c;
    } else {
      dataBuffer += c;
    }
  }
  if (connectedornot && (millis() - lastResponseTime >= 5000)) {
    connectedornot = false;
    handshaking = true;
    cmdBuffer = "";
    dataBuffer = "";
    digitalWrite(LED_G, LOW);
  }
  if (handshaking) {
    digitalWrite(LED_G, LOW);
    digitalWrite(LED_Y, LOW);
    digitalWrite(LED_R, LOW);
    DFPlayer.stop();
    playing = 0;
    pca.setPWM(0, 0, 0);
    pca.setPWM(2, 0, 0);
    pca.setPWM(4, 0, 0);
    pca.setPWM(6, 0, 0);
    pca.setPWM(8, 0, 0);
    pca.setPWM(10, 0, 0);
    pca.setPWM(12, 0, 0);
    pca.setPWM(LCD_R, 0, 1024);
    pca.setPWM(LCD_G, 0, 1024);
    pca.setPWM(LCD_B, 0, 1024);
    lcd.setCursor(0, 0);
    lcd.print("PCMonitor by iKa");
    lcd.setCursor(0, 1);
    lcd.print("Zzzzzzzz........");
  } else {
    if (CL > 460) {
      CLHContinue += 1;
    } else {
      CLHContinue = 0;
    }
    if (GL > 460) {
      GLHContinue += 1;
    } else {
      GLHContinue = 0;
    }
    if (CTH == 0 && GTH == 0) {
      if (CLHContinue < 3 && CT < 409 && GLHContinue < 3 && GT < 409 && RL < 383) {
        pca.setPWM(0, 0, map(CL, 0, 511, 0, 4095));
        pca.setPWM(2, 0, map(CT, 0, 511, 0, 4095));
        pca.setPWM(4, 0, map(CP, 0, 511, 0, 4095));
        pca.setPWM(6, 0, map(GL, 0, 511, 0, 4095));
        pca.setPWM(8, 0, map(GT, 0, 511, 0, 4095));
        pca.setPWM(10, 0, map(GP, 0, 511, 0, 4095));
        pca.setPWM(12, 0, map(RL, 0, 511, 0, 4095));
        digitalWrite(LED_R, LOW);
        digitalWrite(LED_Y, LOW);
        digitalWrite(LED_G, HIGH);
        pca.setPWM(LCD_R, 0, 0);
        pca.setPWM(LCD_G, 0, 1024);
        pca.setPWM(LCD_B, 0, 0);
        HH = T / 100;
        MM = T % 100;
        if(HH < 10){
          lcd.setCursor(0, 0);
          lcd.print("0");
          lcd.print(HH);
          lcd.print(":");
        }
        else{
          lcd.setCursor(0, 0);
          lcd.print(HH);
          lcd.print(":");
        }
        if(MM < 10){
          lcd.print("0");
          lcd.print(MM);
        }
        else{
          lcd.print(MM);
        }
        lcd.print(" ");
        lcd.write(1);
        lcd.print(map(CT, 0, 511, 20, 85));
        lcd.write(0);
        lcd.print(" ");
        lcd.write(2);
        lcd.print(map(GT, 0, 511, 20, 85));
        lcd.write(0);
        lcd.print("      ");
        lcd.setCursor(0, 1);
        lcd.print("(^_^)v Good!    ");
        DFPlayer.stop();
        playing = 0;
      } else {
        pca.setPWM(0, 0, map(CL, 0, 511, 0, 4095));
        pca.setPWM(2, 0, map(CT, 0, 511, 0, 4095));
        pca.setPWM(4, 0, map(CP, 0, 511, 0, 4095));
        pca.setPWM(6, 0, map(GL, 0, 511, 0, 4095));
        pca.setPWM(8, 0, map(GT, 0, 511, 0, 4095));
        pca.setPWM(10, 0, map(GP, 0, 511, 0, 4095));
        pca.setPWM(12, 0, map(RL, 0, 511, 0, 4095));
        digitalWrite(LED_Y, HIGH);
        digitalWrite(LED_R, LOW);
        digitalWrite(LED_G, LOW);
        pca.setPWM(LCD_R, 0, 1024);
        pca.setPWM(LCD_G, 0, 1024);
        pca.setPWM(LCD_B, 0, 0);
        lcd.clear();
        HH = T / 100;
        MM = T % 100;
        if(HH < 10){
          lcd.setCursor(0, 0);
          lcd.print("0");
          lcd.print(HH);
          lcd.print(":");
        }
        else{
          lcd.setCursor(0, 0);
          lcd.print(HH);
          lcd.print(":");
        }
        if(MM < 10){
          lcd.print("0");
          lcd.print(MM);
        }
        else{
          lcd.print(MM);
        }
        lcd.print(" ");
        lcd.write(1);
        lcd.print(map(CT, 0, 511, 20, 85));
        lcd.write(0);
        lcd.print(" ");
        lcd.write(2);
        lcd.print(map(GT, 0, 511, 20, 85));
        lcd.write(0);
        lcd.print("      ");
        lcd.setCursor(0, 1);
        lcd.print("(x_x) Struggling");
        DFPlayer.stop();
        playing = 0;
      }
    } else {
      pca.setPWM(0, 0, map(CL, 0, 511, 0, 4095));
      pca.setPWM(2, 0, map(CT, 0, 511, 0, 4095));
      pca.setPWM(4, 0, map(CP, 0, 511, 0, 4095));
      pca.setPWM(6, 0, map(GL, 0, 511, 0, 4095));
      pca.setPWM(8, 0, map(GT, 0, 511, 0, 4095));
      pca.setPWM(10, 0, map(GP, 0, 511, 0, 4095));
      pca.setPWM(12, 0, map(RL, 0, 511, 0, 4095));
      digitalWrite(LED_Y, LOW);
      digitalWrite(LED_G, LOW);
      digitalWrite(LED_R, HIGH);
      pca.setPWM(LCD_R, 0, 1024);
      pca.setPWM(LCD_G, 0, 0);
      pca.setPWM(LCD_B, 0, 0);
      if (CTH == 0 && GTH != 0 && playing != 2) {
        DFPlayer.stop();
        DFPlayer.loop(2);
        playing = 2;
        lcd.setCursor(0,0);
        lcd.print("  !!High Temp!! ");
        lcd.setCursor(0,1);
        lcd.print("GPU ");
        lcd.print(GTH);
        lcd.write(0);
        lcd.print("      ");
      }
      if (CTH != 0 && GTH == 0 && playing != 3) {
        DFPlayer.stop();
        DFPlayer.loop(3);
        playing = 3;
        lcd.setCursor(0,0);
        lcd.print("  !!High Temp!! ");
        lcd.setCursor(0,1);
        lcd.print("CPU ");
        lcd.print(CTH);
        lcd.write(0);
        lcd.print("      ");
      }
      if (CTH != 0 && GTH != 0 && playing != 1) {
        DFPlayer.stop();
        DFPlayer.loop(1);
        playing = 1;
        lcd.setCursor(0,0);
        lcd.print("!!!High Temp!!! ");
        lcd.setCursor(0,1);
        lcd.print("CPU ");
        lcd.print(CTH);
        lcd.write(0);
        lcd.print(" GPU ");
        lcd.print(GTH);
        lcd.write(0);
        lcd.print("      ");
      }
    }
  }
}