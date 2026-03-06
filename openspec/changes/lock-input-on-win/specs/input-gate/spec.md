## ADDED Requirements

### Requirement: Global input gate
The system SHALL provide a static `InputGate` class with a `locked` boolean property. When `locked` is `true`, all click-based player interaction scripts MUST skip input processing.

#### Scenario: Gate blocks ForcePoint
- **WHEN** `InputGate.locked` is `true` and the player clicks
- **THEN** ForcePoint SHALL NOT call `ApplyForce()`

#### Scenario: Gate blocks PushForceZone
- **WHEN** `InputGate.locked` is `true` and the player clicks
- **THEN** PushForceZone SHALL NOT call `TryPush()`

#### Scenario: Gate blocks GroundLiftZone
- **WHEN** `InputGate.locked` is `true` and the player clicks
- **THEN** GroundLiftZone SHALL NOT call `TryLift()`

### Requirement: WinZone locks input gate on trigger
WinZone SHALL set `InputGate.locked = true` immediately when the win sequence starts, before freezing the player.

#### Scenario: Player enters WinZone
- **WHEN** the player collider enters the WinZone trigger
- **THEN** `InputGate.locked` SHALL be set to `true` before any other win sequence logic

#### Scenario: Click after entering WinZone
- **WHEN** the player has entered WinZone and the user clicks
- **THEN** no force SHALL be applied to the player from any source (ForcePoint, PushForceZone, GroundLiftZone)
