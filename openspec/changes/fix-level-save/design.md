## Bối cảnh

`LevelLoader.SpawnElement()` gọi `Instantiate(prefab)` → prefab gốc có collider kích thước lớn → player va chạm ngay → `GroundLanding.cs` gọi `FreezeAll()` hoặc `HeadCollision.cs` gọi `GameManager.Lose()`.

Sau đó mới `ApplyComponents()` và `ApplySplinePoints()` thay đổi kích thước/hình dạng collider, nhưng đã quá muộn.

## Mục tiêu / Không phải mục tiêu

**Mục tiêu:**
- Level elements phải được apply đầy đủ dữ liệu JSON trước khi có thể va chạm với player
- Player không bị ảnh hưởng bởi physics trong lúc load

**Không phải mục tiêu:**
- Loading screen UI (quá phức tạp cho bug này)
- Thay đổi flow của editor

## Quyết định

**1. Disable collider trước khi spawn, enable sau khi apply**

Trong `SpawnElement()`:
1. Sau `Instantiate()` → tắt tất cả `Collider2D` trên object
2. Apply components + spline
3. Bật lại collider

Đây là cách đơn giản nhất, không cần coroutine hay loading screen.

**2. Freeze player trong Start, unfreeze sau khi load xong**

Trong `LevelLoader.Start()`:
1. Freeze player (dùng `ForcePoint.FreezeAll()`)
2. Load level
3. Đợi 1 frame (coroutine `yield return null`) để Unity cập nhật collider
4. Unfreeze player

Lý do đợi 1 frame: `SpriteShapeController` và `PolygonCollider2D` cần 1 frame để rebuild mesh sau khi thay đổi spline.

**Phương án bị loại:** Loading screen — quá nặng cho 1 bug đơn giản. Chỉ cần disable/enable collider là đủ.

## Rủi ro

- [Collider chưa rebuild sau 1 frame] Nếu SpriteShapeController cần nhiều hơn 1 frame → tăng lên 2 frame hoặc dùng `WaitForFixedUpdate`
- [Player bị rơi trong lúc freeze] Player đã frozen (constraints = FreezeAll) nên không rơi
