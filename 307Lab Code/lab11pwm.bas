'pwm.bas

#config
    __CONFIG _CONFIG1, _INTRC_IO & _PWRTE_ON & _MCLR_OFF & _LVP_OFF
#ENDCONFIG

'Identify and set internal oscillator clock speed
Define OSC 8
OSCCON.4=1
OSCCON.5=1
OSCCON.6=1

'Turn off the A/D converter
ANSEL=0

'Define Pin assignments, variables, and constants

led0		Var	PortB.0		'LSB (bit 0) green LED
led1		Var	PortB.1		'bit 1 red LED
led2		Var	PortB.2		'bit 2 red LED
led3		Var	PortB.3		'MSB (bit 3) red LED

motor		Var	PortA.1		'PWM output pin to motor MOSFET gate
change		Var	PortA.3		'button causing motor to stop for speed adjustment

speed		Var	BYTE		'user input speed
Max_Speed	Con	15		'Max relative speed
T		Var	Word		'Pulse Period in milliseconds
t_on		Var	Word		'Pulse width high state
T_t		Var 	Word		'Pulse down (low state) time:(T-t)

pot_pin		Var	PortA.2		'Keypad pin for POT command
SCALE		Con	255		'POT statement scale factor
pot_val		Var	BYTE		'Value returned by POT command

'Initialize the I/O Pins
TRISA=%11101				'Designate PORTA Pins as inputs and output (RA1)
TRISB=%00000000				'Designate PORTB Pins as outputs
PORTB=0

Low motor				'make motor off indefinetely
'Initialize the speed display information
T=30000					'Pulse period microseconds
speed=7					'select medium speed
PORTB=speed				'display speed as binary number on LED's

top:
gosub myloop
goto new 
'Main Loop
myloop:

					'Endless speed change loop ( until exit with * key)
	Do While(1) 			'1:true
					
      	     POT pot_pin,SCALE,pot_val	'Read the keypad resistance
    
    	'Checks 1-key to increase speed
    		If((pot_val>37) && (pot_val<114) &&(speed<Max_Speed)) Then
    		speed=speed+1
    		PORTB=speed
    		Pause 300
     
    	'Checks the 4-key to decrease the speed
    		ElseIf((pot_val>114) && (pot_val<192) && (speed>0)) Then
        		speed=speed-1
        		PORTB=speed
                Pause 300
    	'Check for the * key to start motor motion
    		ElseIf(pot_val>192) Then
    		return      
    		
       		Endif
    		
	Loop

 new:
'Turn off LED
PORTB=0

'Initialize the pulse information
t_on = T/5 / max_speed * speed + T/20*3			'Duty Cycle Range= 15% to 35%
T_t=T-t_on

'Run the PWM until the user presses the stop button
Do While (change == 0)
	High Motor
	Pauseus t_on
	Low Motor
	Pauseus T_t
Loop

'Turn the LED speed display back on
PORTB=speed

Goto top

End		'End never reached
