## ADDED Requirements

### Requirement: Auto-reset on game start
`InputGate.locked` SHALL be reset to `false` automatically when the game starts (Play mode entry or build launch), before any scene loads.

#### Scenario: Re-enter Play mode after winning
- **WHEN** the player wins (InputGate.locked becomes true) and the developer stops and re-enters Play mode
- **THEN** `InputGate.locked` SHALL be `false` and all click input SHALL work normally

#### Scenario: Fresh build launch
- **WHEN** the game launches from a build
- **THEN** `InputGate.locked` SHALL be `false`

### Requirement: Manual reset method
`InputGate` SHALL expose a public static `Reset()` method that sets `locked` to `false`. This allows a future replay/restart system to reset input state.

#### Scenario: Replay calls Reset
- **WHEN** a replay system calls `InputGate.Reset()`
- **THEN** `InputGate.locked` SHALL be `false` and click input SHALL resume working
