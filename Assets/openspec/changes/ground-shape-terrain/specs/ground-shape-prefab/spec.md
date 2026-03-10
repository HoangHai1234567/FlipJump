## ADDED Requirements

### Requirement: GroundShape prefab uses SpriteShapeController for rendering
The `GroundShape` prefab SHALL use a `SpriteShapeController` component with a SpriteShape profile asset to render terrain with fill textures and edge sprites.

#### Scenario: GroundShape renders with fill and edge textures
- **WHEN** a GroundShape prefab is instantiated in a scene
- **THEN** it SHALL display a filled terrain shape with a tiling core texture and edge sprites along the surface

### Requirement: GroundShape prefab has auto-generated collider
The `GroundShape` prefab SHALL have `UpdateCollider` enabled on its `SpriteShapeController`, generating a `PolygonCollider2D` that matches the spline shape.

#### Scenario: Collider matches terrain shape
- **WHEN** the spline points of a GroundShape are modified
- **THEN** the PolygonCollider2D SHALL automatically update to match the new shape

### Requirement: GroundShape is recognized as a ground prefab
The level editor and level loader SHALL treat `GroundShape` as a ground prefab, applying the "Ground" tag and Ground layer.

#### Scenario: GroundShape gets ground tag and layer in editor
- **WHEN** a level containing a GroundShape element is loaded in the level editor
- **THEN** the GroundShape GameObject SHALL have tag "Ground" and layer "Ground"

#### Scenario: GroundShape gets ground tag and layer at runtime
- **WHEN** a level containing a GroundShape element is loaded by LevelLoader at runtime
- **THEN** the GroundShape GameObject SHALL have tag "Ground" and layer "Ground"

### Requirement: GroundLiftZone works with GroundShape
`GroundLiftZone` SHALL derive its zone width from any `Collider2D` bounds, not only `BoxCollider2D`.

#### Scenario: GroundLiftZone on GroundShape uses PolygonCollider2D bounds
- **WHEN** a GroundShape has a `GroundLiftZone` component and a `PolygonCollider2D`
- **THEN** `GroundLiftZone.zoneWidth` SHALL be set from the PolygonCollider2D's bounds width

### Requirement: GroundShape prefab is located in Assets/Prefabs
The `GroundShape` prefab SHALL be placed under `Assets/Prefabs/` so the level editor's auto-discovery finds it.

#### Scenario: Level editor discovers GroundShape prefab
- **WHEN** the level editor builds its prefab map from `Assets/Prefabs/`
- **THEN** the map SHALL contain an entry with key "GroundShape"
