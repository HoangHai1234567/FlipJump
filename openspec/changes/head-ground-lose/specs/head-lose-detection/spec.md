## ADDED Requirements

### Requirement: Head-ground collision triggers lose
When the player's Head collider enters a collision with a Ground-tagged object, the system SHALL trigger a lose state.

#### Scenario: Head hits the ground
- **WHEN** the Head collider collides with a GameObject tagged "Ground"
- **THEN** `InputGate.locked` SHALL be set to `true` AND the player ragdoll SHALL be frozen via `ForcePoint.FreezeAll()`

#### Scenario: Head hits non-ground object
- **WHEN** the Head collider collides with a GameObject that is NOT tagged "Ground"
- **THEN** no lose state SHALL be triggered

### Requirement: Lose is ignored during win state
The head collision lose detection SHALL NOT trigger if `InputGate.locked` is already `true` (e.g., player already in win sequence).

#### Scenario: Head touches ground after winning
- **WHEN** the player has already entered WinZone (InputGate.locked is true) and the Head touches ground
- **THEN** no lose state SHALL be triggered

### Requirement: Lose triggers only once
The head collision lose detection SHALL trigger at most once per level play.

#### Scenario: Head touches ground multiple times
- **WHEN** the Head collides with ground, then collides again
- **THEN** the lose state SHALL only be triggered on the first collision
