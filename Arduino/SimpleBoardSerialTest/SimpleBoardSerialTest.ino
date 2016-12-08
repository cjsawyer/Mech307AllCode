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
  serialBuffer = "";
  picSerial.begin(PicBAUD);
   if (picSerial.available()) {
      lastRead = (char) picSerial.read();
      picSerial.end();
      
      mySerial.begin(BluetoothBAUD);
      mySerial.println(lastRead);
      mySerial.end();
   }
}



void pooploop() {
  long random255 = random(255);
  
  mySerial.print("_-[|16,120|30,100|25,160[");
  mySerial.print(random255);
  mySerial.println(",1[2[45,200+");
  delay(100);
}
