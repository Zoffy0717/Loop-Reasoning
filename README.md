# Loop-Reasoning
Game title: Loop Reasoning
Team Name: Baka Baka
Team member:
Hankun Liu, Designer in class
Nathan Liu, Programmer in class
Xiyue Feng, Writer 
Weiyi Kong, Artist 
Yingwei Huang, Artist 

Control:
	ad - movement
	e - interact
	f - open/close reasoning board
	c/esc - close card pop up

	In reasoning board:
	click and drag each card to the slot
	the card that can synthesis will highlight in yellow
	When two correct cards are placed in the slot, a new card is formed.
	Similarly for testimony board, except the number of cards require change from 2 to 3.


Milestone 4:
Major bugs/concerns: In testimony reasoning board(the one with 3 slots) the highlight when a card is drag is not working. 
	There can be too many card in hand. We are thinking maybe changed cards in hand from displaying all cards to only showing the new cards from last time. 

	When you accuse the wrong suspect, the game is supposed to move the player back to day 1 investigation, but it seems like certain functions are not working due to the scene being reloaded. When confirm with a wrong suspect, nothing happens.

	we may need to add more dialogues.

credits:
	Weiyi Kong, Artist and Yingwei Huang, Artist : all of the art assets in the game
	Nathan Liu: all of the scripts and putting the game together in Unity
	Xiyue Feng: The story and card information
	Hankun Liu: All of the design documents, arrange meetings with the group/mentor.

milestone 2:
Goal：To test whether the current gameplay idea is fun and feasible, and to gain feedback from the playtest to update and improve the gameplay. To test whether the synthesis system in the reasoning scene can work fluently and effectively.

Know Major Bugs:

When cards pop up, do not click c, press e enter the dialogue again, then press c to close the pop up, when dialogue ends, time.Scale time will not change back to 1. – Fixed
	
Not a bug, but a suggestion – right now, when a player is in conversation with an NPC, if the player accidentally presses F they will open the reasoning board. Playtesters suggest to have only one action available.(i.e if in conversation, then reasoning board can not be open)
	
Another suggestion: Bigger font for the hint, maybe reposition it as well. Some players are missing the hint.

Big questions you have about your game. What issues keep you up at night?

	Our artists were busy with their finals, so they did not have time to work on our project until later in the quarter, so the artwork for this game may be running a little behind. We will have a lot of cards, right now, we already have 79 cards and we are not done yet. The programmer will need to write a new method to display the cards in hand rather than the one we have in the demo, which displays all the cards the player has collected. As for narrative, the challenges lies in how to separate the story into different phases(to unlock the next phase, the player needs to combine and obtain certain cards) while keeping the story attractive, maintaining suspense at each stage. 

 

