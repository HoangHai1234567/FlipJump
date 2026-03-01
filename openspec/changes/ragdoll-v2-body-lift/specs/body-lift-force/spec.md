## ADDED Requirements

### Requirement: Apply thêm lực thẳng đứng vào Body khi tap
Khi `bodyLiftEnabled == true` và `ApplyForce()` được gọi, hệ thống SHALL apply thêm một impulse `(0, bodyLiftForceY)` lên `bodyLiftRigidbody` (Body) ngay sau lực spin vào leaf limb. Nếu `bodyLiftRigidbody` là null, SHALL log warning và bỏ qua body lift (không crash).

#### Scenario: Body lift apply cùng lúc với spin force
- **WHEN** `bodyLiftEnabled = true` và player tap trên ground
- **THEN** `bodyLiftRigidbody.AddForce(new Vector2(0, bodyLiftForceY), Impulse)` được gọi trong cùng frame với lực spin

#### Scenario: Không có body lift khi disabled
- **WHEN** `bodyLiftEnabled = false` và player tap
- **THEN** chỉ lực spin vào leaf limb được apply; body lift không xảy ra

#### Scenario: Không crash khi bodyLiftRigidbody null
- **WHEN** `bodyLiftEnabled = true` nhưng `bodyLiftRigidbody` chưa gán
- **THEN** warning được log; lực spin vẫn apply bình thường; không có exception

### Requirement: Prefab V2 có body lift được bật sẵn
`StickmanRagdollV2.prefab` SHALL là bản sao của `StickmanRagdoll.prefab` với `bodyLiftEnabled = true` và `bodyLiftRigidbody` trỏ đến Body Rigidbody2D.

#### Scenario: V2 prefab hoạt động đúng khi instantiate
- **WHEN** `StickmanRagdollV2` được kéo vào scene và player tap
- **THEN** nhân vật nhảy lên với cả spin và body lift; các tính năng hiện tại (leftmost-limb, grounded guard, landing freeze) vẫn hoạt động

#### Scenario: Prefab gốc không thay đổi
- **WHEN** `StickmanRagdoll` (prefab gốc) được dùng trong scene
- **THEN** hành vi giống hệt trước thay đổi này (bodyLiftEnabled = false)
