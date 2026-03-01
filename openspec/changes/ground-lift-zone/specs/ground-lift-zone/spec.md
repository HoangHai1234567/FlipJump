## ADDED Requirements

### Requirement: GroundLiftZone phát lực nâng lên player khi tap
`GroundLiftZone` SHALL gắn lên ground object và khi `Input.GetMouseButtonDown(0)` được nhận, SHALL dùng `Physics2D.OverlapBox` để tìm tất cả Collider2D trong zone. Với mỗi Collider2D thuộc `playerLayer`, SHALL lấy `transform.root`, thu thập tất cả `Rigidbody2D` trong root hierarchy (dedup bằng HashSet), và apply `AddForce(new Vector2(0f, liftForceY), ForceMode2D.Impulse)` lên mỗi Rigidbody2D.

#### Scenario: Player trong zone — lực được apply
- **WHEN** player tap và ít nhất một Collider2D của ragdoll nằm trong GroundLiftZone
- **THEN** tất cả Rigidbody2D của ragdoll nhận impulse `(0, liftForceY)`

#### Scenario: Player ngoài zone — không apply lực
- **WHEN** player tap nhưng không có Collider2D nào của ragdoll trong zone
- **THEN** không có force nào được apply

### Requirement: Zone tự động lấy chiều rộng từ ground BoxCollider2D
`GroundLiftZone` SHALL tại `Awake()` đọc `BoxCollider2D` trên cùng GameObject. Nếu tìm thấy, SHALL dùng `bounds.size.x` làm chiều rộng zone. Nếu không tìm thấy, SHALL dùng `zoneWidth` field manual.

#### Scenario: Ground có BoxCollider2D
- **WHEN** GroundLiftZone được khởi tạo và ground có BoxCollider2D
- **THEN** zone width = BoxCollider2D.bounds.size.x

#### Scenario: Ground không có BoxCollider2D
- **WHEN** GroundLiftZone được khởi tạo và không tìm thấy BoxCollider2D
- **THEN** zone width = giá trị `zoneWidth` trong Inspector

### Requirement: Gizmo visualize zone trong Editor
`GroundLiftZone` SHALL vẽ `OnDrawGizmosSelected()` một WireBox xanh thể hiện vùng lift zone (chiều rộng × zoneHeight, tính từ top của ground lên).

#### Scenario: Gizmo hiển thị zone
- **WHEN** GroundLiftZone GameObject được chọn trong Scene view
- **THEN** WireBox xanh hiển thị đúng kích thước và vị trí của zone
