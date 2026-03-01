## Why

`GroundLiftZone` hiện apply lực đồng đều cho tất cả player trong zone. Cần cải tiến để lực tỉ lệ nghịch với khoảng cách từ player đến mặt đất — càng gần đất thì lực càng mạnh, càng lên cao thì lực càng nhẹ, đến đỉnh zone thì về 0.

## What Changes

- `GroundLiftZone.cs`: khi `TryLift()`, tính `liftScale` dựa trên khoảng cách Y từ ragdoll root đến top của ground collider
- Công thức: `liftScale = 1 - Clamp01(heightAboveGround / zoneHeight)`
- Apply `liftForceY * liftScale` thay vì `liftForceY` cố định
- Gizmo cập nhật để visualize scale gradient (tùy chọn)

## Capabilities

### New Capabilities
- `ground-lift-height-scale`: Lực lift trong GroundLiftZone tỉ lệ nghịch với khoảng cách Y của player so với mặt đất

### Modified Capabilities
- `ground-lift-zone`: `TryLift()` đổi từ flat force sang height-scaled force

## Impact

- Sửa `Assets/Scripts/Ground/GroundLiftZone.cs` — thêm scale calculation vào `TryLift()`
