#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>
#include <DFRobotDFPlayerMini.h>
#include <SoftwareSerial.h>
#include <LedControl.h>
#include <SPI.h>

#define LED_R 8
#define LED_Y 7
#define LED_G 4

DFRobotDFPlayerMini DFPlayer;
Adafruit_PWMServoDriver pca = Adafruit_PWMServoDriver(0x40);
LedControl m7 = LedControl(11, 13, 9, 4);

unsigned long lastResponseTime = 0;
bool handshaking = true;
bool connectedornot = false;
int playing = 0;

int T = 0, CL = 0, CT = 0, CP = 0, GL = 0, GT = 0, GP = 0, RL = 0, CTH = 0, GTH = 0;
String dataBuffer = "";
String cmdBuffer = "";
int CLHContinue = 0, GLHContinue = 0;

void test() {
  for (int i = 0; i < 16; i++) {
    pca.setPWM(i, 0, 4095);
  }
  digitalWrite(LED_G, HIGH);
  digitalWrite(LED_Y, HIGH);
  digitalWrite(LED_R, HIGH);
  delay(500);
  for (int i = 0; i < 16; i++) {
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
  Serial1.begin(9600);
  Serial.begin(9600);
  SPI.begin();
  pinMode(LED_G, OUTPUT);
  pinMode(LED_Y, OUTPUT);
  pinMode(LED_R, OUTPUT);
  digitalWrite(LED_G, LOW);
  digitalWrite(LED_Y, LOW);
  digitalWrite(LED_R, LOW);
  pca.begin();
  pca.setPWMFreq(1000);
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
      if (CTH == 0 && GTH != 0 && playing != 2) {
        DFPlayer.stop();
        DFPlayer.loop(2);
        playing = 2;
      }
      if (CTH != 0 && GTH == 0 && playing != 3) {
        DFPlayer.stop();
        DFPlayer.loop(3);
        playing = 3;
      }
      if (CTH != 0 && GTH != 0 && playing != 1) {
        DFPlayer.stop();
        DFPlayer.loop(1);
        playing = 1;
      }
    }
  }
}
