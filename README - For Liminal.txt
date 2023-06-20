Unity Hierarchy:
- [Experience App]
	- VRAvatar
		- Gaze Input is used with the GazePointer GameObject under head, centereye, anchor, gazepointer
		The Gaze is used to create the icon and select menu options by looking at them without controllers
		- Position Player is used for the calibration of the player's height. See 'Calibrate Position' and the script 
		-Head, CenterEye:
			- GazePointer, Reticle is the visual for the reticle, GazePointer, GazeTimer uses the Liminal Ponter Timer UI object. It is disabled by PositionPlayer when game starts and leaving menus.
			- PlayerHitbox is the hitbox representing the headset. It has the PlayerHealth Script with hit sounds. Currently, the other audio sources attached to the hitbox are
			the Hit Sounds referenced in the Player Health inspector.
			- MissManager uses the MissController script to pitch and play sound effects when objects pass by the player without contact.
 
- Environment:
	- The Plane objects create the environment. The EM Pulser script is attached to create their shifts in visual colours, using the neonPanel material.
	- FakeFog obfuscates object spawns and distance.
	- Backfog is placed behind the player, this is seen and not the void
	- forwardParticleSystem and backwardParticleSystem are for the 'stars' in the environment
	- wallPulser creates the other tone (In this case white) to the green used by the Plane GameObjects.

- menuCanvas:
	- uses Canvas animation in Assets/Animations/UIAnimations
		- Title And Description ("Evade the Incoming Projectiles!")
- AudioManager:
	- Used for background music, controlled by GameManager. The selected music fir audio source is the one played during the game, not the menu music.
	- Starts with menu music and is changed after the player selects "Yes" and objects start spawning by GameManager.
- playModeCanvas (Name came from Seated/Standing when that was included):
	- Background/Foreground on text used for visual effect (yesBG with yesButton, Text/Background with Text/Foreground
- GameManager:
	- Game Manager Script:
		- End Time is when the game ends and "Complete" pops up.
		- Rewind Sound is played when a player fails and they are rewound back to the prior checkpoint. Located as an Audio source under GameManager
		- Music references AudioManager
		- Menu Music - the music played when the application is launched and while the player is in the menu screen. Located as an Audio source under GameManager
		- Rewind Duration is how long it takes to rewind to a checkpoint after health hits 0.
		- UX Prefabs:
			- Score Canvas is disabled, this is used if you want a score system.
			- Screen Canvas is displayed when the player takes damage, flashing the screen red briefly.
			- Dispenser (ObstacleDispenser prefab) - this is the spawner that handles the spawning of Objects. See Obstacle Dispenser section:
			- Uses the Check Point Script, based on times, to change between different sections and their scripts. Also displays Checkpoint text.
			- Complete Canvas displays on victory
			- Camera (empty) - Fetched by GameManger.cs under LinkSystems()
			- Miss Detector - References Miss Manager at VrAvatar/Head/Anchor/MissManager

- CompleteCanvas - Displayed when then player reaches end


	- Calibrate Position - this is the 'center position' used for calibration of the player. The PositionPlayer script references this position as the 'center'- taken when the player hits 'yes' and starts the spawning of objects at
	height based on the player's current height.


**ObstacleDispenser Prefab:**
	- This is the spawner and handler for blocks.
	- It is tied to the animator Assets/Animations/LevelDesign/ObstacleDispenser - the animation sections are located in folders at the same directory.
	- To edit block spawning, double click into the ObstacleDispenser GameObject, go to the Unity animation tab and select the section to edit - Object spawns are handled as Animation Events on the timeline. **SEE COLUMNS/ROWS.**
	- SpawnPoints and ProjectileContainer are referenced in the script
	- Script options:
		- Spread height values provide a modifier to how far apart blocks are spawned horizontally or vertically

		- **COLUMNS / ROWS** - The objects are spawned on a grid based on these modifiers. Some Animation Events spawn at a float value position based on this. 0 is the lowest row and leftmost column.
		We suggest that in order to avoid this causing issues down the line that a column/row value be determined early on as it can be tedious to change other values after adjusting this.

		For 5:3 Columns, 3 Rows:
		10	11	12	13	14
		5	6	7	8	9
		0	1	2	3	4
		
		- Projectiles: the materials that can be used.
		- Track Accuracy Damping is the randomised range that the projectile target can be off of player center by.
		- Diagonal Mat - used by commented out, unused AnimationEvents.


Scripts:
BCurveProjectile:
- Script to give projectiles a curved arc. Not thoroughly tested or used in current version.

Checkpoint:
- Handles Checkpoints and things which change (such as block speed and which animation section to reference) when checkpoints change. Used by CheckpointCanvas.

DispenserController:
- Handles the spawning and behaviours of projectiles. Used by ObstacleDispenser.

EMPulser:
- Used to create visual effect on walls. Used by elements under Environment in Hierarchy.

GameManager:
- Handles events, Rewind, Particles. Used by GameManager in Hierarchy.

InputDetection:
- Detects button inputs- used by Canvas.

MissController:
- Plays sounds when objects miss player. Pitch Randomised. Used by MissManager at [ExperienceApp]/VRAvatar/Head/CenterEye/MissManager.

PlayerHealth:
- Handles player health and damage immunity. Used at [ExperienceApp]/VRAvatar/Head/CenterEye/PlayerHitbox

PositionPlayer:
- Calibrates the current height of the player's headset and positions ObstacleDispenser to spawn projectiles relative to player height. Used by [ExperienceApp]/VRAvatar

ProgressionManager:*
- Referenced by GameManager. 

ScoreManager:
- Not currently in use. Used by ScoreCanvas.

ScuffedCamFinder:
- Deprecated code

WallSelfDestruct:
- Used by commented out, unused functions in ObstacleDispenser (it's a clean up script).