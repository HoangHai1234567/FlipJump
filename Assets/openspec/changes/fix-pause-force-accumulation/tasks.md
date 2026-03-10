## 1. Lock input during pause

- [x] 1.1 In `GameManager.Pause()`, set `InputGate.locked = true` immediately after setting `State = GameState.Paused`
- [x] 1.2 In `GameManager.Resume()`, set `InputGate.locked = false` immediately after setting `State = GameState.Playing`

## 2. Fix missing InputGate check

- [x] 2.1 Add `if (InputGate.locked) return;` guard to `LiftPoint.Update()` before the `Input.GetMouseButtonDown(0)` check

## 3. Verify

- [ ] 3.1 Play-test: pause the game, tap multiple times, resume — character should not receive accumulated forces
- [ ] 3.2 Play-test: after resuming, tap should apply force normally
- [ ] 3.3 Verify win/lose input locking still works correctly (head collision and win zone)
