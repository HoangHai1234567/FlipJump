## MODIFIED Requirements

### Requirement: ForcePoint apply lực nâng và spin khi tap
Khi player tap (`Input.GetMouseButtonDown(0)`) và `isGrounded`, `ForcePoint` SHALL gọi `UnfreezeAll()`, apply `bodyLiftForceY` lên `bodyLiftRigidbody` nếu `bodyLiftEnabled`, và apply `spinTorque` lên `bodyLiftRigidbody` nếu `spinTorque != 0`. `ForcePoint` SHALL KHÔNG apply lực ngang (`forceX`) — lực ngang được xử lý bởi `PushForceZone`.

#### Scenario: Tap khi grounded — apply lift và spin
- **WHEN** player tap và `isGrounded = true`
- **THEN** `bodyLiftRigidbody` nhận `bodyLiftForceY` (nếu enabled) và `spinTorque` (nếu != 0); không có lực ngang nào được apply từ ForcePoint
