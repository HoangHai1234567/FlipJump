## ADDED Requirements

### Requirement: LiftPoint apply lực nâng đồng đều lên toàn bộ ragdoll khi tap
Khi player tap (`Input.GetMouseButtonDown(0)`) và `isGrounded == true`, `LiftPoint` SHALL tìm tất cả `Rigidbody2D` trong `transform.root` hierarchy và apply `AddForce(new Vector2(0, liftForceY), ForceMode2D.Impulse)` lên mỗi part. Nếu không có Rigidbody2D nào, SHALL không crash.

#### Scenario: Lift apply lên tất cả parts khi grounded
- **WHEN** player tap và `isGrounded == true`
- **THEN** mọi Rigidbody2D con của root đều nhận impulse `(0, liftForceY)`

#### Scenario: Không lift khi đang trên không
- **WHEN** player tap và `isGrounded == false`
- **THEN** không có force nào được apply

### Requirement: LiftPoint có ground check độc lập
`LiftPoint` SHALL kiểm tra `Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer)` mỗi frame trong `Update()`. Kết quả lưu vào `isGrounded`.

#### Scenario: isGrounded đúng khi chạm đất
- **WHEN** transform.position nằm trong `groundCheckRadius` của collider thuộc `groundLayer`
- **THEN** `isGrounded == true`

### Requirement: Inspector có thể cấu hình LiftPoint
`LiftPoint` SHALL expose: `public float liftForceY`, `public float groundCheckRadius`, `public LayerMask groundLayer`. Gizmo SHALL vẽ wire sphere tại vị trí LiftPoint, màu xanh khi grounded, đỏ khi không.

#### Scenario: Gizmo hiển thị trạng thái
- **WHEN** LiftPoint GameObject được chọn trong Scene view
- **THEN** wire sphere hiển thị với màu tương ứng trạng thái grounded
