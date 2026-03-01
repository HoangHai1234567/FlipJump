## ADDED Requirements

### Requirement: Stickman có cấu trúc hierarchy đầy đủ
Nhân vật stickman SHALL có cấu trúc phân cấp đầy đủ gồm: Head, Body, UpperLeftArm, LowerLeftArm, UpperRightArm, LowerRightArm, LeftLeg (UpperLeftLeg + LowerLeftLeg), RightLeg (UpperRightLeg + LowerRightLeg). Mỗi phần thân SHALL có Rigidbody2D và Collider2D.

#### Scenario: Stickman hiển thị đúng trong scene
- **WHEN** prefab StickmanRagdoll được đặt vào scene
- **THEN** tất cả các limb xuất hiện đúng vị trí và kết nối với nhau qua joints

### Requirement: Ragdoll phản ứng với gravity
Tất cả các phần thân của stickman SHALL chịu tác động của gravity (Rigidbody2D.gravityScale > 0) và rơi xuống khi không có ground bên dưới.

#### Scenario: Stickman rơi khi không có ground
- **WHEN** stickman được spawn ở trên không trung
- **THEN** tất cả các limb rơi xuống theo gravity

### Requirement: Balance tự động cân bằng các limb
Script `Balance.cs` SHALL được gắn vào các phần thân chính (Body, Head) để giữ nhân vật không bị lật ngược hoàn toàn. Script SHALL dùng Rigidbody2D.MoveRotation với Mathf.LerpAngle để xoay về `targetRotation`.

#### Scenario: Body tự cân bằng khi bị xoay
- **WHEN** body bị external force làm xoay lệch
- **THEN** Balance.cs dần dần xoay body về targetRotation theo thời gian

### Requirement: Các limb không va chạm với nhau
Script `IgnoreCollision.cs` SHALL được gắn vào root Player object để tắt va chạm giữa tất cả các Collider2D con với nhau. Các limb SHALL chỉ va chạm với environment (ground, obstacles), không va chạm với nhau.

#### Scenario: Limb xuyên qua nhau bình thường
- **WHEN** hai limb của cùng stickman chạm vào nhau
- **THEN** không có collision event xảy ra giữa chúng

#### Scenario: Limb va chạm với ground
- **WHEN** limb chạm vào object không phải Player
- **THEN** collision xảy ra bình thường (stickman nằm trên ground)

### Requirement: Prefab không có Movement script
Prefab `StickmanRagdoll` SHALL không chứa script `Movement.cs` hoặc bất kỳ script nào xử lý player input (Input.GetAxis, Input.GetKeyDown, Input.GetButton).

#### Scenario: Stickman không tự di chuyển
- **WHEN** prefab được đặt vào scene và game chạy
- **THEN** stickman không tự di chuyển theo input keyboard/touch
