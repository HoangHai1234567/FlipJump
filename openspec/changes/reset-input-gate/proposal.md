## Why

`InputGate.locked` is a static field that persists across Unity Editor play sessions. Once the player wins and the gate locks, re-entering Play mode keeps it locked, breaking all input. The same issue will occur when a replay/restart feature is added later.

## What Changes

- Auto-reset `InputGate.locked` to `false` when the game starts (each Play mode entry or build launch)
- Also reset camera and WinZone static state so a fresh game session always starts clean
- Provide a public `InputGate.Reset()` method that a future replay system can call

## Capabilities

### New Capabilities
- `input-gate-reset`: Automatic and manual reset of InputGate state on game start and replay

### Modified Capabilities

## Impact

- `Assets/Scripts/Core/InputGate.cs` — add `[RuntimeInitializeOnLoadMethod]` reset and `Reset()` method
- `Assets/Scripts/Camera/CameraFollow.cs` — may need a `Reset()` to clear `hasMaxX`/`sliding` state
