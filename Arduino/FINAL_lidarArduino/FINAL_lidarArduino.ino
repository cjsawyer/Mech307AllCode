/*
 * Copyright Chris Sawyer, 2016
 * Licence: The following original code can be freely used for educational purposes, any other use is forbiden.
 *          Referenced libraries are covered under their original licences.
 */

#include <SoftwareSerial.h>

#define ArduinoBAUD 115200

//// PINS ///////////////////////////////////////////////
#define triggerPin 2
#define monitorPin 3
#define rxMainArduino 0
#define txMainArduino 1
#define stepper555Power 13
#define stepperStep 12
/////////////////////////////////////////////////////////

// LIDAR
unsigned long pulseWidth = 0;
float RPS = 2.2;

long loopStartTime = 0;
long loopEndTime = 0;

unsigned long distance = 0;
long degreesSwept = 0;

String toPrint = "[";

void setup() {
  Serial.begin(ArduinoBAUD);
  
  pinMode(triggerPin, OUTPUT);
  digitalWrite(triggerPin, LOW);
  pinMode(monitorPin, INPUT);
  
  pinMode(stepper555Power, OUTPUT);
  pinMode(stepperStep, OUTPUT);
  digitalWrite(stepper555Power, HIGH); //Spin the motor
  digitalWrite(stepperStep, LOW);
  
//  delay(1000); // let the other arduino boot
}
void loop() {

  //digitalWrite(stepperStep, HIGH);
  //delay(10);
  //digitalWrite(stepperStep, LOW);
  
  distance = GetDistance();
  
  
  loopEndTime = millis();
  degreesSwept = RPS * 360 *(loopEndTime-loopStartTime)/1000.0;
  //degreesSwept = 39;
  loopStartTime = millis();
  //degreesSwept = 9;
  
  
  // rot=360 deg
  // min = 60 sec
  // ms = sec/1000

    // already has "["
  toPrint += "|";
  toPrint += degreesSwept;  
  toPrint += ",";
  toPrint += distance;

  
  delay(50);
  CheckAskedForData(); // to possibly send      
  //delay(100);// possibly slow down readings
  
  
}

unsigned long GetDistance() {

  pulseWidth = pulseIn(monitorPin, HIGH); // Count how long the pulse is high in microseconds

  pulseWidth = pulseWidth / 10; // 10usec = 1 cm of distance
  return pulseWidth; // Print the distance
}

void CheckAskedForData() {
  if (Serial.available() > 0) {
    if ( ((char)Serial.read()) == 'a' ) {
      
      ////// Clear the serial buffer
      while(Serial.available()){  //is there anything to read?
       char getData = Serial.read();  //if yes, read it
      }   // don't do anything with it.
      //////
      
      Serial.println(toPrint);
      toPrint = "[";  
    }
    
  }
}

