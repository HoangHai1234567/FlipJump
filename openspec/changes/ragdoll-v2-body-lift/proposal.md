## Why

Prefab hiện tại chỉ apply lực lệch tâm vào leaf limb tạo spin, nhưng thiếu lực đẩy thẳng đứng vào Body khiến nhân vật không nảy đủ cao. Cần một variant V2 bổ sung lực thẳng đứng (body lift) lên Body cùng lúc với lực spin, tạo hiệu ứng nhảy + xoay tự nhiên hơn.

## What Changes

- Tạo prefab mới `StickmanRagdollV2.prefab` — bản sao của `StickmanRagdoll.prefab` với tất cả tính năng hiện tại
- Thêm fields vào `ForcePoint.cs`: `bodyLiftEnabled`, `bodyLiftRigidbody`, `bodyLiftForceY` (disabled by default — prefab gốc không bị ảnh hưởng)
- Trong `ApplyForce()`: nếu `bodyLiftEnabled`, apply thêm lực `(0, bodyLiftForceY)` lên `bodyLiftRigidbody` (Body)
- Cấu hình V2 prefab: `bodyLiftEnabled = true`, `bodyLiftRigidbody = Body`

## Capabilities

### New Capabilities
- `body-lift-force`: Khi tap, apply thêm lực thẳng đứng `(0, bodyLiftForceY)` lên Body — bổ sung cùng lúc với lực spin vào leaf limb

### Modified Capabilities
- `force-point`: ForcePoint.cs thêm optional body lift fields; `ApplyForce()` có thể apply 2 lực song song

## Impact

- Sửa `Assets/Scripts/Ragdoll/ForcePoint.cs` — thêm 3 fields, sửa `ApplyForce()`
- Tạo `Assets/Prefabs/StickmanRagdollV2.prefab` — duplicate từ prefab gốc
- Prefab gốc `StickmanRagdoll.prefab` không thay đổi hành vi (`bodyLiftEnabled = false`)
