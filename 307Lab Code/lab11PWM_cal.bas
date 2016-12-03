'pwm_cal.bas

' Displays the pot values for the keypad buttons by blinking the upper three red LED's. 
' Each LED is blinked individually to indicate the number of 100s, 10s, and 1s in the pot value number.
' The Green LED is flashed once between each blinking red LED display

#CONFIG
    __CONFIG _CONFIG1, _INTRC_IO & _PWRTE_ON & _MCLR_OFF & _LVP_OFF
#ENDCONFIG

'Identify and set the internal oscillator clock speed 
Define OSC 8
OSCCON.4=1
OSCCON.5=1
OSCCON.6=1

'Turn off the A/D Converter
ANSEL=0

'Define variables, pin assignments, and constants
led0		Var	PortB.0		'LSB  (bit 0) LED
led1		Var	PortB.1		'bit 1 LED
led2		Var	PortB.2		'bit 2 LED
led3		Var	PortB.3		'bit 3 LED
motor		Var	PortA.1		'PWM output pin to motor MOSFET Gate
pot_pin		Var	PortA.2		'keypad pin for pot command
Scale		Con	255			'pot statement scale factor 
pot_val		Var	BYTE		'value returned by Pot command
i			Var BYTE		'loop variable
digs		Var	BYTE		'digit number for each decimal place

'Initialize the I/O Pins
TRISA= %11101		'Desginate PortA pins as inputs and outputs (RA1)
TRISB= %00000000	'Designate PortB pins as outputs
PortB=0			
Low Motor

'User speed change loop
enter:
	Pot pot_pin, Scale, pot_val

	'Flash the LSB green and blink each of the upper 3 red LEDs to indicate the number of  100s, 10s, and 1's in pot value. 
	
	PortB=0
	
	High LED0
	Pause 500
	Pause 100
	digs= pot_val/100
	For i=1 To digs
		High led3
		Pause 300
		Low led3
		Pause 300
	Next i

	pot_val=pot_val - digs*100
	High led0
	pause 500
	low led0
	pause 100
	digs = pot_val /10
	For i=1 To digs
		High led2
		Pause 300	
		Low Led2
		Pause 300
	
	Next i

	digs= pot_val - digs*10
	High Led0
	Pause 500
	Low Led0
	Pause 100
	For i=1 To digs
		High led1
		Pause 300
		Low led1
		Pause 300
	Next i
	Goto enter

	'End of Program never reached
	End 
