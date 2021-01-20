# ColorTyper
## Overview
ColorTyper is an interview project I completed for a game studio. 
- In Color Typer, the player must select the color of the sphere that is shot towards them to gain points and stop the ball from hitting them (and losing).

## Project Specification
### Game Loop
- In Color Typer, the player must select the color of the sphere that is shot towards them to gain points and stop the ball from hitting them (and losing). 
- The possible colors are Red, Blue, Green, Yellow, and Purple. 
- In a JSON file, there should be ball data that can be serialized into a serializable ball class. 
- Each ball prefab that is instantiated from wherever the spawn point should be a certain color (material should be different) and move closer towards the camera/player. 
- If the ball hits the player or takes up the entirety of the camera, the game is over.
### UI Engineering
- UI should contain three rectangular buttons on the lower half of the screen that become a color. 
- Two of the colors should be random incorrect colors, and one color should be the correct one. 
- The player needs to click the matching color with the ball to gain points.
### Saving System
- Every ball that is successfully guessed should have its velocity increased by a small bit.
- The points should double every 6 colors guessed correctly. The score should be displayed using TextMesh Pro and a non-standard font (download a font and import it as TextMesh Pro (‘Font Asset Creator’)).
- The high score is saved (hint) and can be seen whenever the player plays the game again.

# Playable Build
[Executable](https://drive.google.com/drive/folders/1Ro2Bw0EAKtR_sZGa7V5z0GG9BkU_XaHO?usp=sharing)
