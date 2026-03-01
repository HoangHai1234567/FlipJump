## Why

`ForcePoint` hiện tại xử lý spin + body lift — hai mục đích khác nhau trên cùng một script. Để tách biệt hoàn toàn lực nâng và dễ tune độc lập, cần một script riêng `LiftPoint.cs` chỉ chuyên apply lực thẳng đứng lên toàn bộ ragdoll khi tap, giống cách ForcePoint hoạt động nhưng đơn giản hơn.

## What Changes

- Tạo script mới `LiftPoint.cs` — khi tap (grounded), apply `(0, liftForceY)` lên tất cả Rigidbody2D con của ragdoll root
- Thêm GameObject `LiftPoint` vào prefab `StickmanRagdollV2` với component này
- `bodyLiftEnabled` trên ForcePoint V2 có thể tắt đi (lift nay do LiftPoint đảm nhiệm)

## Capabilities

### New Capabilities
- `lift-point`: Script độc lập apply lực nâng đồng đều lên toàn bộ parts của ragdoll khi tap

### Modified Capabilities

## Impact

- Tạo `Assets/Scripts/Ragdoll/LiftPoint.cs`
- Thêm GameObject + component vào `StickmanRagdollV2.prefab`
- Optionally tắt `bodyLiftEnabled` trên ForcePoint V2
