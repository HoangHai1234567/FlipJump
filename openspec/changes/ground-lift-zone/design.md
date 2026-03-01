## Context

`LiftPoint.cs` hiện gắn vào ragdoll (di chuyển cùng nhân vật) — không phù hợp về mặt vật lý và khó scale theo chiều cao nhảy. Cần thay thế bằng cơ chế: mặt đất phát lực nâng hướng lên trong một zone có kích thước cố định.

## Goals / Non-Goals

**Goals:**
- `GroundLiftZone.cs` gắn trực tiếp lên ground object (fixed in world)
- Zone là một BoxCollider2D trigger với chiều rộng bằng ground collider, chiều cao `zoneHeight` tùy chỉnh
- Khi tap: dùng `Physics2D.OverlapBox` để kiểm tra player có trong zone không → apply `(0, liftForceY)` lên tất cả Rigidbody2D của player ragdoll
- Player được xác định qua `LayerMask playerLayer`

**Non-Goals:**
- Không scale lực theo khoảng cách (flat impulse đồng đều)
- Không tự động resize khi ground thay đổi lúc runtime

## Decisions

**D1: GroundLiftZone gắn lên Ground object, không gắn vào ragdoll**
- Zone cố định trong world → measurement chính xác, không bị sai khi ragdoll di chuyển
- Alternative: giữ LiftPoint trên ragdoll → vẫn không đo được chiều cao nhảy chính xác

**D2: Physics2D.OverlapBox thay vì OnTriggerStay2D**
- Chỉ check khi tap, không cần track state liên tục
- Tránh edge case với trigger enter/exit khi ragdoll deform
- Zone center = ground top edge + zoneHeight/2, size = (groundWidth, zoneHeight)

**D3: Tìm ragdoll root qua Rigidbody2D.transform.root**
- Bất kỳ Rigidbody2D nào trong zone → lấy transform.root → GetComponentsInChildren tất cả Rigidbody2D
- Tránh duplicate: dùng HashSet để chỉ process mỗi root một lần

**D4: Auto-size width từ BoxCollider2D của ground**
- Đọc ground `BoxCollider2D.bounds.size.x` tại `Awake()` để lấy chiều rộng
- Gizmo vẽ zone box để visualize trong Editor

## Risks / Trade-offs

- [Ground không có BoxCollider2D] → fallback về `zoneWidth` manual field
- [Player layer không đúng] → không detect được ragdoll → không apply lực; dễ debug qua gizmo

## Migration Plan

1. Tạo `GroundLiftZone.cs`
2. Thêm component vào Ground object trong scene
3. Xóa LiftPoint khỏi `StickmanRagdollV2.prefab`
