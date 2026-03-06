## ADDED Requirements

### Requirement: Per-bone Rigidbody2D
Each major SpineBoy bone (head, chest, abdomen, hip, L/R-arm, L/R-forearm, L/R-thigh, L/R-foot) SHALL have a Rigidbody2D with gravityScale 0.4 and distributed mass (head: 0.5, torso: 0.8, limbs: 0.6).

#### Scenario: SpineBoy falls with ragdoll physics
- **WHEN** SpineBoy is in the scene with no constraints
- **THEN** all limb rigidbodies SHALL fall independently under gravity at scale 0.4

### Requirement: Per-bone colliders
Each physics-driven bone SHALL have a collider: CircleCollider2D for head (radius ~0.5), CapsuleCollider2D for torso and limbs (sized to match bone visual).

#### Scenario: SpineBoy limbs collide with ground
- **WHEN** SpineBoy ragdoll lands on ground
- **THEN** each limb SHALL independently collide with Ground-tagged objects

### Requirement: HingeJoint2D connections
Connected bones SHALL be linked by HingeJoint2D with angle limits matching StickmanRagdollV3 ranges (e.g., head-chest: -53 to 30 degrees, legs: -55 to 90 degrees).

#### Scenario: Limbs constrained within natural range
- **WHEN** SpineBoy ragdoll is falling and rotating
- **THEN** limbs SHALL NOT rotate beyond their HingeJoint2D angle limits

### Requirement: SkeletonUtilityBone override mode
Physics-driven bones SHALL have their SkeletonUtilityBone mode set to Override (1) so Rigidbody2D drives bone position instead of Spine animation.

#### Scenario: Physics overrides animation
- **WHEN** SpineBoy ragdoll is active
- **THEN** bone positions SHALL be driven by Rigidbody2D physics, not Spine animation

### Requirement: Gameplay scripts
SpineBoy SHALL have ForcePoint on the root (with bodyLiftEnabled, bodyLiftRigidbody pointing to hip) and HeadCollision on the head bone.

#### Scenario: Click applies force
- **WHEN** the player clicks while SpineBoy is active
- **THEN** ForcePoint SHALL apply bodyLift force to the hip rigidbody

#### Scenario: Head hits ground triggers lose
- **WHEN** the SpineBoy head bone collides with a Ground-tagged object
- **THEN** HeadCollision SHALL trigger the lose state
