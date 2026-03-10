## ADDED Requirements

### Requirement: Spline data is serialized in level JSON
When saving a level, if an element is a GroundShape, the editor SHALL serialize all spline point data (position, left tangent, right tangent, tangent mode) into the `LevelElement`'s `splinePoints` array.

#### Scenario: Save level with GroundShape
- **WHEN** a level containing a GroundShape with 5 spline points is saved
- **THEN** the JSON SHALL contain a `splinePoints` array with 5 entries, each having `position`, `leftTangent`, `rightTangent`, and `mode` fields

### Requirement: Spline data is deserialized at runtime
When loading a level, if an element has `splinePoints` data, the loader SHALL apply those points to the `SpriteShapeController`'s spline, replacing the prefab's default spline.

#### Scenario: Load level with GroundShape at runtime
- **WHEN** a level JSON with a GroundShape element containing `splinePoints` is loaded by `LevelLoader`
- **THEN** the instantiated GroundShape's `SpriteShapeController.spline` SHALL have the same point count, positions, and tangents as stored in the JSON

### Requirement: Spline data is deserialized in editor
When loading a level in the editor, if an element has `splinePoints` data, the editor SHALL apply those points to the `SpriteShapeController`'s spline.

#### Scenario: Load level with GroundShape in editor
- **WHEN** a level JSON with a GroundShape element is loaded in the level editor
- **THEN** the GroundShape's spline SHALL match the serialized data and be editable in the scene view

### Requirement: LevelElement supports optional spline data
The `LevelElement` class SHALL have an optional `splinePoints` field of type `SplinePointData[]`. This field SHALL be null for non-GroundShape elements.

#### Scenario: Non-GroundShape elements have null splinePoints
- **WHEN** a Square element is serialized
- **THEN** the `splinePoints` field SHALL be null (omitted from JSON)

### Requirement: Backward compatibility with existing levels
Existing level JSON files without `splinePoints` fields SHALL load without errors.

#### Scenario: Load legacy level without splinePoints
- **WHEN** an existing V2 level JSON (without any GroundShape elements) is loaded
- **THEN** all elements SHALL load normally with `splinePoints` defaulting to null
