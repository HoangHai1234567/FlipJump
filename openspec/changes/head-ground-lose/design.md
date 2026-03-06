## Context

The player ragdoll (StickmanRagdollV2) has a Head child with a Rigidbody2D and collider. Ground objects are tagged "Ground". The game already has `InputGate` for global input locking and `ForcePoint.FreezeAll()` for freezing the ragdoll.

## Goals / Non-Goals

**Goals:**
- Detect head-ground collision and trigger a lose state
- Lock input and freeze the player on lose (reuse existing systems)
- Skip detection if player has already won (`WinZone.triggered` or `InputGate.locked`)

**Non-Goals:**
- Lose UI, restart, or game over screen (future work)
- Death animation or visual effects

## Decisions

**Add a `HeadCollision` MonoBehaviour on the Head child object**

A simple script using `OnCollisionEnter2D` that checks if the colliding object has the "Ground" tag. This is the most direct approach since the Head already has a Rigidbody2D and collider as part of the ragdoll.

Alternative considered: raycasting from head downward. Rejected because the head already has physics colliders — `OnCollisionEnter2D` is simpler and more reliable.

**Guard against triggering after win**

Check `InputGate.locked` before processing the collision. If input is already locked (win state), the head collision is ignored. This prevents a lose trigger during the win sequence.

## Risks / Trade-offs

- [Head may briefly touch ground during normal movement] → May need a minimum velocity or contact angle threshold if false positives occur. Start simple, tune later.
