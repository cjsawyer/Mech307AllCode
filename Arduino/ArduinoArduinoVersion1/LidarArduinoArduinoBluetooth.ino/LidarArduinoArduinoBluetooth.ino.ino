// LIDAR
unsigned long pulseWidth;
int triggerPin = 2;
int monitorPin = 3;

void setup() {
  
  // LIDAR
  Serial.begin(9600); // Start serial communications
  //115200
  pinMode(triggerPin, OUTPUT); // Set pin 2 as trigger pin
  pinMode(monitorPin, INPUT); // Set pin 3 as monitor pin
  
}

void loop(){

  PrintDistance();
  
}

void PrintDistance() {

  //writeString("TEST123");

  Serial.print('[');
  Serial.print(PollDistance());
  Serial.print(']');
  //writeString(tempOut);
  //Serial.write('D');
  
}

void writeString(String stringData) { // Used to serially push out a String with Serial.write()  
  for (int i = 0; i < stringData.length(); i++)
  {
    Serial.print(stringData[i]);   // Push each char 1 by 1 on each loop pass
  }

}// end writeString

int PollDistance() {

  
  digitalWrite(triggerPin, LOW); // Set trigger LOW for read
  
  pulseWidth = pulseIn(monitorPin, HIGH); // Count how long the pulse is high in microseconds
  pulseWidth = pulseWidth / 10; // 10usec = 1 cm of distance

  digitalWrite(triggerPin, HIGH); // Set trigger LOW to stop polling
  //Serial.end();

  return pulseWidth;

}

