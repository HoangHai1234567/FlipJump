## ADDED Requirements

### Requirement: Lực lift tỉ lệ nghịch với khoảng cách Y đến mặt đất
`LiftPoint` SHALL cast `Physics2D.Raycast` từ `transform.position` hướng `Vector2.down` với distance tối đa `maxLiftHeight` và `groundLayer` mask. Nếu hit, SHALL tính `liftScale = 1f - Mathf.Clamp01(hit.distance / maxLiftHeight)` rồi apply `liftForceY * liftScale` lên tất cả Rigidbody2D. Nếu không hit, SHALL không apply lực.

#### Scenario: Trên mặt đất — lực tối đa
- **WHEN** raycast hit.distance ≈ 0 (LiftPoint chạm đất)
- **THEN** `liftScale ≈ 1.0` → apply `liftForceY * 1.0`

#### Scenario: Giữa không trung — lực giảm tuyến tính
- **WHEN** raycast hit.distance = `maxLiftHeight / 2`
- **THEN** `liftScale = 0.5` → apply `liftForceY * 0.5`

#### Scenario: Trên `maxLiftHeight` — không apply lực
- **WHEN** raycast không hit bất kỳ collider nào trong `maxLiftHeight`
- **THEN** không có force nào được apply; không crash

### Requirement: Inspector có thể cấu hình maxLiftHeight
`LiftPoint` SHALL expose `public float maxLiftHeight = 3f`. Gizmo SHALL vẽ line downward từ LiftPoint theo chiều dài `maxLiftHeight` để visualize range.

#### Scenario: Gizmo hiển thị lift range
- **WHEN** LiftPoint GameObject được chọn trong Scene view
- **THEN** line đỏ hoặc xanh dài `maxLiftHeight` hướng xuống hiển thị trong Scene
