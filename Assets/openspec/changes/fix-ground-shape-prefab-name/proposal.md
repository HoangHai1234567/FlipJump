## Why

The user manually created the terrain prefab as "Terrain - GroundShape" but all code references use "GroundShape". The level editor, level loader, and setup script won't recognize the actual prefab.

## What Changes

- Update all hardcoded `"GroundShape"` prefab name references to `"Terrain - GroundShape"` across editor and runtime scripts
- Delete the old auto-generated `GroundShape.prefab` (replaced by user's manual prefab)
- Update `GroundShapeSetup.cs` to reference the correct prefab name/path

## Capabilities

### New Capabilities

### Modified Capabilities

## Impact

- **Files**: `LevelEditorWindow.cs`, `LevelLoader.cs`, `GroundShapeSetup.cs`
- **Risk**: Low — string constant changes only
