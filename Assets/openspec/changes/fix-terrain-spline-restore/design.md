## Context

The level system serializes Terrain - GroundShape spline data to JSON correctly. However, when restoring:
- **Runtime**: `LevelLoader.ApplySplinePoints` modifies the spline after `Instantiate`, but `SpriteShapeController` has already baked its mesh during `Awake()`. The visual mesh is not regenerated. `BakeCollider()` only updates the collider. `BakeMesh()` may not exist in all SpriteShape package versions.
- **Editor**: `LevelEditorWindow.SpawnEditorElement` applies spline points via `ApplySplinePoints`, but the modifications on the prefab instance are not recorded as overrides. When entering Play mode, Unity serializes the scene and non-override changes are lost.

Current broken approaches tried:
- `ssc.BakeMesh()` — may not exist in the user's SpriteShape version
- `SplineApplier` MonoBehaviour with coroutine — added but not yet verified working

## Goals / Non-Goals

**Goals:**
- Terrain spline shape is visually correct after runtime level load
- Terrain spline shape persists in editor after Level Editor load (survives Play mode)
- Solution works across Unity 2D SpriteShape package versions

**Non-Goals:**
- Changing the spline serialization format (it works correctly)
- Supporting runtime spline editing by the player

## Decisions

### Runtime: Disable SpriteShapeController before spline modification, re-enable after
Instead of calling version-specific methods like `BakeMesh()`, disable the `SpriteShapeController` before clearing/modifying the spline, then re-enable it. When `OnEnable` runs, the controller re-initializes with the new spline data and generates the correct mesh.

**Alternative considered**: Coroutine with frame delay (`SplineApplier`). This works but adds an extra component and a visible 1-frame delay where the terrain shows the wrong shape. Direct disable/enable is cleaner and instantaneous.

**Alternative considered**: `BakeMesh()` — not available in all SpriteShape versions.

### Editor: Use SerializedObject for spline modification
Instead of directly modifying `ssc.spline` (which may not register as prefab overrides), use `Undo.RecordObject(ssc, ...)` before modification. This properly records the state change for both Undo and prefab override tracking.

**Alternative considered**: `PrefabUtility.RecordPrefabInstancePropertyModifications` after modification. This is less reliable because it captures modifications after the fact rather than recording the pre-change state.

## Risks / Trade-offs

- [Risk] Disabling/enabling SpriteShapeController might cause a visual flicker on the first frame → Mitigation: the disable/enable happens in the same `SpawnElement` call before the object is rendered, so no flicker should occur.
- [Risk] `Undo.RecordObject` in the editor might bloat the undo stack → Mitigation: this only happens during Level Editor Load, which already uses undo recording for the entire operation.
