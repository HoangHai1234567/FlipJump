## ADDED Requirements

### Requirement: Ground segmentation
The ground SHALL be split into multiple separate ground segments (using the Square prefab) with gaps between them. Each ground segment SHALL have the "Ground" tag and be on the ground layer.

#### Scenario: Ground consists of multiple segments
- **WHEN** the level is loaded
- **THEN** the ground SHALL consist of at least 3 separate ground segments with gaps between them

### Requirement: Gap causes falling
A gap SHALL be an empty space between two ground segments where no collider exists. The ragdoll SHALL fall through gaps due to gravity.

#### Scenario: Ragdoll falls into gap
- **WHEN** the ragdoll moves over a gap (empty space between ground segments)
- **THEN** the ragdoll SHALL fall downward due to gravity since there is no collider to support it

### Requirement: Gap width variation
Gaps SHALL vary in width to create different difficulty levels. Narrow gaps (1-2 units) SHALL be easy to flip over. Wide gaps (4-6 units) SHALL require more momentum.

#### Scenario: Narrow gap is jumpable
- **WHEN** a gap is 1-2 units wide
- **THEN** the ragdoll SHALL be able to clear it with a single flip-jump

#### Scenario: Wide gap requires momentum
- **WHEN** a gap is 4-6 units wide
- **THEN** the ragdoll SHALL need push force or multiple flips to clear it
