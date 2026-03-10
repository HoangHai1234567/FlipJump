## Why

When a user edits a Terrain - GroundShape's spline in the editor and then plays or saves+loads the level, the terrain reverts to the prefab's default shape. The spline data IS correctly serialized into the level JSON, but it is not properly restored at runtime (visual mesh not updated) or in the editor (prefab overrides not recorded).

## What Changes

- Fix runtime spline restoration: after applying saved spline points to an instantiated SpriteShapeController, force the visual mesh to regenerate (not just the collider)
- Fix editor spline restoration: after applying spline points to a prefab instance during level load, record the modifications as prefab overrides so they persist through Play mode transitions
- Remove the broken `SplineApplier` MonoBehaviour approach and `BakeMesh()` call; use a reliable refresh mechanism instead

## Capabilities

### New Capabilities
- `spline-restore`: Terrain spline data saved in level JSON is correctly restored both visually and physically when loading levels at runtime and in the editor

### Modified Capabilities

## Impact

- `Scripts/Level/LevelLoader.cs` — SpawnElement / ApplySplinePoints: fix runtime mesh refresh after spline modification
- `Scripts/Level/SplineApplier.cs` — remove or rework if coroutine approach is needed
- `Editor/LevelEditorWindow.cs` — SpawnEditorElement: ensure prefab override recording works correctly
