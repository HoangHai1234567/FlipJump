## Why

The game currently has a single flat ground (Square) and one PushForce zone but no system for placing obstacles, platforms, or interactive objects to create varied level layouts. Without obstacles, there's no challenge or progression — the ragdoll just flips across an empty surface. A level/obstacle system turns this into an actual game with things to navigate around, land on, and interact with.

## What Changes

- Add a reusable obstacle prefab system for placing physics-enabled kitchen items (tables, fridges, shelves, etc.) as obstacles in the level
- Add elevated platform prefabs that the ragdoll can land on at different heights
- Add a level layout container (parent GameObject) to organize all level elements under one hierarchy
- Add a gap/pit system — breaks in the ground that the player must flip over
- Integrate existing kitchen asset sprites (CatumetGames Modern Kitchen 2D) as obstacle visuals with appropriate colliders
- Store level layouts as JSON files under Assets/Levels/ — a LevelLoader script reads JSON and spawns prefabs at runtime, enabling easy level creation and editing outside Unity

## Capabilities

### New Capabilities
- `obstacle-prefabs`: Reusable obstacle prefabs built from kitchen sprites with BoxCollider2D, configurable as static or physics-enabled (knockable)
- `elevated-platforms`: Platform prefabs at varying heights that the ragdoll can land on, with Ground tag for landing detection
- `ground-gaps`: Gaps/pits in the ground surface that force the player to flip over them
- `level-layout`: A container system for organizing and placing obstacles, platforms, and gaps into a coherent level layout
- `level-data-json`: Level data stored as JSON files — a LevelLoader script reads JSON at runtime and spawns all level elements from prefabs

### Modified Capabilities

_None — no existing spec-level requirements change._

## Impact

- **Prefabs**: New prefabs under `Assets/Prefabs/Obstacles/` and `Assets/Prefabs/Platforms/`
- **Scene**: SampleScene will be updated with a designed level layout
- **Tags/Layers**: May reuse existing "Ground" tag for platforms; obstacles may need a new layer for collision filtering
- **Physics**: Obstacles need Collider2D and optionally Rigidbody2D for knockable items
- **Scripts**: New LevelLoader.cs script to parse JSON and instantiate prefabs at runtime
- **Assets**: JSON level files under Assets/Levels/
- **Existing code**: No changes to existing scripts — GroundLanding, ForcePoint, PushForceZone all work as-is with new tagged/layered objects
