#SALAD CHEF 

  A Couch co-op, programmer art, top down static screen Salad chef simulation in Unity3D 2018.4.13f1

  Game Features:
  
    - 2 Player, keyboard only gameplay P1 - A,S,D,W and SpaceBar, P2 - NumPad-4,5,6,8 and EnterKey
    - Vegetable types and spawn driven by a VegetableConfig scriptable object
    - Chopping board functioality with player pause while using chopping board and transfer to plate after chopping
    - Trash can functioality to dispose vegetable and salad
    - Customer with wait timer, randomized salad ingredients request and satisfied, ignored and angry logic.
    - 3 randomized powerup spawn based on customer satisfaction (Speed, Score, Time)
    - Scoring system based on whether customer is satisfied/Angry/Ignored
    - HUD for different players and GameEnd screen
    - Highscore save for top 10 scores
    
  Areas that can be Improved:
  
    - The Gamemanager currenlty handles most of the UI flow, this can be seperated
    - The powerup classes can have their own unique functionalities
    - The HighscoreManager can be made better
    
  
  Future Plans:
  
    - The game can feature an actual co-op mode where the players work together than competing each other
    - Different levels with varied kitchen layout.
    - Can add a level creation by player as well.
    - More features like Cooking, Salad spill/overcooking cleanup, Plate cleaning etc., could be added
    - Should add a visual indicator whenever a trigger is activated by the player
    - Background sounds and SFX
