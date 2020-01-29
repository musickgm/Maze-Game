Geoff Musick
CPSC 6820
1/18/20
Assignment 1: Field Goal

FUNCTIONALITY:
Simple field goal game that contains 10 levels with various kicking positions. 
At the start of each level, the player can move an aiming object (red sphere) using arrows (awds) 
to control the x/y positions and cntrl/shift buttons to control the z position. The x/y position
of the aiming object will contribute to the direction the ball is kicked, while the z position will
factor into the thrust that is applied to the ball. When the player is satisfied with the position of
the aiming object, the ball is kicked by pressing "space". If the goal is made, 3 points are added
to the player's score. Once the ball returns to the ground, the next level begins. This continues
until all 10 levels are completed at which time the game is over. 


IMPLEMENTATION:
The implementation of this game utilizes 6 simple scripts:
1) AimingObject - takes in input from the player to move the aiming object. These positions are clamped. 
2) Football - awaits input from the kicker for when to apply the impulse. Resets every level. 
3) Kicker - takes input from the player and calls the AddImpulse function on the football with
appropriate parameters.
4) FieldGoal - simple trigger collider script for when the ball enters the goal.  
5) Floor - simple trigger collider script for when the ball returns to the ground. 
6) GameManager - organizes the levels, responds to trigger scripts, and organizes/resets the
AimingObject and Football. 
Objects in the game consist of a "KickingPosition" parent object that holds the camera, aiming object, and
football in proximity to one another. The football is a simple 3D capsule with a simple material. Likewise,
the aiming object is a simple sphere with a simple material. The field goal is made out of 4 cylinders
with a yellow material. The Field is made out of a plane with a png material (source below). 
No major problems arose in the implementation. I did realize that I had to set the football to kinematic
at the start of each new level so that the ball did not continue moving. Additionally, I utilized an
"activeBall" bool in the GameManager so that the floor collider did not go off at the wrong time. 



SOURCES:
Pebbles, granite, and stone wall .png file from https://www.wildtextures.com/category/options/seamless/
Grass/Hedge .png file from https://www.tonytextures.com/tileable-hedge-textures-free-download-3d-rendering/