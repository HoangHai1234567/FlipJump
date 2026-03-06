## Context

FlipJump is a 2D ragdoll physics game in Unity. Currently, win (WinZone) and lose (HeadCollision) conditions only lock input and freeze the ragdoll — there is no UI feedback, no way to retry or advance levels. The PinArrow project (`F:\PinArrow\Assets\00 Game`) has a polished popup-based UI system with animated win/lose screens that we will adapt (simplified, no star/ending system).

Current game flow:
- WinZone.cs: locks InputGate, brakes velocity, freezes player, spawns celebrate animation
- HeadCollision.cs: locks InputGate, freezes player
- LevelLoader.cs: loads levels from JSON via prefab registry
- No GameManager, no UI Canvas, no scene management

## Goals / Non-Goals

**Goals:**
- Add GameManager to centralize game state and level progression
- Show animated PopupWin when player wins with Next/Replay/Home buttons
- Show animated PopupLose when head hits ground with Replay/Home buttons
- Show in-game HUD with level name and restart button
- Copy and use UI sprite assets from PinArrow (ribbons, buttons)
- Support level reload (replay), next level loading, and returning to a home/menu state

**Non-Goals:**
- No star/ending/chapter unlock system (PinArrow's UIEndingUnlockPanel, StarUI, etc.)
- No analytics or ads integration
- No Spine-based ribbon animations (use simple Image + DOTween instead)
- No home/menu scene — "Home" button reloads level 1 for now
- No sound system (can be added later)
- No save/progress system beyond current level tracking

## Decisions

### 1. Simple static GameManager vs Event-driven architecture
**Decision**: Use a simple singleton GameManager with direct method calls, not a full EventManager system like PinArrow.

**Rationale**: PinArrow's event system (EventManager, GamePlayEvents, UIEvents, generated invoke methods) is heavy infrastructure for a game with only 3 UI states. Direct calls (GameManager.Instance.Win(), GameManager.Instance.Lose()) are simpler and sufficient.

**Alternative considered**: Full event system — rejected as over-engineered for current scope.

### 2. Canvas popups as prefabs vs scene-embedded
**Decision**: Win/Lose popups as prefabs instantiated by GameManager at runtime. HUD as a persistent Canvas in the scene.

**Rationale**: Prefab popups keep the scene clean and are easy to show/hide. The HUD is always visible so it belongs in the scene.

### 3. DOTween for animations vs Unity Animator
**Decision**: Use DOTween for all UI animations (scale punch, fade, slide).

**Rationale**: PinArrow uses DOTween extensively for UI. It's more flexible than Animator for procedural animations. Check if DOTween is already in the project; if not, use free DOTween from Asset Store or simple coroutine-based tweens as fallback.

### 4. Modify WinZone/HeadCollision vs wrap them
**Decision**: Modify WinZone.cs and HeadCollision.cs to call GameManager instead of managing everything themselves. WinZone keeps its brake/freeze sequence but delegates the UI popup to GameManager. HeadCollision delegates to GameManager for the lose popup.

**Rationale**: Keeps the physics behavior in place while adding UI on top.

### 5. Asset copying approach
**Decision**: Copy PinArrow textures directly into `Assets/Art/UI/` folder. Only copy end-game ribbons and button textures — don't copy ingame HUD assets (create simple Unity UI instead).

**Files to copy**:
- `ribbon_victory.png`, `ribbon_defeat.png` — win/lose header ribbons
- `button_rectangle_blue.png`, `button_rectangle_green.png` — popup buttons
- `button_restart.png`, `button_pause.png` — HUD buttons (optional)

### 6. Level progression
**Decision**: GameManager tracks current level index. LevelLoader already supports loading from JSON files. Next level increments the index and reloads the scene. Levels are numbered sequentially (level001.json, level002.json, etc.).

**Rationale**: Simple and matches existing LevelLoader pattern.

## Risks / Trade-offs

- **[DOTween dependency]** → If not already in project, need to import. Fallback: use coroutine-based tweens.
- **[WinZone coupling]** → WinZone currently manages the full win sequence including celebrate prefab. Adding GameManager.Win() call means coordinating timing between celebrate animation and popup display. → Mitigation: popup appears after WinZone's existing delay, triggered at end of WinSequence.
- **[No home scene]** → "Home" button has no dedicated scene. → Mitigation: reload level 1 or first level for now; home scene can be added later.
- **[Prefab references]** → PopupWin/PopupLose prefabs need to be assigned to GameManager. → Mitigation: GameManager is a scene singleton with serialized prefab references.
