## ADDED Requirements

### Requirement: Apply AddTorque lên Body khi tap
Khi `ApplyForce()` được gọi và `spinTorque != 0`, hệ thống SHALL gọi `bodyLiftRigidbody.AddTorque(spinTorque, ForceMode2D.Impulse)`. Lực này SHALL không ảnh hưởng đến `linear velocity` của Body. Nếu `bodyLiftRigidbody` là null, SHALL log warning và bỏ qua (không crash).

#### Scenario: Torque tạo rotation không ảnh hưởng linear velocity
- **WHEN** player tap và `spinTorque != 0`
- **THEN** `Body.angularVelocity` tăng theo `spinTorque`
- **THEN** `Body.linearVelocity` không thay đổi do `spinTorque` (AddTorque không tạo linear force)

#### Scenario: spinTorque = 0 không tạo rotation
- **WHEN** `spinTorque = 0`
- **THEN** không có `AddTorque` call; hành vi giống cũ

#### Scenario: Inspector có thể cấu hình spinTorque
- **WHEN** ForcePoint component được chọn trong Inspector
- **THEN** field `spinTorque` (float) hiện ra và có thể chỉnh; giá trị âm = CW, dương = CCW
