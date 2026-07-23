________________________________________

**Team Name:** Level One Legends
**Game Name:** Killer Robot
________________________________________

**Starting Scene:** StartMenuScene
**Main Gameplay Scene:** MainScene
________________________________________

TECHNICAL REQUIREMENTS / HOW TO PLAY:
________________________________________

1. The default scene is called StartMenuScene. This in turn loads MainScene when the Start Game button is clicked.
2. Rules & Info button shows a panel with info on rules and controls for the player to read before starting the game.
3. Controls include WASD to move, Shift to sprint, Spacebar to jump (once unlocked), C to aim the gun for increased accuracy,
   and right click for a melee attack.
   The "T" key closes and opens the in-game tip board while in the Main game scene, which provides players with the controls.
   and information on the pickup items that enemies can drop on death. The "P" key pauses the gameand displays the pause menu.
4. Gamepad Support is available for gameplay, but is not fully supported for all functions of navigating menus, pausing, etc, so keyboard and mouse are necessary.
5. A "debug mode" boolean exists within the PlayerController.cs script that is a component of Player. When debug mode is enabled, there are additional options
   available for testing the game. The "h" key will fully heal the player, the "b" key teleports the player to the boss room, "k" will instantly kill the player,
   and "i" toggles inversion of y-axis controls, useful for swapping between gamepad and keyboard/mouse while testing the game. (Only works while in debugging mode 
   so regular gameplay is not affected by this)

________________________________________

HOW TO OBSERVE TECHNOLOGY REQUIREMENTS:
________________________________________

1. Start menu is shown from the get go when opening the game build.
2. Pause menu is added for in game play use by having the user press "P" on the keyboard.
   It allows the user to choose between restarting the game, resuming the game, or go back to 
   the main menu.
3. Start menu showcases several features including game description and controls information.
   Settings are also a feature in the scene and the chosen settings for Audio, display and controls
   are kept persistent throughout the entire build (between scenes)
4. Audio settings include managing audio for master volume, music volume, and/or SFX volume
5. Controls settings include managing sensitivity, and the ability to invert the Y-axis
6. Display settings allow the user to enable/disable full screen mode, choose their computer, 
   and choose the aspect ratio that best fits their machine.
7. Gameplay starts with the player robot being transported into the killer robot factory where 
   enemy robots are roaming about.
8. As a quick tutorial, the objectives tell the player how to move, run, and shoot. With each 
   objective reached, the objective board dynamically updates to the next objective.
9. Tips board is found in the game play scene which can benefit the user if they forget controls 
   or want more gameplay information. This board can be toggled off and on by pressing "T".
10. The player robot features a green antenna while the eney robots feature a red antenna to 
   showcase the difference between the robots allow they look very similar.
11. The enemy robots have features such as player detection radius and have two types of enemies:
   the standing robot enemy and the robodogs enemies. Some robodogs give bullets to damage the player 
   from long range attacks. Some enemies can drop heath pickups that enable the player to choose 
   to defeat more enemies in the hopes of gaining back their player health. Each health pickup give 
   +10 health. The standing enemy robots give a damage when touching the player such as a headbut 
   animation effect.
12. The boss enemy is featured as being very scaled up in size from the robodog enemy prefab and 
   has a running effect at the player and contains boss health status which means extra health is 
   given to the boss enemy to make it more a challenge for the player to defeat it. The boss enemy 
   has a dying animation effect when defeated as well.
13. The player has a shooting animation effect with the gun in hand while shooting as well as a melee 
   animation affect by using the gun to hit nearby enemies when engaing in close range combat.
14. The obstacles in game include moving platforms, piston crushers, elevator rides, exploding barrels, 
   and spikey rolling balls as part of the ramp obstcle phase where the player has to dodge spikey objects 
   coming at the player or incur a health damage.
15. The game also has a death animation for the case of the player losing all health and dying to which 
   then the screen updates with a you died screen featuring buttons to play the game again or go back 
   to the main menu.
16. In the case the player defeats the boss, the boss defeat animation effect and win music plays 
   as well as the win screen appearing which give the player a storyline message for what happened to the 
   player after they escaped the killer robot facility. 

________________________________________

KNOWN PROBLEM AREAS:
________________________________________

1. Player has slight visual stutter while moving left and right on moving platforms.
2. The boss gets stuck for a moment in the piston crushers in the boss room.

________________________________________

MANIFEST - CONTRIBUTIONS BY TEAM MEMBER
________________________________________


*** Madeline Contributions: ***

** Scripts: ** 
Created Scripts/PlayerHealth1.cs located on player prefab 
Created UI/HealthUI.cs located on PlayerHUD/health counter in the MainScene
Created UI/StartMenuController.cs located on start menu panel in the StartMenuScene 
Created UI/ApplySettings.cs located on settings panel in the StartMenuScene
Created UI/AudioSettingsManager.cs located in the StartMenuScene
Created UI/ControlsSettingsManager.cs located in the StartMenuScene
Created UI/DisplaySettingsManager.cs located in the StartMenuScene
Created UI/GameTimer.cs located on the PlayerHUD in the MainScene
Created UI/PersistentSettings.cs 
Created UI/RulesPanelManager.cs located in the StartMenuScene 
Created UI/SettingsMenuUI.cs located in the StartMenuScene 
Created UI/SettingsUIBinder.cs
Created UI/TipsToggle.cs located in PlayerHUD in the MainScene
Created UI/WinScreenController.cs located in PlayerHUD in the MainScene
Created Platforming/ElevatorController.cs located on ElevatorBox prefab 
Created Platforming/ElevatorCarry.cs located on ElevatorBase prefab with child CarryZone 
Created Platforming/MovingStep.cs located on MovingStepParent Prefab 
Created Platforming/MovingStepCarry.cs located on Moving Step Parent prefab on child CarryTrigger Object 
Created Obstacles/CrushingPistonController.cs located in SecondFloor child object named CrushingPiston 
Created Obstacles/HazardDamage.cs located in SecondFloor/CrushingPiston/PistonDamageZone 
Created Obstacles/LaserDamage.cs located in LazerHazard prefab child object LaserTrigger 
Created Obstacles/LaserRotate.cs located on LazerHazard prefab 
Created Obstacles/LaserToggle.cs located on LazerHazard prefab 
Created Obstacles/LaserMoveUpDown.cs to be able to add it to desired lasers 
Created Obstacles/BallDespawnZone.cs located on SikeyRollingObject prefab
Created Obstacles/RampBallTrigger.cs located on SpikeyRollingObject prefab
Created Obstacles/RollingBallDamage.cs located on SpikeyRollingObject prefab
Created Obstacles/RollingBallSpawner.cs located on SpikeyRollingObject prefab
Modified AppEvents/PauseGameEvent.cs
Modified Audio/AudioEventManager.cs
Modified CameraScripts/PlayerCameraController.cs
Modified CharacterControllerScripts/PlayerController.cs
Modified EnemyScripts/BossController.cs
Modified EnemyScripts/EnemyController.cs
Modified UI/DeathScreenController.cs
Modified UI/ObjectiveController.cs

** Prefabs: **
Elevator Box 
LaserHazard 
MovingStepParent 
CrushingPiston
SpikeyRollingObject
Modified healthPickup
Modified jumpPickup

** Audio: **
AudioMixer.mixer
DeathMusic.mp3
MainSceneBkgrdMusic.mp3
WinMusic.mp3
Music Background KR 2.mp3

** Free Assets: **
Downloaded Assets/game-buttons-frames
Downloaded Assets/FreeLowpolyScifiObjects
Background Images

* Rooms: **
SecondFloor Rooms that include choice of left or right sequence where the player will have to decide which obstacle room they want to go through. 
RampPhase that includes Rolling spikey balls that spawn while the player is on the ramp and the player has to dodge the balls or encur -10 health.
ThirdFloor room that includes moving step staircase to enter the next room (boss room).
Boss Room created and populated with piston crushers.

** Scenes: **
Start Menu Scene created with four buttons: Start Game, Rules & Info, Settings and Exit Game.
Rules and Info Panel created to provide player rules and controls information.
Settings panel created to allow the user to choose which audio, control, and display settings best fit their preference for their gaming experience.

** Hierarchy components: **
MainMenu's Player HUD Health Counter, timer text, tips board, death mesage panel, win message panel, objectives message, and pause panel.
MainMenu's SecondFloor, SecondFloorOption2, RampPhase, ThirdFloor, and BossRoom with child objects created by me.
StartMenuScene's  MainMenu panel, Rules panel, and Settings panel created by me.

-------------------------------------------------------------------------------------------------------------------

*** Josh Contributions: ***

** Scripts: **
PauseGameEvent.cs 
CameraController.cs 
PlayerController.cs 
EnemyMovement.cs 
EnemyController.cs 
Health.cs 
MeleeWeaponController.cs 
MeleeWeaponScript.cs 
Rotator.cs 
GunController.cs 

** Animations: **
WeaponIdle.anim 
WeaponSwingAnimation.anim 
WeaponSwingController.controller 
LowerArm.anim 
RaiseArm.anum 
Shoot.anim 
PlayerRobotAnimatorController.controller 

** Models/Assets/Prefabs: **
EnemyPickup.prefab 
PolyOne- Gun Models (IMPORTED FROM STORE) 
sprite muzzle flashes (IMPORTED FROM STORE) 
Player Robot With M16A1.prefab 
PlayerHUD.prefab 

------------------------------------------------------------------------------------------------------------------ 

*** Sam Contributions: *** 

** Scripts: **
RangedEnemyAI.cs - Created Ranged Enemy Behvaior 
SwarmEnemyAI.cs - Created Swarm Enemy Behavior 
EnemyHealth.cs - Implemented Health to Enemies 
DeathScreenController.cs - Implemented Death Screen 
GunController.cs  - uncommented the damage code to use Enemy Health 
PlayerHealth1.cs - added Death Screen Controller to Die method 
PlayerController.cs - also added Death Screen Controller to Die method here and implemented take damage from enemies 
Projectile.cs - updated to find player and cause damage through PlayerHealth1.
BossAI.cs - Created Boss Enemy Behavior
EnemyAI.cs - Created single controller for Enemy AI Behavior 

** Models:**
Created mesh for Ranged Enemy 
Created mesh for Swarming Enemies 

-------------------------------------------------------------------------------------------------------------------

*** Mariana Contribution Notes: ***

** Models: **
Created robo dog mesh and armature
Created Exploding barrels


** Animations: **
Created animations and animation controller for robo dog
Created explosion

"Scripts"
Modified GunController.cs
Created ExplodingBarrels.cs

-------------------------------------------------------------------------------------------------------------------

*** Audrey Contributions: ***

** Scripts: **
Created AudioEventManager.cs
Created ElevatorFlipTrigger.cs
Created gunshotSoundGenerator.cs
Created jumpSoundGenerator.cs
Modified PlayerController.cs 
Modified PlayerHealth1.cs 
Modified CameraController.cs 
Modified RllingBallSpawner.cs
Modified PlayerCameraController.cs

** Animations: **
Jump.anim
Shoot.anim
PlayerDeath.anim
MoveBack.anim
MoveLeft.anim
MoveRight.anim
MoveForward.anim
Created BossDeath.anim

** Models/Assets/Prefabs: **
Created Player Robot mesh, armature, and animation controller / animator
Worked on main scene physical environment
robotHitAudio.mp3
pistonTrap.mp3
pickupAudio.mp3
laserHit.mp3
jumpSound.mp3
gunshot1.mp3
gunshot2.mp3
genericDeath.mp3
dogShooting.mp3
dogAttack.mp3
bossDeath.mp3
