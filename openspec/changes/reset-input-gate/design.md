## Context

`InputGate.locked` is a static field. In Unity, static fields survive between Editor play sessions and persist until domain reload. After winning, re-entering Play mode starts with `locked = true`, making all input dead.

## Goals / Non-Goals

**Goals:**
- `InputGate.locked` is always `false` at game start
- A `Reset()` method exists for future replay/restart to call

**Non-Goals:**
- Implementing the full replay/restart system (future work)
- Resetting CameraFollow state (camera is an instance on Main Camera, destroyed between scenes)

## Decisions

**Use `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]` to reset static state**

This Unity attribute runs the method before any scene loads, on every Play mode entry and on build launch. It's the standard pattern for resetting static fields in Unity.

Alternative considered: resetting in `Awake()` of a MonoBehaviour. Rejected because it requires a specific GameObject to exist in every scene.

## Risks / Trade-offs

- [RuntimeInitializeOnLoadMethod runs before scene load] → This is exactly the timing we want, no risk
