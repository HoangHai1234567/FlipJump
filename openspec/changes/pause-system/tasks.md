## 1. Asset Setup

- [x] 1.1 Copy pause popup frame/background textures from PinArrow (`F:\PinArrow\Assets\00 Game\Textures\Pin Arrow\Pause\`) to `Assets/Art/UI/` — select relevant frame assets (popup background, title image)
- [x] 1.2 Configure imported textures as Sprite (2D and UI) in Unity import settings

## 2. GameManager Updates

- [x] 2.1 Add `Paused` to `GameState` enum in `GameManager.cs`
- [x] 2.2 Add `popupPausePrefab` serialized field to GameManager
- [x] 2.3 Implement `Pause()` method — guard State == Playing, set State to Paused, set Time.timeScale = 0, instantiate popupPausePrefab
- [x] 2.4 Implement `Resume()` method — guard State == Paused, set Time.timeScale = 1, set State to Playing
- [x] 2.5 Add `Time.timeScale = 1` to Replay(), NextLevel(), and Home() before scene reload

## 3. PopupPause UI

- [x] 3.1 Create `Assets/Scripts/UI/PopupPause.cs` — extends PopupBase, references buttonResume and buttonHome, wires callbacks to GameManager.Resume() and GameManager.Home() via CloseAndAction
- [x] 3.2 Add PopupPause prefab builder to `UIPopupBuilder.cs` — Canvas (sort 100), dark overlay, panel, "PAUSED" title text, Resume button (green), Home button (blue), attach PopupPause.cs
- [x] 3.3 Build PopupPause prefab by running the builder menu item

## 4. HUD Pause Button

- [x] 4.1 Add `pauseButton` field to `UIIngame.cs` and wire it to call `GameManager.Instance.Pause()` in Start()
- [x] 4.2 Add pause button to HUD setup in `UIPopupBuilder.SetupScene()` — top-left position using `button_pause.png` sprite, with ButtonPressEffect

## 5. Scene Integration

- [x] 5.1 Assign popupPausePrefab on GameManager in scene setup
- [x] 5.2 Run Setup Game Scene menu item to update HUD with pause button
- [x] 5.3 Verify no compilation errors in Unity console

## 6. Verification

- [x] 6.1 Play test: tap pause button → game freezes, PopupPause appears with animation
- [x] 6.2 Play test: tap Resume → popup closes, game resumes at timeScale 1
- [x] 6.3 Play test: tap Home from pause → returns to level 0
