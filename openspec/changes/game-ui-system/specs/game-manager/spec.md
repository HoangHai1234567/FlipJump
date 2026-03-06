## ADDED Requirements

### Requirement: GameManager singleton manages game state
The system SHALL have a GameManager MonoBehaviour singleton that tracks the current game state: Playing, Won, or Lost. It SHALL be present in the game scene and persist for the duration of gameplay.

#### Scenario: Game starts in Playing state
- **WHEN** the scene loads
- **THEN** GameManager.State SHALL be Playing and InputGate.locked SHALL be false

#### Scenario: Only one instance exists
- **WHEN** the scene contains a GameManager
- **THEN** GameManager.Instance SHALL return that single instance

### Requirement: GameManager handles win condition
The system SHALL provide a `GameManager.Win()` method that transitions to the Won state and shows the win popup.

#### Scenario: Win triggered from WinZone
- **WHEN** WinZone completes its brake/freeze sequence and calls GameManager.Instance.Win()
- **THEN** GameManager.State SHALL change to Won
- **AND** the PopupWin prefab SHALL be instantiated and displayed

#### Scenario: Win cannot trigger twice
- **WHEN** GameManager.Win() is called while State is already Won or Lost
- **THEN** the call SHALL be ignored

### Requirement: GameManager handles lose condition
The system SHALL provide a `GameManager.Lose()` method that transitions to the Lost state and shows the lose popup.

#### Scenario: Lose triggered from HeadCollision
- **WHEN** HeadCollision detects head-ground collision and calls GameManager.Instance.Lose()
- **THEN** GameManager.State SHALL change to Lost
- **AND** the PopupLose prefab SHALL be instantiated and displayed after a short delay (1 second)

#### Scenario: Lose cannot trigger twice
- **WHEN** GameManager.Lose() is called while State is already Won or Lost
- **THEN** the call SHALL be ignored

### Requirement: GameManager supports level progression
The system SHALL track the current level index and provide methods for Replay, NextLevel, and Home actions.

#### Scenario: Replay reloads current scene
- **WHEN** Replay is invoked
- **THEN** InputGate.locked SHALL be reset to false
- **AND** the current scene SHALL be reloaded

#### Scenario: NextLevel loads the next level
- **WHEN** NextLevel is invoked
- **THEN** the level index SHALL increment by 1
- **AND** the current scene SHALL be reloaded (LevelLoader picks up new index)

#### Scenario: Home returns to first level
- **WHEN** Home is invoked
- **THEN** the level index SHALL reset to 0
- **AND** the current scene SHALL be reloaded

### Requirement: GameManager holds UI prefab references
GameManager SHALL have serialized fields for popupWinPrefab, popupLosePrefab, and a reference to the scene's UI Canvas parent.

#### Scenario: Prefabs assigned in inspector
- **WHEN** GameManager is configured in the scene
- **THEN** popupWinPrefab and popupLosePrefab SHALL be assignable via the Inspector
