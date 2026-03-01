## Why

Gameplay FlipJump cần cơ chế phóng nhân vật (jump) khi player tap màn hình. Thay vì hardcode lực, cần một **Force Point** có thể tùy chỉnh hướng và độ lớn trực tiếp trong Unity Scene Editor thông qua Gizmo, giúp designer dễ dàng điều chỉnh feel của cú jump mà không cần chỉnh code.

## What Changes

- Tạo script `ForcePoint.cs` gắn vào một child object bên trong `StickmanRagdoll`
- Script nhận lực (direction + magnitude) và apply lên Rigidbody2D của Body khi được kích hoạt
- Vẽ Gizmo trong Scene view: mũi tên hiển thị hướng và độ lớn lực, có thể kéo để chỉnh trực tiếp
- Expose các field `forceDirection` (Vector2) và `forceMagnitude` (float) trong Inspector
- Cung cấp public method `ApplyForce()` để gameplay code gọi khi player tap

## Capabilities

### New Capabilities
- `force-point`: Điểm phát lực tùy chỉnh trên stickman — cho phép apply impulse force với hướng và độ lớn có thể chỉnh bằng Gizmo trực tiếp trong Scene Editor

### Modified Capabilities

## Impact

- Thêm `Assets/Scripts/Ragdoll/ForcePoint.cs`
- Cập nhật prefab `StickmanRagdoll` để có child object `ForcePoint`
- Không ảnh hưởng đến `Balance.cs` hay `IgnoreCollision.cs`
