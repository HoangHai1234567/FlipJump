## Context

FlipJump currently has a GameManager with three states (Playing, Won, Lost) and an in-game HUD with level text and restart button. PopupWin and PopupLose inherit from PopupBase which provides DOTween entrance/exit animations with `SetUpdate(true)` for timeScale independence. The `button_pause.png` texture is already in `Assets/Art/UI/`. PinArrow's pause system uses `Time.timeScale = 0` and a popup with Resume/Home/toggle-settings — we only need Resume and Home.

## Goals / Non-Goals

**Goals:**
- Pause button on the HUD that opens a pause popup
- PopupPause with Resume and Home buttons, reusing PopupBase animation system
- Freeze game physics and time via `Time.timeScale = 0`
- Resume restores `Time.timeScale = 1` and returns to gameplay
- Home navigates to level 0 (same as existing `GameManager.Home()`)

**Non-Goals:**
- Sound/music/haptic toggle settings (no audio system exists)
- Language selector
- Settings persistence (PlayerPrefs) — not needed yet
- Pause via device back button or system events

## Decisions

### 1. Add `Paused` to GameState enum
Add `Paused` to the existing `GameState { Playing, Won, Lost }` enum. `Pause()` only works from `Playing` state, `Resume()` only from `Paused` state. This prevents pausing after win/lose.

**Alternative**: Separate `bool isPaused` flag. Rejected because the state machine already exists and adding to it keeps the logic centralized.

### 2. Time.timeScale for pause
Set `Time.timeScale = 0` on pause, restore to `1` on resume. All existing DOTween popup animations already use `.SetUpdate(true)` so they'll work at timeScale 0.

**Alternative**: Disable specific scripts individually. Rejected — timeScale is simpler and matches PinArrow's approach.

### 3. PopupPause extends PopupBase
Reuse the same entrance/exit animation pattern. Resume calls `CloseAndAction` then `GameManager.Resume()`. Home calls `CloseAndAction` then `GameManager.Home()`.

### 4. Pause button placement
Add pause button to existing HUD (top-left corner, mirroring the restart button on top-right). Use the existing `button_pause.png` sprite. Wire it in UIIngame.cs.

### 5. GameManager owns timeScale
`GameManager.Pause()` sets `Time.timeScale = 0` and instantiates popupPausePrefab. `GameManager.Resume()` sets `Time.timeScale = 1` and restores state to `Playing`. This keeps all game flow logic in GameManager.

### 6. Ensure timeScale reset on scene reload
`GameManager.Replay()`, `NextLevel()`, and `Home()` already reload the scene. Add `Time.timeScale = 1` before scene load to ensure clean state.

## Risks / Trade-offs

- **WaitForSeconds in ShowLoseDelayed won't tick at timeScale 0** → Not an issue: pause is only reachable from Playing state, and Lose transitions out of Playing immediately.
- **PopupPause instantiated while timeScale = 0** → Safe: PopupBase animations use `SetUpdate(true)`.
- **HUD hides when state != Playing** → Need to also show HUD during Paused state, or hide it and rely on the popup overlay. Decision: keep HUD hidden during pause (popup overlay covers screen anyway).
