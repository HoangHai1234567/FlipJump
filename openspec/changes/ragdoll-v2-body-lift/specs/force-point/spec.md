## MODIFIED Requirements

### Requirement: ForcePoint apply lực vào dynamic target
ForcePoint SHALL không còn dùng `targetRigidbody` cố định. Thay vào đó, mỗi khi `ApplyForce()` được gọi, SHALL gọi `FindLeftmostLeaf()` để lấy Rigidbody2D của leaf limb ở bên trái nhất, rồi apply `new Vector2(forceX, forceY)` với `ForceMode2D.Impulse` lên target đó. Ngoài ra, nếu `bodyLiftEnabled == true`, SHALL apply thêm impulse `(0, bodyLiftForceY)` lên `bodyLiftRigidbody`.

#### Scenario: Apply force vào leftmost leaf
- **WHEN** player tap và `isGrounded == true`
- **THEN** ForcePoint tìm leaf limb có X nhỏ nhất
- **THEN** apply impulse `(forceX, forceY)` lên Rigidbody2D của limb đó

#### Scenario: Apply body lift khi enabled
- **WHEN** `bodyLiftEnabled == true` và player tap và `isGrounded == true`
- **THEN** apply thêm impulse `(0, bodyLiftForceY)` lên `bodyLiftRigidbody`

#### Scenario: Gizmo hiển thị trên leaf limb được chọn
- **WHEN** game đang chạy (Play mode)
- **THEN** không có yêu cầu gizmo runtime; gizmo editor vẫn vẽ từ vị trí ForcePoint

#### Scenario: Không có leafLimbs — fallback hoạt động
- **WHEN** `leafLimbs` array rỗng và player tap
- **THEN** không có exception; hệ thống vẫn apply force (fallback behavior)
