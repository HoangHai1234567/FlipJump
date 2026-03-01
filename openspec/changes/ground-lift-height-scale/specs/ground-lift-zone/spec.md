## MODIFIED Requirements

### Requirement: GroundLiftZone phát lực nâng lên player khi tap
`GroundLiftZone` SHALL gắn lên ground object và khi `Input.GetMouseButtonDown(0)` được nhận, SHALL dùng `Physics2D.OverlapBoxAll` để tìm tất cả Collider2D trong zone. Với mỗi root tìm được, SHALL tính `liftScale = 1f - Mathf.Clamp01((root.position.y - groundTop) / zoneHeight)` và apply `AddForce(new Vector2(0f, liftForceY * liftScale), ForceMode2D.Impulse)` lên tất cả Rigidbody2D trong root.

#### Scenario: Player trong zone — lực được apply với scale
- **WHEN** player tap và ít nhất một Collider2D của ragdoll nằm trong GroundLiftZone
- **THEN** tất cả Rigidbody2D của ragdoll nhận impulse `(0, liftForceY * liftScale)` tùy theo độ cao

#### Scenario: Player ngoài zone — không apply lực
- **WHEN** player tap nhưng không có Collider2D nào của ragdoll trong zone
- **THEN** không có force nào được apply
