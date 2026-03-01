## MODIFIED Requirements

### Requirement: LiftPoint apply lực nâng đồng đều lên toàn bộ ragdoll khi tap
Khi player tap (`Input.GetMouseButtonDown(0)`) và raycast downward hit trong `maxLiftHeight`, `LiftPoint` SHALL tìm tất cả `Rigidbody2D` trong `transform.root` hierarchy và apply `AddForce(new Vector2(0f, liftForceY * liftScale), ForceMode2D.Impulse)` lên mỗi part. `liftScale` được tính từ khoảng cách raycast. Nếu raycast không hit, không apply.

#### Scenario: Lift apply với lực scaled khi tap
- **WHEN** player tap và raycast hit ground trong `maxLiftHeight`
- **THEN** mọi Rigidbody2D nhận impulse `(0, liftForceY * liftScale)` với scale tùy khoảng cách

#### Scenario: Không lift khi quá cao
- **WHEN** player tap nhưng raycast không hit (trên `maxLiftHeight`)
- **THEN** không có force nào được apply

#### Scenario: Gizmo hiển thị trạng thái và range
- **WHEN** LiftPoint được chọn trong Scene view
- **THEN** line downward dài `maxLiftHeight` và wire sphere hiển thị
