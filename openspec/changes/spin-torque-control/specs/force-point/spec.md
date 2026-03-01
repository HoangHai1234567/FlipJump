## MODIFIED Requirements

### Requirement: ForcePoint apply lực vào dynamic target
ForcePoint SHALL không còn dùng `targetRigidbody` cố định. Thay vào đó, mỗi khi `ApplyForce()` được gọi, SHALL gọi `FindLeftmostLeaf()` để lấy Rigidbody2D của leaf limb ở bên trái nhất, rồi apply `new Vector2(forceX, forceY)` với `ForceMode2D.Impulse` lên target đó. Ngoài ra:
- Nếu `bodyLiftEnabled == true`, SHALL apply thêm impulse `(0, bodyLiftForceY)` lên `bodyLiftRigidbody`
- Nếu `spinTorque != 0`, SHALL apply `AddTorque(spinTorque, Impulse)` lên `bodyLiftRigidbody`

#### Scenario: Apply force vào leftmost leaf
- **WHEN** player tap và `isGrounded == true`
- **THEN** ForcePoint tìm leaf limb có X nhỏ nhất, apply impulse `(forceX, forceY)` lên Rigidbody2D của limb đó

#### Scenario: Apply body lift khi enabled
- **WHEN** `bodyLiftEnabled == true` và player tap và `isGrounded == true`
- **THEN** apply thêm impulse `(0, bodyLiftForceY)` lên `bodyLiftRigidbody`

#### Scenario: Apply spin torque khi spinTorque != 0
- **WHEN** `spinTorque != 0` và player tap và `isGrounded == true`
- **THEN** apply `AddTorque(spinTorque, Impulse)` lên `bodyLiftRigidbody`

#### Scenario: Không có leafLimbs — fallback hoạt động
- **WHEN** `leafLimbs` array rỗng và player tap
- **THEN** không có exception; hệ thống vẫn apply force (fallback behavior)
