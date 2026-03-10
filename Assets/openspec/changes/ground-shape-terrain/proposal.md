## Why

The current ground is a transparent Square prefab with a BoxCollider2D — functional but visually invisible. Replacing it with a SpriteShape-based terrain (like PinArrow's GroundShape) gives the ground a proper visual appearance with textured surfaces and edges, making levels look polished. The `com.unity.2d.spriteshape` package is already installed.

## What Changes

- Create a new `GroundShape` prefab using `SpriteShapeController` + `SpriteShapeRenderer` + `PolygonCollider2D`
- Copy terrain textures (core fill + surface edge) from PinArrow project
- Create SpriteShape profile asset(s) with fill texture and edge sprites
- Add `GroundShape` as a recognized ground prefab in level editor and level loader
- Serialize/deserialize spline point data in level JSON so terrain shapes persist per-level
- Existing `Square` and `Platform` prefabs remain unchanged for backward compatibility

## Capabilities

### New Capabilities
- `ground-shape-prefab`: SpriteShape-based ground prefab with spline editing, fill textures, edge sprites, and auto-generated collider
- `ground-shape-serialization`: Serialize/deserialize SpriteShapeController spline data (points, tangents) in level JSON

### Modified Capabilities

## Impact

- **New files**: GroundShape prefab, SpriteShape profile asset(s), copied terrain textures, optional collider sync script
- **Modified files**: `LevelEditorWindow.cs` (serialize spline data), `LevelLoader.cs` (deserialize spline data + recognize GroundShape), `LevelDataV2` classes (add spline fields)
- **Dependencies**: `com.unity.2d.spriteshape` (already installed v9.1.0)
- **Backward compatible**: Existing levels using Square/Platform are unaffected
