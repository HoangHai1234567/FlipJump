## Context

The game uses `Time.timeScale = 0f` to pause physics and animations. However, Unity's `Update()` still runs at timeScale 0, so `Input.GetMouseButtonDown(0)` still fires. Four scripts — `ForcePoint`, `LiftPoint`, `GroundLiftZone`, and `PushForceZone` — call `Rigidbody2D.AddForce(..., ForceMode2D.Impulse)` from `Update()` on each tap. These impulse forces accumulate on the rigidbodies while physics is frozen and all apply simultaneously when `Time.timeScale` returns to 1 on resume.

The project already has an `InputGate.locked` static flag used to block input after win/lose, but it is **not** set during pause.

## Goals / Non-Goals

**Goals:**
- Prevent any gameplay input from being processed while the game is paused
- Use the existing `InputGate` mechanism so all input-consuming scripts are covered by one centralized check
- Fix `LiftPoint` which currently lacks the `InputGate.locked` check entirely

**Non-Goals:**
- Changing how pause/resume works (timeScale approach stays)
- Adding new input abstraction layers or event systems
- Handling edge cases like multi-touch or held-button states (only `GetMouseButtonDown` is used)

## Decisions

### Decision 1: Lock InputGate during pause rather than adding per-script GameState checks

**Choice:** Set `InputGate.locked = true` in `GameManager.Pause()` and `InputGate.locked = false` in `GameManager.Resume()`.

**Rationale:** All force scripts already check `InputGate.locked` (except `LiftPoint`, which is a secondary bug). Using the existing gate means one change in `GameManager` covers all current and future input consumers. Per-script `GameState` checks would be redundant, scattered, and easy to forget in new scripts.

**Alternative considered:** Check `GameManager.Instance.State != Playing` in each script's `Update()`. Rejected because it duplicates logic, adds a singleton dependency to every script, and doesn't fix the root cause (input not gated during pause).

### Decision 2: Add InputGate check to LiftPoint

**Choice:** Add `if (InputGate.locked) return;` to `LiftPoint.Update()`.

**Rationale:** `LiftPoint` is the only input-consuming script missing this guard. Without it, lift forces would still accumulate during pause (and during win/lose states). This is a pre-existing bug that this change fixes as a side effect.

## Risks / Trade-offs

- **[Risk] InputGate.locked state may conflict with win/lose lock** → No conflict. `Pause()` only fires from `Playing` state. `Resume()` restores `locked = false`, returning to the same state as before pause. Win/Lose set `locked = true` independently and never transition to `Paused`.
- **[Risk] Resume could incorrectly unlock input if it was locked before pause (e.g., head collision during pause transition)** → `HeadCollision` and `WinZone` only fire during `Playing` state, so `locked` cannot be set to `true` by them while paused. The state machine prevents this race.
