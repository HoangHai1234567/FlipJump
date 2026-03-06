## Why

When the player enters the WinZone, only ForcePoint's click input is locked. However, PushForceZone and GroundLiftZone also listen for `Input.GetMouseButtonDown(0)` independently, so clicking still applies forces to the player during the win sequence. All click-based interactions must be disabled when the win state triggers.

## What Changes

- Introduce a shared input gate that all click-driven scripts check before processing input
- WinZone sets the gate on trigger, blocking ForcePoint, PushForceZone, and GroundLiftZone simultaneously
- Remove the instance-level `inputLocked` field from ForcePoint in favor of the shared gate

## Capabilities

### New Capabilities
- `input-gate`: A shared mechanism to globally disable all click-based player interactions (ForcePoint, PushForceZone, GroundLiftZone) from a single call site

### Modified Capabilities

## Impact

- `Assets/Scripts/Ragdoll/ForcePoint.cs` — replace instance `inputLocked` with shared gate check
- `Assets/Scripts/Ground/PushForceZone.cs` — add gate check before processing click
- `Assets/Scripts/Ground/GroundLiftZone.cs` — add gate check before processing click
- `Assets/Scripts/Level/WinZone.cs` — call the shared gate instead of ForcePoint.LockInput()
