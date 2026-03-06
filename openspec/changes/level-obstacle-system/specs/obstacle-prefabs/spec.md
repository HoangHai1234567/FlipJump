## ADDED Requirements

### Requirement: Static obstacle prefab structure
Each static obstacle prefab SHALL contain a SpriteRenderer (using a CatumetGames kitchen sprite), a BoxCollider2D fitted to the sprite bounds, and no Rigidbody2D. The obstacle SHALL be placed on the Default layer.

#### Scenario: Static obstacle blocks ragdoll
- **WHEN** the ragdoll collides with a static obstacle
- **THEN** the ragdoll bounces off or stops against the obstacle, and the obstacle does not move

#### Scenario: Static obstacle has correct collider
- **WHEN** a static obstacle prefab is inspected in the editor
- **THEN** the BoxCollider2D size SHALL approximate the visible sprite bounds

### Requirement: Knockable obstacle prefab structure
Each knockable obstacle prefab SHALL contain a SpriteRenderer, a BoxCollider2D, and a Rigidbody2D with gravity enabled. The Rigidbody2D mass SHALL be configurable per prefab.

#### Scenario: Knockable obstacle reacts to ragdoll impact
- **WHEN** the ragdoll collides with a knockable obstacle
- **THEN** the obstacle SHALL move based on physics forces from the collision

#### Scenario: Knockable obstacle falls with gravity
- **WHEN** a knockable obstacle is placed above the ground with no support
- **THEN** it SHALL fall until it rests on a collider below

### Requirement: Obstacle prefabs use kitchen sprites
Obstacle prefabs SHALL use sprites from the CatumetGames Modern Kitchen 2D asset pack. At minimum, the following items SHALL have prefabs: fridge, table, bar stool, microwave.

#### Scenario: Kitchen sprite visible on obstacle
- **WHEN** an obstacle prefab is instantiated in the scene
- **THEN** it SHALL display the corresponding kitchen sprite via SpriteRenderer
