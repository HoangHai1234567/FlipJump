## 1. LevelCollection ScriptableObject

- [x] 1.1 Create `Assets/Scripts/Level/LevelCollection.cs` — ScriptableObject with `[CreateAssetMenu]`, holds `LevelEntry[]` (each with name + TextAsset), `GetLevel(int index)` with bounds clamping, `Count` property
- [x] 1.2 Create `Assets/Data/LevelCollection.asset` via editor script — populate with level001, level002, level003

## 2. GameManager Updates

- [x] 2.1 Replace `TextAsset[] levels` with `LevelCollection levelCollection` in GameManager
- [x] 2.2 Update `Awake()` to use `levelCollection.GetLevel(CurrentLevelIndex)` to assign levelJson
- [x] 2.3 Update `NextLevel()` bounds check to use `levelCollection.Count`
- [x] 2.4 Update `GetLevelName()` to return name from LevelCollection entry

## 3. LevelLoader Code Updates

- [x] 3.1 Add `playerTransform` field to `LevelLoader.cs`
- [x] 3.2 In `LoadV2()`, set `playerTransform.position` from `data.playerPosition` (guard null/empty)

## 4. UIPopupBuilder SetupScene Updates

- [x] 4.1 Add LevelLoader creation to `SetupScene()` — LevelLoader GameObject, Level container, prefab registry (10 prefabs), groundLayer, playerTransform (ForcePoint root)
- [x] 4.2 Create and assign LevelCollection asset in SetupScene
- [x] 4.3 Assign `gm.levelLoader` and `gm.levelCollection` after creation/find

## 5. Scene Setup & Verification

- [x] 5.1 Run "Tools/Setup Game Scene" to create LevelLoader + LevelCollection in Design scene
- [x] 5.2 Verify no compilation errors
- [x] 5.3 Save scene
