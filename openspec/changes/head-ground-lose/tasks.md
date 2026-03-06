## 1. Create HeadCollision script

- [x] 1.1 Create `Assets/Scripts/Ragdoll/HeadCollision.cs` with `OnCollisionEnter2D` that checks for "Ground" tag, guards with `InputGate.locked` and a `triggered` flag, then sets `InputGate.locked = true` and calls `ForcePoint.FreezeAll()` on the root player

## 2. Attach to prefab

- [x] 2.1 Add `HeadCollision` component to the Head child of StickmanRagdollV2 prefab
