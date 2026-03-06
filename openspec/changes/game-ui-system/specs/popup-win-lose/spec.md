## ADDED Requirements

### Requirement: PopupWin displays victory screen with action buttons
The PopupWin prefab SHALL display a victory ribbon image, and three buttons: "Next Level", "Replay", and "Home".

#### Scenario: PopupWin appears after winning
- **WHEN** GameManager instantiates PopupWin
- **THEN** a full-screen overlay SHALL appear with the victory ribbon centered at top
- **AND** three buttons SHALL be visible below the ribbon

#### Scenario: Next Level button advances level
- **WHEN** the player taps "Next Level"
- **THEN** GameManager.NextLevel() SHALL be called

#### Scenario: Replay button restarts current level
- **WHEN** the player taps "Replay"
- **THEN** GameManager.Replay() SHALL be called

#### Scenario: Home button returns to first level
- **WHEN** the player taps "Home"
- **THEN** GameManager.Home() SHALL be called

### Requirement: PopupLose displays defeat screen with action buttons
The PopupLose prefab SHALL display a defeat ribbon image and two buttons: "Replay" and "Home".

#### Scenario: PopupLose appears after losing
- **WHEN** GameManager instantiates PopupLose
- **THEN** a full-screen overlay SHALL appear with the defeat ribbon centered at top
- **AND** two buttons SHALL be visible below the ribbon

#### Scenario: Replay button restarts current level
- **WHEN** the player taps "Replay"
- **THEN** GameManager.Replay() SHALL be called

#### Scenario: Home button returns to first level
- **WHEN** the player taps "Home"
- **THEN** GameManager.Home() SHALL be called

### Requirement: Popup entrance animation
Both PopupWin and PopupLose SHALL animate on entrance with a scale punch effect and fade-in.

#### Scenario: Animated entrance
- **WHEN** a popup is instantiated
- **THEN** the popup panel SHALL scale from 0 to 1 with an overshoot ease (OutBack)
- **AND** a background overlay SHALL fade in from transparent to semi-transparent black
- **AND** the ribbon SHALL appear first, followed by buttons after a short delay

### Requirement: Button press animation
All popup buttons SHALL have a press animation — scale down on press, scale back up on release.

#### Scenario: Button pressed and released
- **WHEN** the player presses a button
- **THEN** the button SHALL scale to 0.9x
- **AND** on release, scale back to 1x with OutBack ease
- **AND** the button's action SHALL fire after the release animation

### Requirement: Popup blocks game input
While a popup is visible, game input SHALL remain locked (InputGate.locked = true).

#### Scenario: Input stays locked during popup
- **WHEN** PopupWin or PopupLose is active
- **THEN** InputGate.locked SHALL be true
- **AND** no gameplay forces can be applied
