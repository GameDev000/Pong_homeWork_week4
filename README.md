# Pong – Project README

## 1. How to Play

You can play the game in the browser:  
**https://raz-oununu.itch.io/pong**

### Running Locally
1. Open the project in **Unity (6000.2.8f1 or compatible)**.
2. Load the main gameplay scene.
3. Press **Play**.

### Controls
- **Left Player:** W / S  
- **Right Player:** Arrow Up / Arrow Down 

### Target
- First player to reach **5 points**.

### Timer
- One minute. If no player reaches the target at the end of the time, there is no winner.

---

## 2. Class Relationships 

### **GameManager**
Handles score, timer, UI, and win conditions.  
Receives events from `Goal` and `OutOfLimits`.

### **PlayerMovment / InputMover**
Controls paddle movement, input, and camera-bounded clamping.

### **MoveDisk**
Responsible for disc physics:
- Launching the disc
- Resetting to center
- Ensuring minimum speed  
- Tracks which player hit last (`LastPlayerHit`).

### **CollisionDisk**
-Computes angle + force when disc hits a paddle.  
-Updates the disc velocity and sets the last hitter.

### **Goal**
Trigger zones behind each player that report scoring to `GameManager`.

### **OutOfLimits**
-Top/bottom triggers detecting when the disc exits the field.  
-Penalizes the last hitter, moving gates, and resets the disc.

### **Gates**
Mechanical obstacles using **HingeJoint2D** + motor rotation.

### **MagnetArtibute**
-Applies a distance-based magnetic pull on the disc.  
-Used as an “effort mechanic” by magnet.

### **AsteroidMoverDVD**
This functionality is currently not used in the game, but may be used in the future.

---

## 3. Architecture & Physics Decisions

### Physics Setup
- Moving objects use **Rigidbody2D** for consistent physics.
- Goals and boundaries (Up+Bottom) use **Trigger Colliders**.
- Gates use **HingeJoint2D** to create mechanical rotation.

### Disc Movement
- Light mass + strong impulse → responsive motion.
- Angle calculated from paddle impact offset, realistic control.
- Minimum speed enforced to avoid stalling.

### Paddle Control
- Screen-bounded via camera clamping.
- Supports both classic input and Input System actions.

### Environment Effects
- Asteroids are static and do not move or collide. The ball can collide with it.
- Each asteroid has a `MagnetArtibute` script that pulls the disc with:
  - Force decreasing by distance
  - Direction pointing toward the asteroid center 

### Responsibility Separation (Updated)
- **Physics:** MoveDisk, Gates, MagnetArtibute  
- **Gameplay Logic:** Goal, OutOfLimits, GameManager  
- **Input Handling:** PlayerMovment, InputMover  
- **Environment:** Static asteroids acting only as magnetic fields  

---

## 4. Difficulty Customization 

The game difficulty can be adjusted by tuning the following parameters:

- **Disk minimum speed (`MoveDisk.minSpeed`)** – higher = faster, more demanding; lower = slower, more forgiving.  
- **Disk start speed (`MoveDisk.startSpeed`)** – controls the intensity of the initial serve.  
- **Magnet force (`MagnetArtibute.magnetForce`)** – stronger pull makes the disc curve more aggressively.  
- **Magnet radius (`MagnetArtibute.magnetRadius`)** – controls how far from the asteroid the magnetic effect starts.  
- **Gate behaviour (`Gates.closeSpeed`, `Gates.motorF`)** – higher values make gates close faster and with stronger torque, increasing chaos and difficulty.  
- **Paddle movement speed (`PlayerMovment.speed` / `InputMover.speed`)** – higher speed makes defense easier; lower speeds increase challenge.

---

# 5. UML Diagram 

```text
+------------------------+        +------------------------+        +-----------------------+
|      GameManager       |        |        MoveDisk        |        |         Gates         |
+------------------------+        +------------------------+        +-----------------------+
| - targetGoals : int    |        | - rb : Rigidbody2D     |        | - hinge : HingeJoint2D|
| - leftScore : int      |        | - startSpeed : float   |        | - motor : JointMotor2D|
| - rightScore : int     |        | - minSpeed : float     |        | - isTop : bool        |
| - timer : float        |        | - LastPlayerHit : enum |        | - restAngle : float   |
| - UI texts (TMP)       |        +------------------------+        +-----------------------+
+------------------------+        | +Reset()               |        | +CloseGate()          |
| +ScoreLeftPlayer()     |        | +FirstMove()           |        +-----------------------+
| +ScoreRightPlayer()    |        | +KeepMinSpeed()        |
| +EndGameWithWinner()   |        | +SetLastPlayerHit()    |
| +EndGameByTime()       |        +------------------------+
+------------------------+

          ^                              ^                           ^
          |                              |                           |
          |        uses (score update)   |   uses (set velocity)     | used by OutOfLimits
          |                              |                           |
+------------------------+        +------------------------+        +----------------------+
|         Goal           |        |     CollisionDisk      |        |      OutOfLimits     |
+------------------------+        +------------------------+        +----------------------+
| - RightGoal : bool     |        | - isLeftPlayer : bool  |        | - gameManager        |
+------------------------+        | - hitSpeed : float     |        | - disk : MoveDisk    |
| +OnTriggerEnter2D()    |        | - yFactor : float      |        | - associated gates   |
+------------------------+        +------------------------+        +----------------------+
                                  | +OnCollisionEnter2D()  |        | +OnTriggerEnter2D()  |
                                  +------------------------+        | -CloseGatesAndReset()|
                                           ^                       +----------------------+
                                           |
                                           | applies hit response
                                           |
                     +--------------------+--------------------+
                     |                                         |
             +--------------------+                    +--------------------+
             |    PlayerMovment   |                    |     InputMover     |
             +--------------------+                    +--------------------+
             | - upKey : KeyCode  |                    | - moveAction       |
             | - downKey :KeyCode |                    | - speed : float    |
             | - speed : float    |                    | - padding : float  |
             +--------------------+                    +--------------------+
             | +Update()          |                    | +Update()          |
             | +SetKeys()         |                    | +OnEnable()/Disable|
             +--------------------+                    +--------------------+

                    interacts with                         affects disc motion
                               |                                       ^
                               v                                       |
                     +--------------------+                     +---------------------  -+
                     |   (Static Object)  |     magnet pull     |   MagnetArtibute       |
                     |     Asteroid       | -------------------> | - DiskRb : Rigidbody2D|
                     +--------------------+                     | - magnetRadius :float  |
                                                                | - magnetForce  :float  |
                                                                +------------------------+
                                                                | +FixedUpdate()         |
                                                                | +OnDrawGizmos...       |
                                                                +------------------------+
