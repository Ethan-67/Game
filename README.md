Game created with Unity that utilises procedural generation to create the terrain.

Sources for project
[1] - Lague, S. (2016). Procedural Landmass Generation [C#]. https://github.com/SebLague/Procedural-Landmass-Generation/tree/master/Proc%20Gen%20E13
[2] -- Bridson, R. (2007, August 5). Fast Poisson Disk Sampling in Arbitrary Dimensions. Fast Poisson Disk Sampling in Arbitrary Dimensions. Siggraph, Vancouver, Canada. https://doi.org/https://doi.org/10.1145/1278780.1278807
[3] - Abbitt, G. (2019, July 29). Create A Low Poly Well. https://www.youtube.com/watch?v=OlnkGCdtGEw
[4] - RigidBody Fps Controller . (2020, December 5). https://www.youtube.com/watch?v=LqnPeqoJRFY&t=3s
[4.1] - RigidBody Fps Controller . (2021, January 25). https://www.youtube.com/watch?v=E5zNi_SSP_w
[4.2] - RigidBody Fps Controller . (2021, February 21) https://www.youtube.com/watch?v=cTIAhwlvW9M&t=5s
[5] - FPS Movement. (2021, December 8). https://www.youtube.com/watch?v=rJqP5EesxLk
[5.1] - FPS Movement. (2022, February 22).  https://www.youtube.com/watch?v=gPPGnpV1Y1c&t=1018s
[6] - Papush, R. (2019, July 21). Clouds With Shader Graph. https://www.youtube.com/watch?v=Y7r5n5TsX_E

File Name / Location in relation to Game-main folder 	Source 
Game-main/Assets/Scripts/Terrain	[1] - Procedural-Landmass-Generation/Proc Gen E13/Assets/Scripts/ 

Game-main/Assets/Scripts/Terrain/MapGenerator.cs	(Lague, 2016, File. MapGenerator.cs)
Game-main/Assets/Scripts/Terrain/Noise.cs	(Lague, 2016, File. Noise.cs)
Game-main/Assets/Scripts/Terrain/ApplyTexture.cs	(Lague, 2016, File. TextureGenerator.cs)
Game-main/Assets/Scripts/Terrain/MeshDrawer.cs	(Lague, 2016, File. MeshGenerator.cs)
Game-main/Assets/Scripts/Terrain/EndlessMeshGenerator.cs	(Lague, 2016, File. EndlessTerrain.cs)
	
Game-main/Assets/Scripts/Procedural Placement	
Game-main/Assets/Scripts/ProceduralPlacement/PoissonDiskSampling.cs	[2] - (Bridson, 2007, sec. II. The Algorithm)
Game-main/Assets/Scripts/ProceduralPlacement/ProceduralPlacement.cs	N/A
Game-main/Assets/Scripts/ProceduralPlacement/ProceduralLayer.cs	N/A
	
Game-main/Assets/Scripts/PlayerPhysicsBased	
Game-main/Assets/Scripts/PlayerPhysicsBased/InputManager.cs	[5] - (FPS Movement, 2021, Timestamp. 7:22 – 9:00)
Game-main/Assets/Scripts/PlayerPhysicsBased/MoveCamera.cs	[4.2] - (RigidBody Fps Controller , 2020, Timestamp. 4:30 – 5:15)
Game-main/Assets/Scripts/PlayerPhysicsBased/PlayerInteract.cs	[5] -(FPS Movement, 2021, Timestamp. 7:22 – 9:00)
Game-main/Assets/Scripts/PlayerPhysicsBased/PlayerLook.cs	[4.1] - (RigidBody Fps Controller , 2020, Timestamp. 0:11 – 2:11)
Game-main/Assets/Scripts/PlayerPhysicsBased/PlayerMotor.cs	[4] - (RigidBody Fps Controller , 2020, Timestamp. 1:00 – 3:45)
Game-main/Assets/Scripts/PlayerPhysicsBased/PlayerUI.cs	[5.1] - -(FPS Movement, 2021, Timestamp. 13:00 – 13:35)
	
Game-main/Assets/Scripts/ObjectBehaviour/Interactable.cs	[5.1] - -(FPS Movement, 2021, Timestamp. 1:00 – 2:15)
	
Game-main/Assets/Meshes/Well.fbx	[3] - (Abbitt, 2019)
