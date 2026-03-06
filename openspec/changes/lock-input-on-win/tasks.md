## 1. Create InputGate

- [x] 1.1 Create `Assets/Scripts/Core/InputGate.cs` with a static `locked` boolean property (default `false`)

## 2. Wire up gate checks

- [x] 2.1 In `ForcePoint.Update()`, check `InputGate.locked` and return early if true. Remove the instance `inputLocked` field and `LockInput()` method
- [x] 2.2 In `PushForceZone.Update()`, check `InputGate.locked` and return early if true
- [x] 2.3 In `GroundLiftZone.Update()`, check `InputGate.locked` and return early if true

## 3. Update WinZone

- [x] 3.1 In `WinZone.WinSequence()`, set `InputGate.locked = true` as the first line, before calling `FreezeAll()`
