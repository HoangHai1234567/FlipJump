## Context

Three scripts independently listen for `Input.GetMouseButtonDown(0)`:
- `ForcePoint` — applies body lift / torque to the ragdoll
- `PushForceZone` — pushes the player horizontally
- `GroundLiftZone` — lifts the player vertically

When the player enters WinZone, only ForcePoint is locked via an instance field. The other two scripts continue responding to clicks, breaking the win sequence.

## Goals / Non-Goals

**Goals:**
- All click-based player interactions are disabled the moment WinZone triggers
- Single call site in WinZone to lock everything

**Non-Goals:**
- UI input blocking (no UI exists yet)
- Unlock/reset mechanism (will be handled by a future level restart system)

## Decisions

**Use a static `InputGate` class instead of a static field on ForcePoint**

A dedicated `InputGate` with a static `locked` property keeps the gate logic separate from any specific script. ForcePoint, PushForceZone, and GroundLiftZone all check `InputGate.locked` before processing clicks.

Alternative considered: static field on ForcePoint (`ForcePoint.globalInputLocked`). Rejected because it couples unrelated scripts (PushForceZone, GroundLiftZone) to ForcePoint.

## Risks / Trade-offs

- [Static state persists across scene loads] → Call `InputGate.locked = false` on level reset (future work, acceptable for now since win = end of level)
