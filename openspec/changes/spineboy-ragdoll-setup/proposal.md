## Why

A new SpineBoy prefab was created from Spine. It currently has only a single root Rigidbody2D and CapsuleCollider2D with no ragdoll physics. It needs the same distributed ragdoll setup as StickmanRagdollV3: per-limb Rigidbody2D, colliders, HingeJoint2D, and gameplay scripts (ForcePoint, HeadCollision).

## What Changes

- Add Rigidbody2D (mass 0.5-0.8, gravityScale 0.4) to each major SpineBoy bone: head, chest, abdomen/hip, L/R-arm, L/R-forearm, L/R-thigh, L/R-foot
- Add colliders: CircleCollider2D for head, CapsuleCollider2D for limbs and torso
- Add HingeJoint2D between connected body parts with angle limits matching V3
- Switch SkeletonUtilityBone mode from Follow (0) to Override (1) on physics-driven bones so Rigidbody2D controls them
- Add ForcePoint script to root with bodyLift configured
- Add HeadCollision script to head bone
- Set all bones to Layer 6 (Ragdoll)

## Capabilities

### New Capabilities
- `spineboy-ragdoll`: Physics ragdoll configuration for the SpineBoy Spine character

### Modified Capabilities

## Impact

- `Assets/Prefabs/SpineBoy.prefab` — all changes are on this prefab (must be done in Unity Editor)
- Reuses existing scripts: ForcePoint, HeadCollision, InputGate
- No new scripts needed
