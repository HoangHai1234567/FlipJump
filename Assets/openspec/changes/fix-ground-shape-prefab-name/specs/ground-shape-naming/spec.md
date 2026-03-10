## MODIFIED Requirements

### Requirement: GroundShape is recognized as a ground prefab
The level editor and level loader SHALL recognize `"Terrain - GroundShape"` as the ground prefab name.

#### Scenario: Prefab name matches in editor and loader
- **WHEN** a level with a "Terrain - GroundShape" element is saved or loaded
- **THEN** the system SHALL correctly identify it as a ground prefab with tag "Ground" and layer "Ground"
