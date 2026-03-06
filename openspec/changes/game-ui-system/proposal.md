## Why

FlipJump currently has no UI — win and lose conditions trigger only code-level events (InputGate lock, FreezeAll) with no visual feedback to the player. There are no screens to retry, go home, or advance to the next level. A complete in-game UI system is needed to make the game playable end-to-end.

## What Changes

- Add a **GameManager** singleton to orchestrate game state (playing, won, lost) and dispatch events
- Add a **PopupWin** screen shown when player enters WinZone — with "Next Level", "Replay", and "Home" buttons
- Add a **PopupLose** screen shown when head hits ground — with "Replay" and "Home" buttons
- Add an **in-game HUD** showing the current level name and a restart button
- Copy UI texture assets (ribbons, buttons) from the PinArrow project (`F:\PinArrow\Assets\00 Game\Textures\Pin Arrow\end game\`)
- Integrate with existing WinZone.cs and HeadCollision.cs to trigger UI popups
- Add scene reload and level progression logic (next level, replay, home)
- **No star/ending/chapter unlock system** — stripped from PinArrow reference

## Capabilities

### New Capabilities
- `game-manager`: Central game state controller — tracks playing/won/lost states, dispatches game events, handles level loading and scene transitions
- `popup-win-lose`: Win and lose popup screens with animated ribbons, action buttons (next, replay, home), and scene transition logic
- `ingame-hud`: In-game heads-up display showing level name and restart button

### Modified Capabilities

## Impact

- **WinZone.cs** — will call GameManager to trigger win instead of managing the full sequence alone
- **HeadCollision.cs** — will call GameManager to trigger lose instead of only freezing
- **New scripts**: GameManager.cs, PopupWin.cs, PopupLose.cs, UIIngame.cs
- **New prefabs**: PopupWin, PopupLose, UIIngame (Canvas-based)
- **New assets**: Copied from PinArrow — ribbon_victory.png, ribbon_defeat.png, button textures
- **Dependencies**: DOTween (for UI animations) — check if already in project, add if not
- **Scene changes**: Design.unity needs a UI Canvas with EventSystem
