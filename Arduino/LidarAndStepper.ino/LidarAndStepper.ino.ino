// Stepper
int dirPin = 2;
int stepperPin = 3;

// LIDAR
unsigned long pulseWidth;
int triggerPin = 4; //2
int monitorPin = 5; //3

void setup() {
  
  // Stepper
  pinMode(dirPin, OUTPUT);
  pinMode(stepperPin, OUTPUT);


  // LIDAR
  Serial.begin(115200); // Start serial communications

  pinMode(triggerPin, OUTPUT); // Set pin 2 as trigger pin
  digitalWrite(triggerPin, LOW); // Set trigger LOW for continuous read

  pinMode(monitorPin, INPUT); // Set pin 3 as monitor pin
  
}

void step(boolean dir,int steps, int delayTime){

  // Stepper
  digitalWrite(dirPin,dir);
  //delay(50);
  for(int i=0;i<steps;i++){
    digitalWrite(stepperPin, HIGH);
    delayMicroseconds(delayTime);
    digitalWrite(stepperPin, LOW);
    delayMicroseconds(delayTime);
  }
}

void loop(){
//311 RPM
//StepOnce();
step(true, 1000, 65);
//delay(100);

//PrintDistance();

  /*
  step(true, 5000, 46);
  delay(100);

  step(false, 5000, 46);
  delay(100);
  //*/
  //MusicSweep();
}

void StepOnce() {
  step(true, 2, 50); //46 MINIMUM
}

void PrintDistance() {
  pulseWidth = pulseIn(monitorPin, HIGH); // Count how long the pulse is high in microseconds

  // If we get a reading that isn't zero, let's print it
  if(pulseWidth != 0)
  {
    pulseWidth = pulseWidth / 10; // 10usec = 1 cm of distance
    Serial.println(pulseWidth); // Print the distance
  }
}

void MusicSweep() {
  for(int i=1; i<24; i++) {
    step(true, 750, i);
  }
  delay(1);  
}

