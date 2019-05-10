README

GITHUB ISSUE TRACKING: https://github.com/BerejikB/AiDemo/issues

INCLUSIONS:
Included is C# code I have along with some free assets from the Unity Asset store included to showcase the behaviour

REQUIREMENTS:
CURRENT:
•	AI follows player in different stages
	Stage 1) slow stalk, if the AI sees the player at a certain distance, it will slowly stalk the player. If player moves out of range it will return to it's wander phase. Can be scared off at this point
	
	Stage 2) After AI has stalked player and is within the stage 2 range, it will begin a faster walk. Can still be scared off at this point
	
	Stage 3) After the AI has closed the distance, it will charge and **TODO attack** the player. Can not be scared off at this stage.
	
	"Spooked" Stage) If the player comes within a certain range of the AI without it "visually" detecting the player, it will get "spooked" and run away in the opposite direction untill it meets the 'maxDistance" (which is also its max visual detect range). After it reaches the specified distance, it will stalk the player, regardless of if the player runs out of range, and begin stage 2 and 3 normally.
 

TO DO: AI should freeze in place if the player looks at it when stalking during stage 1.
		AI should stop walking toward the player normalle, and move randomly toward when the player looks at it during stage 2
		AI should be scared off by **TODO damage** and **TODO fire**
		After the AI is scared off, it should enter the "Spooked" stage

LONG TERM GOALS:		
	Add custom models/sounds/textures eventually. 
	
	Possibly turn into a stand-alone game. 
		

LICENCE
This software and all related documentation is provided under the GNU GPLv3 License
