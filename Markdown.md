#include <Streaming.h>
#include "player.h"
// -- OLED -------
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <TM1638plus.h>
// OLED i2c
#define OLED_RESET -1 // This is code from the CMP101 Week One and Two Labs
#define OLED_SCREEN_I2C_ADDRESS 0x3C
#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels
#define STROBE_TM D5 // strobe = GPIO connected to strobe line of module
#define CLOCK_TM D6 // clock = GPIO connected to clock line of module
#define DIO_TM D7 // data = GPIO connected to data line of module
bool high_freq = false; //default false, If using a high freq CPU > ~100 MHZ set to true.
// Constructor object (GPIO STB , GPIO CLOCK , GPIO DIO, use high freq MCU)
TM1638plus tm(STROBE_TM, CLOCK_TM , DIO_TM, high_freq);
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT);

 Player playerOne; // Create objects playerOne and playerTwo of class Player
 Player playerTwo;

void setup()
{
  tm.displayBegin();

 display.begin(SSD1306_SWITCHCAPVCC, OLED_SCREEN_I2C_ADDRESS);
 display.display();
 Serial.begin(9600);
 delay(2000);
 display.setCursor(0,0);
 display.setTextSize(1); // - a line is 21 chars in this size
 display.setTextColor(WHITE);
 display.clearDisplay();
 display.display();  // This is where the code from the CMP101 Week One and Two Labs ends
 
}
void loop()
{
 while (playerOne.getScore() < 10){ // While player ones score is below 10 run the game

  display << "Player One Score is: " << playerOne.getScore() << endl  << "Attempts: " << playerOne.getAttempts() << endl << endl << endl; // display score and attempts for both players on the LED screen
  display << "Player Two Score is: " << playerTwo.getScore() << endl << "Attempts: " << playerTwo.getAttempts();
  display.setCursor(0,0);
  display.display();
  display.clearDisplay();

  uint8_t gameDecider = random(1,3); // generate a random to determine which game to play every turn
  if (gameDecider == 2){ 
      tm.reset();
    tm.displayText("Turn It!"); // display turn it on the 7 segment display
   playerOne.twistIt(); // call twist it function 
    tm.reset();
    tm.displayText("");
    delay(500);
    tm.reset();
  
  }
  else if (gameDecider == 1){
    tm.reset();
    tm.displayText("Bop It!"); // display bop it on the 7 segment display
    uint8_t random = rand() % 8; // generate rand for use in function
    Serial.print("Random is");
    Serial.print(random); // print the random value for checking if it aligns
    tm.setLED(random, 1); // set the LED to the button you are supposed to push
    delay(4000);
    playerOne.bopIt(random); // call bop it function with random
    tm.reset();
    tm.displayText("");
    delay(500);
    tm.reset();

    }
 
  }

   while (playerTwo.getScore() < 10){ // While player twos score is below 10

  display << "Player One Score is: " << playerOne.getScore() << endl  << "Attempts: " << playerOne.getAttempts() << endl << endl << endl; // Display player one and twos score and attempts to the LED screen
  display << "Player Two Score is: " << playerTwo.getScore() << endl << "Attempts: " << playerTwo.getAttempts();
  display.setCursor(0,0);
  display.display();
  display.clearDisplay();

  uint8_t gameDecider = random(1,3);
  if (gameDecider == 2){
      tm.reset();
    tm.displayText("Turn It!"); // display Turn it on the 7 segment display
  playerTwo.twistIt(); // call twist it function
    tm.reset();
    tm.displayText("");
    delay(500);
    tm.reset();
  }
  else if (gameDecider == 1){
    tm.reset();
    tm.displayText("Bop It!"); // display bop it on the 7 segment display
    uint8_t random = rand() % 8; // random for use in function
    Serial.print("Random is");
    Serial.print(random); // print the random value for checking if it aligns 
    tm.setLED(random, 1); // set the LED corresponding to the button you are supposed to push
    delay(4000);
    playerTwo.bopIt(random); // call bop it function with random
    tm.reset();
    tm.displayText("");
    delay(500);
    tm.reset();
    }
  }
    for (int i = 0; i < 1; i++){
      if (playerOne.getAttempts() < playerTwo.getAttempts()){ // if player one has less attempts than player two
        display << "Player One has beaten Player Two!" << endl; // display that player one has won on the LED screen
        display.display();
        while (true){

        }
      }
      if (playerTwo.getAttempts() < playerOne.getAttempts()){ // if player two has less attempts than player one
        display << "Player Two has beaten Player One!" << endl; // display that player two has won on the LED screen
        display.display();
        while (true){

        }
      }
      if (playerOne.getAttempts() == playerTwo.getAttempts()){ // if players attempts are equal
        display << "The game was a tie." << endl; // display that the game was tied on the LED screen
        display.display();
        while (true){
        }
      }
    }
  }






Player.cpp:







#include "player.h"
#include <Streaming.h>
// -- OLED -------
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <TM1638plus.h>
// OLED i2c
#define OLED_RESET -1 //This is code from CMP101 Labs One and Two
#define OLED_SCREEN_I2C_ADDRESS 0x3C
#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels
#define STROBE_TM D5 // strobe = GPIO connected to strobe line of module
#define CLOCK_TM D6 // clock = GPIO connected to clock line of module
#define DIO_TM D7 // data = GPIO connected to data line of module

bool highfreq = false; //default false, If using a high freq CPU > ~100 MHZ set to true.
// Constructor object (GPIO STB , GPIO CLOCK , GPIO DIO, use high freq MCU)
TM1638plus tm1(STROBE_TM, CLOCK_TM , DIO_TM, highfreq); // This is where the code from CMP101 Labs One and Two ends

byte buttons;

int Player::getScore(){ // Function to get score as it is a private variable in the player class
  return score;
}

int Player::getAttempts(){ // Function to get attempts as it is a private variable in the player class
  return attempts;
}

void Player::bopIt(int rand){ // Function for Bop It part of the game
    buttons = tm1.readButtons(); // read input of buttons being pressed
    switch (rand){
      case 0: if (buttons == 1){ // if rand aligns with the input for buttons being pressed
        score += 1;
      }
      case 1: if (buttons == 2){ // if rand aligns with the input for buttons being pressed
        score += 1;
      }
      case 2: if (buttons == 4){ // if rand aligns with the input for buttons being pressed
        score += 1;
      }
      case 3: if (buttons == 8){ // if rand aligns with the input for buttons being pressed
        score += 1;
      }
      case 4: if (buttons == 16){ // if rand aligns with the input for buttons being pressed
        score += 1;
      }
      case 5: if (buttons == 32){ // if rand aligns with the input for buttons being pressed
        score += 1;
      }
      case 6: if (buttons == 64){ // if rand aligns with the input for buttons being pressed
        score += 1;
      }
      case 7: if (buttons == 128){ // if rand aligns with the input for buttons being pressed
        score += 1;
      }
      default: ;
    }
    attempts += 1;
}

void Player::twistIt(){ // Function for Twist It part of the game
    int oldSensorValue = analogRead(A0); // read the analogs value
    delay(4000);
    int sensorValue = analogRead(A0); // reread the analogs value after the timer has passed
    if (sensorValue > oldSensorValue){ // if there is enough change between old analog value and new analog value
      if (sensorValue - oldSensorValue > 25){ // buffer to account for any analog read variance 
        score += 1;
      }
    }
    if (sensorValue < oldSensorValue){ // checking if there is enough change in the other direction as turning the analog either way works
      if (oldSensorValue - sensorValue > 25){ // buffer to account for any analog read variance
        score += 1;
      }
    }
    attempts += 1;
}









player.h








#include <iostream>
#pragma once

class Player{
  private:
  int score;
  int attempts;
  public:
  int getScore();
  int getAttempts();
  void bopIt(int);
  void twistIt();
};


