#include <SoftwareSerial.h>

#define BluetoothBAUD 115200
#define ArduinoBAUD 115200
#define PicBAUD 9600

//// PINS ///////////////////////////////////////////////
#define rxBluetooth 12
#define txBluetooth 13
#define rxLightsPic 11
#define txLightsPic 10 // no connection, just for SoftwareSerial.h
#define rxThrottlePic 9
#define txThrottlePic 8 // no connection, just for SoftwareSerial.h
#define rxLidarArduino 7
#define txLidarArduino 6
#define rxSpeedCadenceSensor 0
// All of these are SoftwareSerial except for the cadence, which is either an interrupt or hardware serial depending on
//    if interrupts are too slow and break softwareserial
// interrupt: use pins 2 or 3
// serial: use  RX pin 0
// https://www.arduino.cc/en/Reference/AttachInterrupt
/////////////////////////////////////////////////////////

// only one of these can run a at a time!
SoftwareSerial bluetoothSerial(rxBluetooth, txBluetooth); //RX, TX
SoftwareSerial lidarArduinoSerial(rxLidarArduino, txLidarArduino);
SoftwareSerial lightPicSerial(rxLightsPic, txLightsPic);
SoftwareSerial throttlePicSerial(rxThrottlePic, txThrottlePic);


char lastRead, lastLastRead;
String serialBuffer;
bool gotFirstBracket, gotEndBracket;
int dummyLidarDistance = 400;
String strReturn;


void setup() {
  bluetoothSerial.begin(BluetoothBAUD);
}

void loop() {
  
  //'serpator start lidar lidar lidar throttle lights cadence end
  //'_-[|16,120|30,100|25,160[100,1[2[45,200+

  ///////////////////////////////////////////////////////////
  // Gather all information and send over bluetooth serial
  ///////////////////////////////////////////////////////////

  // Start packet
  bluetoothSerial.print("_-");

  // Generate and send dummy LIDAR information: "[|angleChange,distance|angleChange,distange....."
  bluetoothSerial.print("[|");
  bluetoothSerial.print(5);
  bluetoothSerial.print(",");
  bluetoothSerial.print(dummyLidarDistance--);
  if (dummyLidarDistance < 0)
    dummyLidarDistance = 400;
    
  // Send throttle: "[255Position,STATE"
  strReturn = WaitForPicPacket(throttlePicSerial);
  //strReturn = "[100,1"; 
  bluetoothSerial.print(strReturn);
  
  // Send lights: "[STATE"
  strReturn = WaitForPicPacket(lightPicSerial);
  //strReturn = "[1";
  bluetoothSerial.print(strReturn);
  
  // Send cadence: "[numberWheelRevs,timeMsSinceLastMeasurement"
  bluetoothSerial.print("[3,200");

  // End packet
  bluetoothSerial.println("+");
  bluetoothSerial.end();

  
  
}







String WaitForPicPacket(SoftwareSerial serialConnection) {

  serialBuffer = "";
  gotFirstBracket = false;
  gotEndBracket = false;
  
  bluetoothSerial.end();
  serialConnection.begin(PicBAUD);
  
  // wait until we read a bracket, signifiying the start of a packet
  while (!gotFirstBracket) {
     if (serialConnection.available()) {
        lastRead = (char) serialConnection.read();
        if (lastRead == '[') {
          gotFirstBracket = true;
        }
     }
  }
  
  serialBuffer = "[";
  
  // read until we read another bracket, signifiying the start of the next packet and the end of this packet
  while (!gotEndBracket) {
     if (serialConnection.available()) {
        lastRead = (char) serialConnection.read();
        if (lastRead == '[') {
          gotEndBracket = true;
        } else {
          // Don't want to add an extra '[' on the end of the packet. That's the start of the next one!
          serialBuffer += lastRead;
        }
     }
  }

  serialConnection.end();
  bluetoothSerial.begin(BluetoothBAUD);

  return serialBuffer;
}
