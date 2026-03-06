## Context

SpineBoy is a Spine-animated character with a bone hierarchy driven by SkeletonUtilityBone components in Follow mode. StickmanRagdollV3 uses distributed Rigidbody2D + HingeJoint2D for physics ragdoll. The goal is to make SpineBoy behave identically to V3 in terms of physics.

The SpineBoy bone hierarchy is: root → hip → abdomen → chest → head, with arm/leg branches. All bones currently have SkeletonUtilityBone in mode 0 (Follow).

## Goals / Non-Goals

**Goals:**
- SpineBoy ragdoll behaves like StickmanRagdollV3 (same physics feel)
- Reuse existing ForcePoint and HeadCollision scripts
- Create an Editor script to automate the setup (avoid manual repetitive work)

**Non-Goals:**
- Spine animation blending with ragdoll (pure ragdoll mode only)
- Runtime switching between animated and ragdoll states

## Decisions

**Editor script approach**: Create `SpineBoyRagdollSetup.cs` in `Assets/Editor/` that:
1. Finds the SpineBoy prefab's bone hierarchy
2. Adds Rigidbody2D + Collider to each major bone
3. Adds HingeJoint2D connecting parent-child bones
4. Switches SkeletonUtilityBone mode from Follow (0) to Override (1)
5. Adds ForcePoint to root, HeadCollision to head
6. Sets all bones to Layer 6

This avoids error-prone manual prefab YAML editing and provides a repeatable, one-click setup.

**Bone-to-physics mapping** (matching V3):
| Bone | Mass | GravScale | Collider | Joint To |
|------|------|-----------|----------|----------|
| hip | 0.8 | 0.4 | Capsule | (root anchor) |
| abdomen | 0.6 | 0.4 | Capsule | hip |
| chest | 0.8 | 0.4 | Capsule | abdomen |
| head | 0.5 | 0.4 | Circle r=0.5 | chest |
| L/R-arm | 0.6 | 0.4 | Capsule | chest |
| L/R-forearm | 0.6 | 0.4 | Capsule | arm |
| L/R-thigh | 0.6 | 0.4 | Capsule | hip |
| L/R-foot | 0.6 | 0.4 | Capsule | thigh |

## Risks / Trade-offs

- **[Spine Override conflict]** → SkeletonUtilityBone Override mode may fight with skeleton update. Mitigation: disable Spine animation playback at runtime when ragdoll is active.
- **[Collider sizing]** → Bone dimensions differ from V3 sprite parts. Mitigation: editor script uses approximate sizes that can be fine-tuned manually after setup.
- **[Root Rigidbody2D]** → SpineBoy already has a root RB2D + CapsuleCollider. Mitigation: remove root collider, keep root RB2D as kinematic anchor or remove it if hip becomes the main body.
