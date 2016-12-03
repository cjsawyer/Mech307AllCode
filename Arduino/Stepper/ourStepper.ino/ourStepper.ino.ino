/********************************************************
**         More info about the project at:             **
**  http://lusorobotica.com/index.php?topic=106.0  **
**   by TigPT         at         [url=http://www.LusoRobotica.com]www.LusoRobotica.com[/url]  **
*********************************************************/
int dirPin = 2;
int stepperPin = 3;

void setup() {
  pinMode(dirPin, OUTPUT);
  pinMode(stepperPin, OUTPUT);
}

void step(boolean dir,int steps, int delayTime){
  digitalWrite(dirPin,dir);
  delay(50);
  for(int i=0;i<steps;i++){
    digitalWrite(stepperPin, HIGH);
    delayMicroseconds(delayTime);
    digitalWrite(stepperPin, LOW);
    delayMicroseconds(delayTime);
  }
}

void loop(){

  /*
  step(true, 5000, 46);
  delay(100);

  step(false, 5000, 46);
  delay(100);
  //*/
  MusicSweep();
}


void MusicSweep() {
  for(int i=1; i<24; i++) {
    step(true, 750, i);
  }
  delay(1);  
}

