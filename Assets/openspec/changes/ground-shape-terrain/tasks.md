## 1. Copy textures and create SpriteShape profile

- [x] 1.1 Create `Assets/Textures/Terrain/` directory and copy `core_stone.png` and `surface_grass.png` from PinArrow (`F:/PinArrow/Assets/00 Game/Textures/Pin Arrow/Background/Terrain/`)
- [x] 1.2 Create a SpriteShape profile asset (`Assets/SpriteShape/GrassStone.asset`) with the copied fill texture and edge sprite (via GroundShapeSetup editor script)

## 2. Create GroundShape prefab

- [x] 2.1 Create `GroundShape` prefab in `Assets/Prefabs/` with: SpriteShapeController (using GrassStone profile, UpdateCollider enabled), SpriteShapeRenderer, PolygonCollider2D, GroundLiftZone (via GroundShapeSetup editor script)
- [x] 2.2 Set prefab tag to "Ground" and layer to "Ground" (via GroundShapeSetup editor script)

## 3. Add spline data model to LevelData

- [x] 3.1 Add `SplinePointData` serializable class to `LevelData.cs` with fields: `float[] position`, `float[] leftTangent`, `float[] rightTangent`, `int mode`
- [x] 3.2 Add `SplinePointData[] splinePoints` field to `LevelElement` class

## 4. Update level editor serialization

- [x] 4.1 Add `SerializeSplinePoints()` method to `LevelEditorWindow.cs` — reads SpriteShapeController spline and returns `SplinePointData[]`
- [x] 4.2 Call `SerializeSplinePoints()` in `CollectElements()` when the element has a SpriteShapeController
- [x] 4.3 Add `ApplySplinePoints()` method to `LevelEditorWindow.cs` — writes SplinePointData[] to SpriteShapeController spline
- [x] 4.4 Call `ApplySplinePoints()` in `SpawnEditorElement()` after instantiation
- [x] 4.5 Add "GroundShape" to `IsGroundPrefab()` check
- [x] 4.6 Add "GroundShape" to `IsObstaclePrefab()` exclusion list

## 5. Update runtime level loader

- [x] 5.1 Add "GroundShape" to `LevelLoader.GroundPrefabs` HashSet
- [x] 5.2 Add spline application logic in `LevelLoader.SpawnElement()` — after instantiation, if `el.splinePoints` is not null, apply to SpriteShapeController
- [x] 5.3 Register GroundShape prefab in the LevelLoader's prefabRegistry (via GroundShapeSetup editor script)

## 6. Adapt GroundLiftZone for PolygonCollider2D

- [x] 6.1 Update `GroundLiftZone.Awake()` to use `GetComponent<Collider2D>()` instead of `GetComponent<BoxCollider2D>()` for bounds width

## 7. Verify

- [ ] 7.1 Place a GroundShape in a level, edit spline points in scene view, save level, reload — verify shape persists
- [ ] 7.2 Play-test a level with GroundShape — verify collider works, GroundLiftZone works, character can land and interact
- [ ] 7.3 Load an existing level (level001) — verify backward compatibility (no errors, Square still works)
