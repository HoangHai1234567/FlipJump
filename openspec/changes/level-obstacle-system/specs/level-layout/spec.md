## ADDED Requirements

### Requirement: Level root hierarchy
All level elements SHALL be organized under a root GameObject named "Level" in the scene hierarchy. The Level object SHALL have child containers: "Ground", "Platforms", "Obstacles".

#### Scenario: Level hierarchy in editor
- **WHEN** the scene is opened in the Unity editor
- **THEN** a "Level" root object SHALL exist with children "Ground", "Platforms", and "Obstacles"

#### Scenario: Ground segments under Ground container
- **WHEN** a ground segment is placed in the level
- **THEN** it SHALL be a child of the "Level/Ground" container

### Requirement: Level layout provides gameplay progression
The level layout SHALL progress from simple (flat ground, no obstacles) at the start to increasingly complex (gaps, obstacles, elevated platforms) as the player moves right.

#### Scenario: Level starts simple
- **WHEN** the ragdoll begins at the leftmost position
- **THEN** the initial area SHALL have flat ground with no obstacles or gaps

#### Scenario: Difficulty increases rightward
- **WHEN** the player progresses to the right
- **THEN** the level SHALL introduce gaps first, then obstacles, then elevated platforms

### Requirement: Existing scene objects integrated
The existing PushForce zone and kitchen decoration objects SHALL be repositioned into the level layout under appropriate containers. The StickmanRagdollV2 player SHALL remain at the root level (not under Level).

#### Scenario: PushForce zone in level
- **WHEN** the level is loaded
- **THEN** the PushForce zone SHALL be positioned at a meaningful location within the level (e.g., before a wide gap to help the player gain momentum)
