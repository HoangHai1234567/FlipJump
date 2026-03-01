## MODIFIED Requirements

### Requirement: ForcePoint có thể cấu hình hướng và độ lớn lực
ForcePoint SHALL expose hai field độc lập trong Inspector:
- `forceX` (float): lực ngang, dương = sang phải. Default = 5
- `forceY` (float): lực dọc, dương = lên. Default = 12
- `groundCheckRadius` (float): bán kính overlap circle để detect ground. Default = 0.3

Field cũ `forceDirection` và `forceMagnitude` SHALL bị xóa.

#### Scenario: Inspector hiển thị forceX, forceY riêng biệt
- **WHEN** ForcePoint object được select trong Hierarchy
- **THEN** Inspector hiển thị `forceX`, `forceY`, `groundCheckRadius` có thể chỉnh số

### Requirement: Gizmo hiển thị mũi tên lực trong Scene view
Gizmo SHALL vẽ mũi tên từ vị trí ForcePoint theo vector tổng hợp `(forceX, forceY)`, độ dài tỉ lệ với magnitude của vector. Handle kéo tại đầu mũi tên SHALL cập nhật cả `forceX` và `forceY`.

#### Scenario: Gizmo phản ánh vector (forceX, forceY)
- **WHEN** `forceX = 5` và `forceY = 12`
- **THEN** mũi tên nghiêng lên-phải theo đúng tỉ lệ

#### Scenario: Handle kéo cập nhật forceX và forceY riêng biệt
- **WHEN** user kéo handle đến vị trí mới
- **THEN** `forceX` = delta.x từ ForcePoint đến vị trí handle
- **THEN** `forceY` = delta.y từ ForcePoint đến vị trí handle

#### Scenario: Gizmo hiển thị khi không select
- **WHEN** game không đang chạy và ForcePoint tồn tại trong scene
- **THEN** mũi tên màu vàng nhạt hiển thị hướng và độ lớn lực

#### Scenario: Gizmo sáng hơn khi select
- **WHEN** ForcePoint object được select trong Editor
- **THEN** mũi tên đổi sang màu vàng sáng và hiện thêm handle tại đầu mũi tên
