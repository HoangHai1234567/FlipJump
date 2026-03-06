## Why

The game currently has no lose condition. The player can flip and land on their head without consequence. Adding a head-ground collision lose trigger gives the game its core challenge: land on your feet, not your head.

## What Changes

- Add a script to the Head bone of the player ragdoll that detects collision with Ground-tagged objects
- When the head touches the ground, trigger a lose sequence (lock input, brief delay, then signal loss)
- No UI or restart yet — just the detection and freeze, similar to how WinZone works

## Capabilities

### New Capabilities
- `head-lose-detection`: Detect when the player's head collides with ground and trigger a lose state

### Modified Capabilities

## Impact

- New script on the Head child of StickmanRagdollV2 prefab
- Uses existing `InputGate` to lock input on lose
- Uses existing `ForcePoint.FreezeAll()` to stop the ragdoll
