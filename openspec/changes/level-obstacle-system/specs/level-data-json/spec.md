## ADDED Requirements

### Requirement: JSON level file format
Level data SHALL be stored as JSON files under `Assets/Levels/`. Each JSON file SHALL contain arrays for ground segments, platforms, obstacles, and zones. Each element SHALL specify a prefab name, position, and optionally scale and rotation.

#### Scenario: Valid JSON level file
- **WHEN** a JSON file exists at `Assets/Levels/level1.json`
- **THEN** it SHALL contain a JSON object with `name` (string), `groundSegments` (array), `platforms` (array), `obstacles` (array), and `zones` (array)

#### Scenario: Element entry structure
- **WHEN** an element is defined in any array
- **THEN** it SHALL have at minimum `prefab` (string) and `position` (array of 3 numbers), with optional `scale` (array of 3 numbers), `rotation` (array of 3 numbers), and type-specific fields

### Requirement: LevelLoader script
A `LevelLoader` MonoBehaviour SHALL be responsible for reading a JSON level file and instantiating all level elements from prefabs at runtime. It SHALL be attached to a GameObject in the scene.

#### Scenario: Level loads on scene start
- **WHEN** the scene starts and LevelLoader is present
- **THEN** LevelLoader SHALL read the configured JSON file and instantiate all elements under the Level hierarchy

#### Scenario: LevelLoader clears previous level
- **WHEN** LevelLoader loads a level
- **THEN** it SHALL destroy all existing children under the Level containers before spawning new elements

### Requirement: Prefab registry
LevelLoader SHALL have a serialized array of prefab entries mapping string names to GameObject prefab references. JSON elements reference prefabs by name. The mapping SHALL be configured in the Unity Inspector.

#### Scenario: Prefab resolved by name
- **WHEN** a JSON element has `"prefab": "Fridge"`
- **THEN** LevelLoader SHALL look up "Fridge" in its prefab registry and instantiate that prefab

#### Scenario: Unknown prefab name
- **WHEN** a JSON element references a prefab name not in the registry
- **THEN** LevelLoader SHALL log a warning and skip that element without crashing

### Requirement: Spawned elements use correct hierarchy
LevelLoader SHALL instantiate ground segments under `Level/Ground`, platforms under `Level/Platforms`, obstacles under `Level/Obstacles`, and zones at the appropriate location.

#### Scenario: Ground segment parented correctly
- **WHEN** a ground segment is spawned from JSON
- **THEN** it SHALL be a child of the "Level/Ground" container

#### Scenario: Obstacle parented correctly
- **WHEN** an obstacle is spawned from JSON
- **THEN** it SHALL be a child of the "Level/Obstacles" container

### Requirement: Spawned elements receive correct tags and layers
Ground segments and platforms spawned by LevelLoader SHALL automatically receive the "Ground" tag and ground layer. Obstacles SHALL remain on the Default layer.

#### Scenario: Spawned ground has Ground tag
- **WHEN** a ground segment is instantiated by LevelLoader
- **THEN** its tag SHALL be "Ground" and its layer SHALL match the ground layer

#### Scenario: Spawned platform has Ground tag
- **WHEN** a platform is instantiated by LevelLoader
- **THEN** its tag SHALL be "Ground" and its layer SHALL match the ground layer
