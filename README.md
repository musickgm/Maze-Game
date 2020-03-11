# Maze-Game
Geoff Musick
CPSC 6820
1/31/20
Assignment 2: Maze

FUNCTIONALITY:
I created a hedge maze where the player has to reach the center collectable to win. The controls are displayed
in the top left corner. The player can move directionally with the WADS keys and change the direction they are
looking with the mouse. The player can also increase speed with a sprint (shift) button.
The score is displayed in the top right. The score starts at 200 (or some assigned value) and decreases over time. 
On the way to the destination, 3 types of collectables (other than the objective) can be found:
1) Yellow coins - add to the player's score.
2) Semi-transparent blue coins - make the walls invisible for a certain amount of time.
3) Big red coins - displays a mini-map for a certain amount of time.
The 2nd and 3rd "power-up" type coins also show the amount of time remaining on the powerups. At the beginning,
a canvas displays the objective (which fades away). At the end, a message displays the end of the game and prompts
the user to restart (r key). A player can quit at any time with the escape key. Sound fx trigger when collectables
are found. Additionally background music plays and loops throughout the game. 


IMPLEMENTATION:
For core game functions, I implemented a game manager that does the following:
-Communicates with collectables, sfx
-Handles starts/end game logic
-Controls UI popups
-Additional key inputs (R, escape)
For movement, a player controller was implemented that takes in player input and translates/rotates the player
accordingly. Additionally a clamp function is used to prevent unintentional rotation and other bugs.
For spring camera implementation, see below. 
The main problems I encountered dealt with wall clipping, unintentional player rotation, and the player being 
able to climb walls with the sprint feature. I alleviated the wall climbing and the player rotation by 
implmenting the clamp function for the PlayerController. Taking off colliders for the player's arms also
removed the rotation problem. See wall clipping solution below. 
Another problem I ran into was jerky motion when a player ran into a wall. I fixed this by checking for a 
raycast and removing the sprint if the player was right in front of a wall. 
For the minimap, I implemented a second camera (orthographic). For better visualization, I parented spheres
above each collectable that are on their own layer that is excluded from the main camera. A gian cube
above the player represents the player on the minimap as well. 



FOLLOW-CAMERA
For the spring camera, I implemented a CameraFollow script. This script works by setting a goal position and
rotation that the camera "desires" to be at using an offset from the player. This goal position and rotation
is lerped to using a spring constant for both rotation and position. Additionally, a linecast is used to avoid
wall clipping as much as possible. I addapted the idea for the linecast from reddit: 
https://www.reddit.com/r/Unity3D/comments/cfzv5r/made_a_thirdperson_camera_collision_adjustment/eudmi61/
The idea behind the linecast is to check if there is a wall between where the camera is and its goal position.
I adapted the reddit script to only do this if there are walls (since coins were causing a "lag" effect when
the player walked past them). Additionally, I edited the linecast to put the camera as close to the wall as 
possible by weighting an average 9:1 in favor of the way compared to the goal position. For the most part
this solution is effective. However, some wall clipping does occur in extreme cases. 



SOURCES:
-Pebbles, granite, and stone wall .png file from https://www.wildtextures.com/category/options/seamless/
-Grass/Hedge .png file from https://www.tonytextures.com/tileable-hedge-textures-free-download-3d-rendering/
-Singleton.cs copied from a previous project of mine. Not sure what the original source was. 
-SFX and background audio was from Phys 1 (a game I developed) - I have rights to the music which was made
by August Pappas, a friend of mine. 
