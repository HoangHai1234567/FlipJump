## Context

All scripts use `"GroundShape"` but the actual prefab is named `"Terrain - GroundShape"`. The prefab map is built by prefab file name, so the name must match exactly.

## Goals / Non-Goals

**Goals:** Replace all `"GroundShape"` string references with `"Terrain - GroundShape"`.
**Non-Goals:** No logic changes, no new features.

## Decisions

### Decision 1: Simple string replacement

All occurrences of `"GroundShape"` in `IsGroundPrefab`, `IsObstaclePrefab`, `GroundPrefabs` HashSet, and `GroundShapeSetup` will be updated to `"Terrain - GroundShape"`.

## Risks / Trade-offs

- None — straightforward rename.
