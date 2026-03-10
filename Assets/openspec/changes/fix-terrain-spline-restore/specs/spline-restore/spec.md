## ADDED Requirements

### Requirement: Runtime spline restoration updates visual mesh
When a level is loaded at runtime, Terrain - GroundShape elements with saved spline data SHALL display the saved shape visually, not the prefab's default shape.

#### Scenario: Terrain shape matches saved spline after runtime load
- **WHEN** LevelLoader loads a level containing a Terrain - GroundShape element with splinePoints data
- **THEN** the instantiated terrain SHALL display the saved spline shape (both collider and rendered mesh)

### Requirement: Editor spline restoration persists as prefab overrides
When a level is loaded in the Level Editor, spline modifications on Terrain - GroundShape prefab instances SHALL be recorded as prefab overrides so they survive scene operations and Play mode transitions.

#### Scenario: Loaded terrain shape survives editor Play mode
- **WHEN** a level with custom terrain spline is loaded via Level Editor's Load button
- **AND** the user presses Unity's Play button (not Level Editor's Play Level)
- **THEN** the terrain in the scene SHALL retain the loaded spline shape

### Requirement: Level Editor Play Level preserves terrain shape
When the user presses the Level Editor's "Play Level" button, the terrain shape from the current scene SHALL be serialized and restored at runtime.

#### Scenario: Play Level button preserves edited terrain
- **WHEN** the user edits a Terrain - GroundShape's spline in the scene
- **AND** presses the Level Editor's "Play Level" button
- **THEN** the terrain in play mode SHALL display the edited spline shape
