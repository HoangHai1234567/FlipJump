## Why

When the player taps/clicks during pause (or hits the pause button which registers as a tap), force is still applied to the ragdoll character via `AddForce` calls. These forces queue up while physics is frozen (`Time.timeScale = 0`), and when the game resumes, all accumulated forces fire simultaneously — launching the character unpredictably. This breaks gameplay and makes the pause feature unreliable.

## What Changes

- Add a pause-state guard to all input-driven force scripts so that taps/clicks during pause are ignored entirely
- Ensure `InputGate.locked` is set to `true` when paused and restored on resume, providing a single centralized check
- No new features or APIs; this is a targeted bug fix

## Capabilities

### New Capabilities
- `pause-input-blocking`: Prevent all gameplay input (force application, lifts, pushes) from being processed while the game is paused

### Modified Capabilities

## Impact

- **Scripts affected**: `GameManager.cs` (pause/resume methods), `ForcePoint.cs`, `LiftPoint.cs`, `GroundLiftZone.cs`, `PushForceZone.cs`
- **Risk**: Low — adds early-return guards only; no changes to physics, UI, or game flow
- **Dependencies**: None
