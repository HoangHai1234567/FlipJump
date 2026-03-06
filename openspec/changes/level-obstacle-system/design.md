## Context

FlipJump is a 2D physics ragdoll game where the player clicks to flip-jump a stickman across a level. Currently, the scene contains a single flat ground (`Square` prefab, scale 96x1) and one `PushForce` zone. The game uses kitchen assets from CatumetGames (Modern Kitchen 2D) for decoration only. There is no obstacle or platforming challenge — the ragdoll just flips across an empty surface.

Existing systems that interact with level elements:
- **GroundLanding.cs** — freezes ragdoll on collision with objects tagged "Ground"
- **ForcePoint.cs** — checks grounding via `Physics2D.OverlapCircle` on `groundLayer`
- **GroundLiftZone.cs** — applies lift when player is in zone
- **PushForceZone.cs** — applies horizontal push when player is in zone
- **CameraFollow.cs** — follows player on X axis

## Goals / Non-Goals

**Goals:**
- Create reusable obstacle prefabs from kitchen sprites with proper colliders
- Create elevated platform prefabs that work with existing Ground tag and landing system
- Support gaps in the ground that force the player to jump
- Provide a clean hierarchy for organizing level elements in the scene

**Non-Goals:**
- Procedural level generation — levels are authored as JSON, not generated
- Level progression or multiple scenes — single scene, single level file for now (but JSON format supports adding more later)
- Destructible obstacles — obstacles are static or knockable, not breakable
- Scoring or distance tracking — separate concern
- Moving platforms — static only for this change

## Decisions

### 1. Obstacle types: Static vs Knockable

**Decision:** Obstacles come in two variants — **static** (no Rigidbody2D, immovable) and **knockable** (has Rigidbody2D, can be pushed by the ragdoll on impact).

**Rationale:** Static obstacles create fixed challenges. Knockable obstacles add physics comedy (kitchen items flying when ragdoll crashes into them), which fits the game's tone. Both use the same prefab structure, just with/without Rigidbody2D.

**Alternative considered:** All obstacles static — simpler but less entertaining.

### 2. Platforms reuse Ground tag and layer

**Decision:** Elevated platforms use the same "Ground" tag and ground layer as the existing Square, so `GroundLanding` and `ForcePoint.CheckGrounded()` work without code changes.

**Rationale:** The existing systems already detect ground by tag/layer. Reusing these means zero script changes for platforms to "just work."

**Alternative considered:** New "Platform" tag — would require modifying GroundLanding and ForcePoint to also check this tag. Unnecessary complexity.

### 3. Gaps as empty space between ground segments

**Decision:** Gaps are created by splitting the single large ground into multiple smaller ground segments with space between them. No special "gap" GameObject needed.

**Rationale:** The simplest approach — just place ground prefabs with spaces. The ragdoll falls through gaps naturally due to gravity. No new code needed.

**Alternative considered:** Trigger zone "kill pits" — overengineered for now; gravity already handles falling.

### 4. Kitchen sprites as obstacle visuals

**Decision:** Use existing CatumetGames kitchen sprites directly on obstacle prefabs via SpriteRenderer, with manually fitted BoxCollider2D or PolygonCollider2D.

**Rationale:** Assets are already imported. BoxCollider2D is sufficient for most rectangular kitchen items (fridge, table, cabinet). PolygonCollider2D only for irregularly shaped items if needed.

### 5. Level hierarchy structure

**Decision:** All level elements organized under a root `Level` GameObject with children: `Ground/`, `Platforms/`, `Obstacles/`.

**Rationale:** Clean hierarchy makes it easy to select, move, and manage level elements in the editor. LevelLoader spawns into these containers at runtime.

### 6. Level data stored as JSON

**Decision:** Level layouts are defined in JSON files under `Assets/Levels/`. A `LevelLoader` MonoBehaviour reads the JSON at runtime (or on scene load) and instantiates prefabs at specified positions/rotations/scales.

**JSON structure:**
```json
{
  "name": "Level 1",
  "groundSegments": [
    { "prefab": "Square", "position": [0, -2, 0], "scale": [20, 1, 1] }
  ],
  "platforms": [
    { "prefab": "Platform", "position": [25, 0, 0], "scale": [5, 0.5, 1] }
  ],
  "obstacles": [
    { "prefab": "Fridge", "position": [15, -1.5, 0], "knockable": false },
    { "prefab": "BarStool", "position": [18, -1.5, 0], "knockable": true }
  ],
  "zones": [
    { "prefab": "PushForce", "position": [22, 0, 0] }
  ]
}
```

**Rationale:** JSON is human-readable, easy to edit outside Unity, version-control friendly, and Unity has built-in `JsonUtility` for parsing. This decouples level design from the scene file — the scene just needs a LevelLoader and prefab references.

**Alternative considered:**
- ScriptableObject — Inspector-friendly but harder to edit outside Unity and less readable in diffs.
- Scene-based — ties level layout to scene file, making it hard to create/share levels.

### 7. Prefab registry via serialized array

**Decision:** `LevelLoader` holds a serialized `PrefabEntry[]` array mapping string names to prefab references. JSON references prefabs by name (e.g., `"Square"`, `"Fridge"`), and the loader resolves them at runtime via dictionary lookup.

**Rationale:** Avoids hardcoded paths or `Resources.Load`. Prefab references are set in the Inspector, giving Unity's asset pipeline full control. The name→prefab mapping is explicit and easy to debug.

**Alternative considered:** Addressables — overkill for this project size. `Resources.Load` — not recommended by Unity for production.

## Risks / Trade-offs

- **[Physics performance]** Many knockable obstacles with Rigidbody2D could slow physics simulation → Mitigation: Use static obstacles by default, only mark select items as knockable. Keep total Rigidbody2D count low (<20).
- **[Collider sizing]** BoxCollider2D may not perfectly match kitchen sprite shapes → Mitigation: Acceptable for gameplay; pixel-perfect collision not needed for a ragdoll game.
- **[Ground segments]** Splitting ground into segments means GroundLiftZone width needs adjustment per segment → Mitigation: Each ground segment gets its own GroundLiftZone with auto-sized width (already supported in code).
- **[Camera]** Camera only follows X axis — player falling into gap won't be visible if gap is deep → Mitigation: Keep gaps shallow (ragdoll visible at bottom) or add Y follow later as separate change.
- **[JSON schema drift]** JSON structure may need to evolve as new element types are added → Mitigation: Keep JSON flat and simple; add new arrays for new types rather than changing existing structure.
- **[Prefab name mismatch]** JSON references a prefab name not registered in LevelLoader → Mitigation: Log warning and skip that element; don't crash the whole level load.
