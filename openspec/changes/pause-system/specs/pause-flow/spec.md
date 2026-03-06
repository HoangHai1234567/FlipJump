## ADDED Requirements

### Requirement: GameManager supports Paused state
The GameManager.GameState enum SHALL include a Paused value. GameManager SHALL expose Pause() and Resume() methods.

#### Scenario: Pause from Playing state
- **WHEN** GameManager.Pause() is called while State is Playing
- **THEN** State changes to Paused, Time.timeScale is set to 0, and popupPausePrefab is instantiated

#### Scenario: Pause ignored when not Playing
- **WHEN** GameManager.Pause() is called while State is Won, Lost, or already Paused
- **THEN** nothing happens (method returns early)

#### Scenario: Resume from Paused state
- **WHEN** GameManager.Resume() is called while State is Paused
- **THEN** State changes to Playing, Time.timeScale is set to 1

### Requirement: TimeScale reset on scene transitions
GameManager.Replay(), NextLevel(), and Home() SHALL set Time.timeScale to 1 before reloading the scene.

#### Scenario: Replay after pause restores timeScale
- **WHEN** player pauses then taps Home or Replay (via popup)
- **THEN** Time.timeScale is set to 1 before scene reload

### Requirement: Pause button on HUD
The in-game HUD SHALL display a pause button (top-left corner) that calls GameManager.Pause() when tapped.

#### Scenario: Player taps pause button
- **WHEN** player taps the pause button during gameplay (State is Playing)
- **THEN** GameManager.Pause() is called

#### Scenario: Pause button hidden when not playing
- **WHEN** GameState is not Playing (Won, Lost, or Paused)
- **THEN** the HUD is hidden (existing behavior — hudContainer is deactivated)
