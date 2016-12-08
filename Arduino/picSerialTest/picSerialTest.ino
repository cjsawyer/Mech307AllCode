#include <SoftwareSerial.h>

long BluetoothBAUD = 115200;
long PicBAUD = 9600;
SoftwareSerial mySerial(3, 2); // RX, TX
SoftwareSerial picSerial(12, 13); // RX, TX
// only one of these can run a at a time

void setup() {
  //picSerial.begin(9600);
  //mySerial.begin(115200);
}

char lastRead, lastLastRead;
String serialBuffer;
bool gotFirstBracket, gotEndBracket;

int dumbTest = 400;

void loop() {
  // [255,1[255,1[255,1[255,1[255,1
  serialBuffer = "";
  gotFirstBracket = false;
  gotEndBracket = false;
  
  picSerial.begin(PicBAUD);
  
  // wait until we read a bracket, signifiying the start of a packet
  while (!gotFirstBracket) {
     if (picSerial.available()) {
        lastRead = (char) picSerial.read();
        if (lastRead == '[') {
          gotFirstBracket = true;
        }
     }
  }

  serialBuffer = "[";

  // read until we read another bracket, signifiying the end of the packet
  while (!gotEndBracket) {
     if (picSerial.available()) {
        lastRead = (char) picSerial.read();
        if (lastRead == '[') {
          gotEndBracket = true;
        } else {
          // Don't want to add an extra '[' on the end of the packet. That's the start of the next one!
          serialBuffer += lastRead;
        }
     }
  }

  // send one packet to bluetooth
  picSerial.end();
  mySerial.begin(BluetoothBAUD);

  // Start packet
  mySerial.print("_-[|");
  
  // generate and send random LIDAR information
  mySerial.print(5);// angle random(10,30));
  mySerial.print(",");
  mySerial.print(dumbTest--);//random(20, 400));

  if (dumbTest < 0)
    dumbTest = 400;
  
  mySerial.print(serialBuffer);
  mySerial.println("[2[45,200+");
  mySerial.end();

  //delay(1);
  
  
  //mySerial.print("_-[|16,120|30,100|25,160[");
  //mySerial.print(random255);
  //mySerial.println(",1[2[45,200+");
    
  
}



void pooploop() {
  long random255 = random(255);
  
  mySerial.print("_-[|16,120|30,100|25,160[");
  mySerial.print(random255);
  mySerial.println(",1[2[45,200+");
  delay(100);
}
