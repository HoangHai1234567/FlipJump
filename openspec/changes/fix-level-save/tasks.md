## 1. Disable collider khi spawn

- [x] 1.1 Trong `SpawnElement()`, sau `Instantiate()` → tắt tất cả `Collider2D` trên object (`SetCollidersEnabled(go, false)`)
- [x] 1.2 Sau khi apply components + spline → bật lại collider (`SetCollidersEnabled(go, true)`)
- [x] 1.3 Tạo helper method `SetCollidersEnabled(GameObject go, bool enabled)`

## 2. Freeze player trong lúc load

- [x] 2.1 Trong `LoadV2()`, freeze player trước khi spawn elements (dùng `ForcePoint.FreezeAll()`)
- [x] 2.2 Chuyển phần load sang coroutine, sau khi spawn xong → `yield return null` (đợi 1 frame)
- [x] 2.3 Sau khi đợi 1 frame → unfreeze player

## 3. Test

- [ ] 3.1 Play level với terrain prefab lớn — player không chết ngay khi spawn
- [ ] 3.2 Player vẫn ở đúng vị trí sau khi load xong
