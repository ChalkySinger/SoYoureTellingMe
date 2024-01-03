#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>


//---------Analouge-Inputs---------//
Adafruit_MPU6050 mpu; // This is variable for the Gyro/Accel
#define VRX_PIN  A2 // Arduino pin connected to VRX pin
#define VRY_PIN  A1 // Arduino pin connected to VRY pin
#define POTENTIOMETER_PIN A0
//-------------------------------//


//---------Digital-Inputs---------//
#define LED1_PIN 10
#define LED2_PIN 11
#define LED3_PIN 6
#define LED4_PIN 3

#define MOTOR_PIN 4
//-------------------------------//

String sendData;


//---------Joystick-Inputs---------//
int joyXvalue = 0; // To store value of the joystick's X axis
int joyYvalue = 0; // To store value of the joystick's Y axis
//--------------------------------//

//---------Gyro-Inputs---------//
float gXValue = 0;
float gYValue = 0;
float gZValue = 0;
//--------------------------------//

int LED_AMOUNT = 5;

int potentValue;




void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);

  GyroSetup();


  pinMode(LED1_PIN, OUTPUT);        // LED 1  
  pinMode(LED2_PIN, OUTPUT);        // LED 2
  pinMode(LED3_PIN, OUTPUT);        // LED 3
  pinMode(LED4_PIN, OUTPUT);        // LED 4

  pinMode(MOTOR_PIN, OUTPUT);

  // // Turns off all the leds at first
  // digitalWrite(LED1_PIN, LOW); 
  // digitalWrite(LED2_PIN, LOW); 
  // digitalWrite(LED3_PIN, LOW); 
  // digitalWrite(LED4_PIN, LOW); 


}

void loop() {
  // put your main code here, to run repeatedly:
  
  LedsWithPotent();
  WriteSerial();

}

void WriteSerial()
{
  sensors_event_t a, g, temp;
	mpu.getEvent(&a, &g, &temp);

  // read analog X and Y analog values
  joyXvalue = analogRead(VRX_PIN);
  joyYvalue = analogRead(VRY_PIN);

  gXValue = g.gyro.x;
  gYValue = g.gyro.y;
  gZValue = g.gyro.z;

  sendData = String(potentValue) + "," + String(gXValue) + "," + String(gYValue) + "," + String(gYValue) + "," + String(joyXvalue) + "," + String(joyYvalue);  
  Serial.println(sendData);


}

void ReadSerial()
{
  // Here use chars to detect outputs from Unity

}


// Takes value from potent to leds to turn them on 
// No interaction between Unity and LEDs
void LedsWithPotent()
{
  potentValue = analogRead(POTENTIOMETER_PIN);
  int ledChoice = potentValue / (1024 / LED_AMOUNT);
  // Serial.print("Value: ");
  // Serial.println(ledChoice);

  if (ledChoice > LED_AMOUNT - 1)
  {
    ledChoice = LED_AMOUNT - 1;
  }

  // No LEDs are on
  if(ledChoice == 0)
  {
    digitalWrite(LED1_PIN, LOW);
    digitalWrite(LED2_PIN, LOW);
    digitalWrite(LED3_PIN, LOW);
    digitalWrite(LED4_PIN, LOW);

  }
  else if(ledChoice == 1)
  {
    digitalWrite(LED1_PIN, HIGH);
    digitalWrite(LED2_PIN, LOW);
    digitalWrite(LED3_PIN, LOW);
    digitalWrite(LED4_PIN, LOW);

  }
  else if(ledChoice == 2)
  {
    digitalWrite(LED1_PIN, HIGH);
    digitalWrite(LED2_PIN, HIGH);
    digitalWrite(LED3_PIN, LOW);
    digitalWrite(LED4_PIN, LOW);

  }
  else if(ledChoice == 3)
  {
    digitalWrite(LED1_PIN, HIGH);
    digitalWrite(LED2_PIN, HIGH);
    digitalWrite(LED3_PIN, HIGH);
    digitalWrite(LED4_PIN, LOW);

  }
  else if(ledChoice == 4)
  {
    digitalWrite(LED1_PIN, HIGH);
    digitalWrite(LED2_PIN, HIGH);
    digitalWrite(LED3_PIN, HIGH);
    digitalWrite(LED4_PIN, HIGH);

  }
 



}

void GyroSetup()
{
  // Try to initialize!
	if (!mpu.begin()) {
		// Serial.println("Failed to find MPU6050 chip"); // Commented out cause this will be sent to Unity
		while (1) {
		  delay(10);
		}
	}
	// Serial.println("MPU6050 Found!"); // Commented out cause this will be sent to Unity

	// set accelerometer range to +-8G
	mpu.setAccelerometerRange(MPU6050_RANGE_8_G);

	// set gyro range to +- 500 deg/s
	mpu.setGyroRange(MPU6050_RANGE_500_DEG);

	// set filter bandwidth to 21 Hz
	mpu.setFilterBandwidth(MPU6050_BAND_21_HZ);

	delay(100);
}

void GyroFunction()
{
  
}