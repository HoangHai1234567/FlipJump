## ADDED Requirements

### Requirement: Lực lift tỉ lệ nghịch với khoảng cách Y đến mặt đất
Với mỗi ragdoll root tìm được trong zone, `GroundLiftZone` SHALL tính `heightAboveGround = root.position.y - groundTop` và `liftScale = 1f - Mathf.Clamp01(heightAboveGround / zoneHeight)`. SHALL apply `liftForceY * liftScale` lên tất cả Rigidbody2D trong root đó.

#### Scenario: Player sát mặt đất — lực tối đa
- **WHEN** ragdoll root ở vị trí gần hoặc bằng `groundTop`
- **THEN** `liftScale ≈ 1.0` → apply `liftForceY * 1.0`

#### Scenario: Player giữa zone — lực giảm tuyến tính
- **WHEN** ragdoll root ở độ cao `zoneHeight / 2` so với ground
- **THEN** `liftScale = 0.5` → apply `liftForceY * 0.5`

#### Scenario: Player tại đỉnh zone — không apply lực
- **WHEN** ragdoll root ở độ cao `>= zoneHeight` so với ground
- **THEN** `liftScale = 0` → không có force nào được apply
