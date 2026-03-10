## ADDED Requirements

### Requirement: Input SHALL be blocked while game is paused
The system SHALL set `InputGate.locked` to `true` when entering the paused state, preventing all gameplay input from being processed.

#### Scenario: Player taps during pause
- **WHEN** the game is paused and the player taps/clicks the screen
- **THEN** no force, lift, or push SHALL be applied to any rigidbody

#### Scenario: Pause button tap does not register as gameplay input
- **WHEN** the player taps the pause button
- **THEN** `InputGate.locked` SHALL be set to `true` before the next `Update()` frame, so the tap that triggered pause cannot also trigger a force

### Requirement: Input SHALL be restored when game resumes
The system SHALL set `InputGate.locked` to `false` when resuming from pause, re-enabling gameplay input.

#### Scenario: Player resumes and taps
- **WHEN** the game is resumed from pause and the player taps/clicks
- **THEN** forces SHALL be applied normally as in the `Playing` state

### Requirement: All input-consuming scripts SHALL check InputGate
Every script that processes player tap/click input to apply physics forces SHALL check `InputGate.locked` and return early if locked.

#### Scenario: LiftPoint respects InputGate
- **WHEN** `InputGate.locked` is `true` and the player taps
- **THEN** `LiftPoint` SHALL NOT apply any lift force
