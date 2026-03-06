## ADDED Requirements

### Requirement: LevelLoader loads levels at runtime
The LevelLoader component SHALL be present in the Design scene with a prefab registry containing all level prefabs and a level container for spawned objects. On Start(), it SHALL load the level JSON assigned by GameManager.

#### Scenario: First level loads on scene start
- **WHEN** the scene starts and GameManager has assigned level001.json to LevelLoader.levelJson
- **THEN** LevelLoader spawns all elements from the JSON under the Level container, tags ground objects as "Ground", and logs the level name

#### Scenario: Level container is cleared before loading
- **WHEN** LevelLoader.LoadLevel() is called
- **THEN** all existing children of levelContainer are destroyed before spawning new elements

### Requirement: Player position is set from level data
LevelLoader SHALL have a `playerTransform` field. When loading a V2 level, it SHALL set the player's position from `data.playerPosition`.

#### Scenario: Player repositioned on level load
- **WHEN** a V2 level is loaded with playerPosition [-2.34, -0.99, 0]
- **THEN** playerTransform.position is set to (-2.34, -0.99, 0)

#### Scenario: Missing playerPosition is ignored
- **WHEN** a V2 level is loaded with null or empty playerPosition
- **THEN** playerTransform.position is not modified

### Requirement: NextLevel loads the next level
When the player wins and taps "NEXT LEVEL", GameManager.NextLevel() SHALL increment CurrentLevelIndex, reload the scene, and the next level JSON SHALL be loaded by LevelLoader.

#### Scenario: Win level 1, advance to level 2
- **WHEN** player is on level 1 (index 0), wins, and taps "NEXT LEVEL"
- **THEN** CurrentLevelIndex becomes 1, scene reloads, LevelLoader loads level002.json

#### Scenario: Last level wraps or stays
- **WHEN** player is on the last level and taps "NEXT LEVEL"
- **THEN** CurrentLevelIndex does not exceed levels.Length - 1

### Requirement: Prefab registry covers all level prefabs
The prefab registry SHALL include entries for: Square, PushForce, WinZone, wall, BarStool, Block, Fridge, Microwave, Table, Platform.

#### Scenario: All level prefabs are registered
- **WHEN** LevelLoader is set up via UIPopupBuilder.SetupScene()
- **THEN** prefabRegistry contains 10 entries matching all prefab names used in level JSON files

### Requirement: SetupScene automates LevelLoader creation
UIPopupBuilder.SetupScene() SHALL create a LevelLoader GameObject with prefab registry, Level container, player transform reference, and ground layer when one doesn't exist.

#### Scenario: SetupScene creates LevelLoader
- **WHEN** "Tools/Setup Game Scene" is executed and no LevelLoader exists in scene
- **THEN** a LevelLoader GameObject is created with full prefab registry, Level container, playerTransform assigned to StickmanRagdollV3, and groundLayer set

#### Scenario: SetupScene preserves existing LevelLoader
- **WHEN** "Tools/Setup Game Scene" is executed and a LevelLoader already exists
- **THEN** the existing LevelLoader is not replaced, only GameManager.levelLoader is assigned
