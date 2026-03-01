## ADDED Requirements

### Requirement: Chỉ nhảy khi đang đứng trên ground
Hệ thống SHALL từ chối apply lực khi `isGrounded == false`. Ground detection SHALL dùng `Physics2D.OverlapCircle` tại vị trí gốc của ForcePoint với radius có thể cấu hình, lọc theo tag `"Ground"`.

#### Scenario: Tap khi đang đứng — nhân vật nhảy
- **WHEN** player tap và `isGrounded == true`
- **THEN** ForcePoint apply impulse `(forceX, forceY)` lên Body Rigidbody2D
- **THEN** tất cả limb Rigidbody2D được unfreeze trước khi apply lực

#### Scenario: Tap khi đang trên không — không có gì xảy ra
- **WHEN** player tap và `isGrounded == false`
- **THEN** không có force nào được apply
- **THEN** nhân vật tiếp tục rơi theo gravity

### Requirement: Nhảy parabol sang phải và lên
Lực apply SHALL có hai thành phần độc lập: `forceX` (horizontal, dương = sang phải) và `forceY` (vertical, dương = lên). Cả hai SHALL được apply cùng lúc dưới dạng `new Vector2(forceX, forceY)` với `ForceMode2D.Impulse`.

#### Scenario: Nhân vật bật lên và sang phải
- **WHEN** `ApplyForce()` được gọi với `forceX > 0` và `forceY > 0`
- **THEN** nhân vật di chuyển theo đường cung parabol sang phải và lên
- **THEN** gravity kéo nhân vật xuống trong khi bay

### Requirement: Landing reset velocity và khóa nhân vật
Khi Body Rigidbody2D va chạm với object có tag `"Ground"`, hệ thống SHALL reset velocity của tất cả Rigidbody2D trong StickmanRagdoll về `Vector2.zero` và freeze constraints để nhân vật đứng yên cho đến lần tap tiếp theo.

#### Scenario: Chạm đất sau khi nhảy
- **WHEN** Body collider va chạm với ground
- **THEN** tất cả Rigidbody2D velocity = Vector2.zero
- **THEN** tất cả Rigidbody2D bị freeze (`FreezeAll`)
- **THEN** `isGrounded` trở thành true

#### Scenario: Nhân vật đứng yên khi chưa tap
- **WHEN** nhân vật đang ở trạng thái grounded và không có input
- **THEN** nhân vật không trượt, không ngã, đứng ở vị trí cũ

### Requirement: Unfreeze khi bắt đầu nhảy
Ngay trước khi apply jump force, hệ thống SHALL unfreeze tất cả Rigidbody2D để ragdoll hoạt động tự do trong khi bay.

#### Scenario: Unfreeze trước khi apply lực
- **WHEN** `ApplyForce()` được gọi và `isGrounded == true`
- **THEN** tất cả Rigidbody2D được set `constraints = RigidbodyConstraints2D.None` trước
- **THEN** sau đó mới apply impulse force lên Body
