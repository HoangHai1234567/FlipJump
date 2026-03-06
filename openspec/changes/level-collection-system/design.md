## Context

FlipJump has a LevelLoader.cs script that can load V1/V2 JSON level files, spawn prefabs, and tag ground objects. GameManager holds `TextAsset[] levels` and `LevelLoader levelLoader`. No LevelLoader exists in the scene, so nothing loads at runtime. ArrowTest's arrow-puzzle project uses a clean pattern: a **LevelCollection ScriptableObject** holding level references, with `GetLevel(index)` and a default fallback.

The player character is `StickmanRagdollV3` (root with ForcePoint component). Level JSONs store `playerPosition` but LevelLoader doesn't use it.

## Goals / Non-Goals

**Goals:**
- **LevelCollection ScriptableObject** to replace raw `TextAsset[]` on GameManager (following ArrowTest pattern)
- LevelLoader in scene with prefab registry for all 10 existing prefabs
- Level container for spawned level objects
- Player repositioning per level from JSON `playerPosition`
- End-to-end flow: Start → load level → Win → NextLevel → load next level
- Automated setup via UIPopupBuilder.SetupScene() for reproducibility

**Non-Goals:**
- Level select screen / menu
- Addressables / async loading (overkill for 3 levels)
- Level editor changes (already exists separately)
- PlayerPrefs persistence (keep static int for now, simple enough)

## Decisions

### 1. LevelCollection ScriptableObject (from ArrowTest)
Create a `LevelCollection` ScriptableObject with `[CreateAssetMenu]`. Holds an array of `LevelEntry` (name + TextAsset). `GetLevel(int index)` returns the entry with bounds clamping. GameManager references `LevelCollection` instead of `TextAsset[]`.

**Alternative**: Keep raw `TextAsset[]` on GameManager. Rejected — ScriptableObject is cleaner, reusable, and separates data from logic (ArrowTest's proven pattern).

### 2. Add playerTransform to LevelLoader
LevelLoader gets a `Transform playerTransform` field. In LoadV2, after spawning elements, set `playerTransform.position` from `data.playerPosition`. Keeps level loading self-contained.

### 3. Automate setup via UIPopupBuilder.SetupScene()
Add LevelLoader creation + prefab registry + Level container to SetupScene(). Prefab paths are hardcoded since the set is stable.

### 4. Level container as child management
LevelLoader uses a single `levelContainer` Transform. On load, clears all children then spawns new. `groundContainer` = same object (V2 flat structure).

### 5. GameManager uses LevelCollection
Replace `TextAsset[] levels` with `LevelCollection levelCollection`. In Awake: `levelLoader.levelJson = levelCollection.GetLevel(CurrentLevelIndex).levelJson`. GetLevelName returns the name from the collection entry.

## Risks / Trade-offs

- **Manually placed objects in scene** → LevelLoader.ClearContainer clears Level container only. Player, camera, HUD unaffected.
- **groundLayer** → Use LayerMask.GetMask("Default") for now.
- **Migration**: SetupScene needs to create the LevelCollection asset and assign it instead of the raw TextAsset array.
