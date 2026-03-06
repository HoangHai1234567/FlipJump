## Why

The game has no way to pause during gameplay. Players need to be able to pause the game to take a break or return to the home screen without losing/winning. The existing HUD already has a pause button texture (`button_pause.png`) copied from PinArrow but it's unused.

## What Changes

- Add a **pause button** to the existing in-game HUD (top-left corner)
- Create a **PopupPause** screen that appears when the pause button is tapped, showing Resume and Home buttons
- Implement **pause/resume logic** using `Time.timeScale = 0/1` integrated with GameManager
- Copy relevant pause UI textures from PinArrow (`F:\PinArrow\Assets\00 Game\Textures\Pin Arrow\Pause\`) — specifically the popup frame/background assets
- Strip PinArrow's toggle settings (Music/Sound/Haptic) and language selector since FlipJump doesn't have those systems

## Capabilities

### New Capabilities
- `pause-popup`: Pause popup UI with resume and home buttons, entrance/exit animation inheriting from PopupBase
- `pause-flow`: Pause/resume game flow using Time.timeScale, integrated with GameManager state machine

### Modified Capabilities
<!-- No existing specs to modify -->

## Impact

- `Assets/Scripts/Core/GameManager.cs` — add Paused state and Pause()/Resume() methods
- `Assets/Scripts/UI/UIIngame.cs` — add pause button reference and handler
- `Assets/Scripts/UI/PopupPause.cs` — new popup script extending PopupBase
- `Assets/Editor/UIPopupBuilder.cs` — add PopupPause prefab builder and HUD pause button setup
- `Assets/Art/UI/` — new pause UI textures from PinArrow
- `Assets/Prefabs/UI/PopupPause.prefab` — new prefab
