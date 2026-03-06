## Why

The game has 3 level JSON files and a LevelLoader script, but nothing is wired together at runtime. There's no LevelLoader in the scene, no prefab registry, and no level container. When `GameManager.NextLevel()` reloads the scene, the next level doesn't actually load — the scene always shows the same manually-placed objects. The level system needs to be connected end-to-end so levels load from JSON dynamically and NextLevel/Replay/Home actually switch between levels.

## What Changes

- Add **LevelLoader GameObject** to the Design scene with prefab registry (10 prefabs), level container, and ground layer configured
- Add **player position** support to LevelLoader so the player spawns at the correct position per level
- Wire **GameManager → LevelLoader** so levels load automatically on scene start
- Update **UIPopupBuilder.SetupScene()** to automate LevelLoader creation for future scenes
- Update **UIIngame** level text to show the current level name from JSON data

## Capabilities

### New Capabilities
- `level-loading`: Runtime level loading from JSON via LevelLoader, with prefab registry, level container, player positioning, and GameManager integration

### Modified Capabilities

## Impact

- `Assets/Scripts/Level/LevelLoader.cs` — add playerTransform field, set player position from V2 JSON
- `Assets/Scripts/Core/GameManager.cs` — minor: already wires levels, may need level name from loaded data
- `Assets/Editor/UIPopupBuilder.cs` — add LevelLoader + Level container + prefab registry creation to SetupScene()
- Design scene — add LevelLoader GameObject, Level container, configure prefab registry
