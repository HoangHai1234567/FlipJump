## Why

`LiftPoint` hiện tại apply `liftForceY` cố định chỉ khi chạm đất. Cần cơ chế linh hoạt hơn: lực nâng tỉ lệ nghịch với khoảng cách Y từ LiftPoint đến mặt đất — càng gần đất thì lực càng mạnh, càng xa (càng cao) thì lực càng yếu, đến `maxLiftHeight` thì về 0.

## What Changes

- Thay `isGrounded` check bằng raycast downward: cast xuống từ LiftPoint, đo khoảng cách Y đến mặt đất
- Tính `liftScale = 1 - Clamp01(groundDist / maxLiftHeight)` — on ground = 1.0, at maxHeight = 0.0
- Apply `liftForceY * liftScale` lên tất cả Rigidbody2D
- Thêm field `public float maxLiftHeight` vào `LiftPoint.cs`
- Nếu raycast không hit trong `maxLiftHeight`, không apply lực (hoặc scale = 0)

## Capabilities

### New Capabilities
- `height-scaled-lift`: Lực lift tỉ lệ nghịch với khoảng cách Y đến mặt đất, fade về 0 tại `maxLiftHeight`

### Modified Capabilities
- `lift-point`: `LiftPoint.cs` thay cơ chế trigger — từ `isGrounded` sang raycast-based height scale

## Impact

- Sửa `Assets/Scripts/Ragdoll/LiftPoint.cs` — thay ground check, thêm field, tính scaled force
- Cập nhật `StickmanRagdollV2.prefab` — thêm `maxLiftHeight` value
