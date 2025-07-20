# Source-Code-for-Subatomic
The c# source code for the unity project titled Subatomic, found on my itch.io page: https://nimclay.itch.io/


Scene setup:

Player (tag as Player, Layer as Player)
  - rigidbody component
  - sphere collider
  - movement.cs
  - FireParticle.cs
  - AbilityLogging.cs
  - HealthScript.cs
  - Player body (empty container, set reference to the body field for the health script
      - orbiting particle (however many), contains the OrbitingParticle.cs script and a trail renderer. Set FireParticle.orbit_electrons to these objects
      - lookat object (set to the lookat for the camera script)
      - Particle system force field
      - objects containing disabled effects for lightmode, defencemode, and electronbuff
   
  - all ability scripts (can be placed anywhere, just needs to be added to the list of abilities in the FireParticle component

For enemies, replace any attack prefabs with a variant that can collide with the Player layer. Also, set the AIControlled bool on movement.cs to true, and add the AIController script.

Camera
  - Camera component
  - Post processing components
  - CameraMove.cs
  - Box Collider (trigger), for object targeting
  - ObjectTargeting.cs
  - BindedObject.cs
  - SetMusicVolume.cs

GameManager
  - EndingManager.cs
  - EnemyManager.cs (one for players, and one for enemies)
  - RandomBackground.cs

Target (the visual for object targeting, set to the object targeting script in the camera)
  - BillboardQuad.cs
  - BindedObject.cs

Canvas
  - HideControls.cs (set values to text objects added onto the canvas)
  - For children: BaryonUI needs a Canvas Group and UIFade.cs components
