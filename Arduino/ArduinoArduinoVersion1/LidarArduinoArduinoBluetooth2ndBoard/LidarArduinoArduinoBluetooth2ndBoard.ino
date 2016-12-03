#include <SoftwareSerial.h>
SoftwareSerial mySerial(4, 2); // RX, TX

byte val = ' '; // Temporarily stores each byte sent by of the other arduino
int incoming = 0;

void setup() {
  Serial.begin(9600); // opens serial port
                        // this is to read in from the other board

  mySerial.begin(9600);
}

void loop() {
  // send data to the bluetooth when there is data avalible from the RX port
  incoming = Serial.available();  
  while (incoming != 0)           //while there is something to be read
  {
    // Need to cap this loop by ending after 360 degrees have passed
    // Also need to parse this into a single string to send instead of this byte stream
    val = Serial.read();
    mySerial.begin(9600);
    mySerial.write(val);
    mySerial.end();
    incoming = Serial.available();
  }
    
  delay(1000);

  //mySerial.begin(9600);
  //mySerial.end();
  
  //mySerial.println("============");
  //Serial.println("");
}
