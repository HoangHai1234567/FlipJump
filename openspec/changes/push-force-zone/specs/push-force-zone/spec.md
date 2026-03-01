## ADDED Requirements

### Requirement: PushForceZone phát lực đẩy ngang khi tap
`PushForceZone` SHALL là một MonoBehaviour gắn lên world object. Khi `Input.GetMouseButtonDown(0)`, SHALL dùng `Physics2D.OverlapBoxAll` tại `transform.position` với size `(zoneWidth, zoneHeight)` và `playerLayer` mask. Với mỗi root tìm được (dedup bằng HashSet), SHALL apply `AddForce(new Vector2(forceX, 0f), ForceMode2D.Impulse)` lên tất cả Rigidbody2D trong root.

#### Scenario: Player trong zone — lực được apply
- **WHEN** player tap và ít nhất một Collider2D của ragdoll nằm trong PushForceZone
- **THEN** tất cả Rigidbody2D của ragdoll nhận impulse `(forceX, 0)`

#### Scenario: Player ngoài zone — không apply lực
- **WHEN** player tap nhưng không có Collider2D nào của ragdoll trong zone
- **THEN** không có force nào được apply

### Requirement: Gizmo visualize zone
`PushForceZone` SHALL vẽ `OnDrawGizmosSelected()` WireBox màu vàng tại `transform.position` với size `(zoneWidth, zoneHeight)`.

#### Scenario: Gizmo hiển thị zone
- **WHEN** PushForceZone GameObject được chọn trong Scene view
- **THEN** WireBox vàng hiển thị đúng kích thước zone
