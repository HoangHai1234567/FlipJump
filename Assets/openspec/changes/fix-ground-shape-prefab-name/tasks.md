## 1. Update prefab name references

- [x] 1.1 In `LevelEditorWindow.cs` `IsGroundPrefab()`: change `"GroundShape"` to `"Terrain - GroundShape"`
- [x] 1.2 In `LevelEditorWindow.cs` `IsObstaclePrefab()`: change `"GroundShape"` to `"Terrain - GroundShape"`
- [x] 1.3 In `LevelLoader.cs` `GroundPrefabs` HashSet: change `"GroundShape"` to `"Terrain - GroundShape"`
- [x] 1.4 Update `GroundShapeSetup.cs` to reference `"Terrain - GroundShape"` prefab path and name

## 2. Cleanup

- [x] 2.1 Delete old `Assets/Prefabs/GroundShape.prefab` (replaced by user's manual prefab)
