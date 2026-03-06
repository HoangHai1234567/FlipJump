## ADDED Requirements

### Requirement: Elevated platform prefab
An elevated platform prefab SHALL contain a SpriteRenderer, a BoxCollider2D, and no Rigidbody2D. The platform SHALL use the "Ground" tag and the same layer as the existing ground so that GroundLanding and ForcePoint ground detection work without code changes.

#### Scenario: Ragdoll lands on elevated platform
- **WHEN** the ragdoll body collides with an elevated platform
- **THEN** GroundLanding SHALL detect the collision via the "Ground" tag and freeze the ragdoll

#### Scenario: Ragdoll can jump from elevated platform
- **WHEN** the ragdoll is resting on an elevated platform and the player clicks
- **THEN** ForcePoint.CheckGrounded() SHALL return true (platform is on groundLayer) and the jump force SHALL be applied

### Requirement: Platform visual appearance
Platforms SHALL use a simple rectangular sprite (reuse the existing Square sprite or a similar solid-color sprite). The platform width and height SHALL be adjustable via Transform scale.

#### Scenario: Platform scale determines size
- **WHEN** a platform's Transform scale X is set to 5
- **THEN** the platform's visible width and collider width SHALL both reflect the 5x scale

### Requirement: Platform height variation
The level SHALL contain platforms at different Y positions to create vertical variation. Platforms SHALL be placed at a minimum of 2 distinct heights above the base ground level.

#### Scenario: Multiple platform heights exist
- **WHEN** the level is loaded
- **THEN** at least 2 platforms SHALL exist at different Y positions above the ground
