## ADDED Requirements

### Requirement: Tìm leaf limb ở bên trái nhất
Khi tap, hệ thống SHALL tìm Transform trong mảng `leafLimbs` có `position.x` nhỏ nhất (world space) và trả về Rigidbody2D của Transform đó. Nếu mảng rỗng hoặc null, SHALL fallback về Body Rigidbody2D.

#### Scenario: Tìm đúng limb bên trái nhất
- **WHEN** `leafLimbs` có nhiều Transforms với các vị trí X khác nhau
- **THEN** hệ thống chọn Transform có `position.x` nhỏ nhất

#### Scenario: Fallback khi mảng rỗng
- **WHEN** `leafLimbs` là null hoặc rỗng
- **THEN** hệ thống dùng Body Rigidbody2D làm target, không crash

#### Scenario: Apply lực tạo spin
- **WHEN** lực được apply vào leaf limb lệch tâm so với Body
- **THEN** nhân vật tịnh tiến theo (forceX, forceY) và đồng thời xoay vòng tự nhiên do torque

### Requirement: Inspector có thể cấu hình danh sách leaf limbs
`ForcePoint` SHALL expose `public Transform[] leafLimbs` trong Inspector để designer chỉ định các bộ phận đầu cuối tham gia selection.

#### Scenario: Inspector hiển thị mảng leafLimbs
- **WHEN** ForcePoint GameObject được chọn trong Hierarchy
- **THEN** Inspector hiển thị field `leafLimbs` dạng array có thể drag-drop các GameObject vào
