## 1. Asset Setup

- [x] 1.1 Copy UI textures from PinArrow (`F:\PinArrow\Assets\00 Game\Textures\Pin Arrow\end game\`) to `Assets/Art/UI/` — ribbon_victory.png, ribbon_defeat.png, button_rectangle_blue.png, button_rectangle_green.png
- [x] 1.2 Copy HUD button textures from PinArrow (`Ingame/button_restart.png`, `Ingame/button_pause.png`) to `Assets/Art/UI/`
- [x] 1.3 Configure imported textures as Sprite (2D and UI) in Unity import settings
- [x] 1.4 Check if DOTween is in the project; if not, add DOTween (free) or implement coroutine-based tween helpers

## 2. GameManager

- [x] 2.1 Create `Assets/Scripts/Core/GameManager.cs` — singleton MonoBehaviour with State enum (Playing, Won, Lost), Instance property, serialized prefab fields (popupWinPrefab, popupLosePrefab), and uiCanvas reference
- [x] 2.2 Implement `Win()` method — guard against double-call, set State to Won, instantiate popupWinPrefab under uiCanvas
- [x] 2.3 Implement `Lose()` method — guard against double-call, set State to Lost, start coroutine to instantiate popupLosePrefab after 1s delay
- [x] 2.4 Implement `Replay()` — reset InputGate.locked, reload active scene via SceneManager
- [x] 2.5 Implement `NextLevel()` — increment level index, reload scene
- [x] 2.6 Implement `Home()` — reset level index to 0, reload scene
- [x] 2.7 Add level index tracking (static int to persist across scene reloads, or use LevelLoader's existing JSON file list)

## 3. Popup UI Prefabs

- [x] 3.1 Create `Assets/Scripts/UI/PopupWin.cs` — references: ribbon Image, buttonNext, buttonReplay, buttonHome. OnEnable plays entrance animation. Button callbacks call GameManager methods.
- [x] 3.2 Create `Assets/Scripts/UI/PopupLose.cs` — references: ribbon Image, buttonReplay, buttonHome. Same animation pattern as PopupWin.
- [x] 3.3 Create `Assets/Scripts/UI/ButtonPressEffect.cs` — reusable component: on pointer down scale to 0.9, on pointer up scale back to 1 with OutBack ease, then invoke onClick
- [x] 3.4 Build PopupWin prefab in Unity — Canvas (Screen Space Overlay), dark background overlay, ribbon_victory Image, 3 buttons with text labels, attach PopupWin.cs
- [x] 3.5 Build PopupLose prefab in Unity — same structure as PopupWin but with ribbon_defeat Image and 2 buttons (Replay, Home), attach PopupLose.cs

## 4. In-Game HUD

- [x] 4.1 Create `Assets/Scripts/UI/UIIngame.cs` — shows level name text, has restart button, hides when game state is not Playing
- [x] 4.2 Add HUD Canvas to Design scene — Screen Space Overlay, level name TextMeshPro at top-center, restart button at top-right
- [x] 4.3 Wire restart button to GameManager.Replay()

## 5. Integration with Existing Scripts

- [x] 5.1 Modify `WinZone.cs` — at the end of WinSequence coroutine (after spawning celebrate prefab), call `GameManager.Instance.Win()`
- [x] 5.2 Modify `HeadCollision.cs` — after freezing, call `GameManager.Instance.Lose()`
- [x] 5.3 Add GameManager GameObject to Design scene with prefab references assigned
- [x] 5.4 Add EventSystem to scene if not already present

## 6. Verification

- [x] 6.1 Play test: enter WinZone → PopupWin appears with animation → Next/Replay/Home buttons work
- [x] 6.2 Play test: head hits ground → PopupLose appears → Replay/Home buttons work
- [x] 6.3 Play test: HUD shows level name, restart button works during gameplay
- [x] 6.4 Verify no compilation errors in Unity console
