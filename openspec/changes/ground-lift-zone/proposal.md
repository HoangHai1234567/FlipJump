## Why

Áp dụng lực nảy lên một bộ phận riêng lẻ của ragdoll là không hợp lý về mặt vật lý. Cần một cơ chế mới: toàn bộ mặt đất trở thành vùng phát lực (lift zone) hướng lên trên, tác động đều lên tất cả Rigidbody2D của player khi có vùng ground bên dưới.

## What Changes

- Tạo script `GroundLiftZone.cs`: một trigger zone phủ toàn bộ chiều rộng của ground collider, phát lực lên khi player tap
- Zone có chiều rộng bằng ground, chiều cao (height) tùy chỉnh qua Inspector
- Khi player tap và có bất kỳ Rigidbody2D nào của player trong zone, apply impulse `(0, liftForceY)` lên tất cả Rigidbody2D của player
- Xóa `LiftPoint.cs` và LiftPoint GameObject khỏi prefab V2 (thay thế bằng GroundLiftZone)

## Capabilities

### New Capabilities
- `ground-lift-zone`: Vùng phát lực gắn với ground, phủ toàn bộ chiều rộng ground, tác động lực nâng lên tất cả Rigidbody2D của player khi tap và player nằm trong zone

### Modified Capabilities

## Impact

- Tạo mới `Assets/Scripts/Ground/GroundLiftZone.cs`
- Cập nhật `StickmanRagdollV2.prefab`: xóa LiftPoint child, không còn dùng `LiftPoint.cs`
- Ground object trong scene cần được thêm `GroundLiftZone` component
