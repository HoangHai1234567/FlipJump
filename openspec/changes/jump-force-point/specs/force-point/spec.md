## ADDED Requirements

### Requirement: ForcePoint có thể cấu hình hướng và độ lớn lực
Script `ForcePoint` SHALL expose hai field trong Inspector:
- `forceDirection` (Vector2): hướng phát lực, được normalize trước khi dùng
- `forceMagnitude` (float): độ lớn lực tính bằng đơn vị physics

#### Scenario: Inspector hiển thị các field cấu hình
- **WHEN** ForcePoint object được select trong Hierarchy
- **THEN** Inspector hiển thị `forceDirection` và `forceMagnitude` có thể chỉnh số trực tiếp

### Requirement: Gizmo hiển thị mũi tên lực trong Scene view
`ForcePoint` SHALL vẽ một mũi tên Gizmo luôn hiển thị trong Scene view, bắt đầu từ vị trí của ForcePoint object, theo hướng `forceDirection` với độ dài tỉ lệ thuận với `forceMagnitude`.

#### Scenario: Gizmo hiển thị khi không select
- **WHEN** game không đang chạy và ForcePoint tồn tại trong scene
- **THEN** mũi tên màu vàng nhạt hiển thị hướng và độ lớn lực

#### Scenario: Gizmo sáng hơn khi select
- **WHEN** ForcePoint object được select trong Editor
- **THEN** mũi tên đổi sang màu vàng sáng và hiện thêm handle tại đầu mũi tên

### Requirement: Handle kéo được để chỉnh lực trực tiếp trong Scene
Khi ForcePoint được select, SHALL hiển thị một handle (điểm tương tác) tại đầu mũi tên lực. Kéo handle SHALL cập nhật cả `forceDirection` và `forceMagnitude` theo vị trí chuột.

#### Scenario: Kéo handle thay đổi direction và magnitude
- **WHEN** user kéo handle tại đầu mũi tên trong Scene view
- **THEN** `forceDirection` cập nhật theo hướng từ ForcePoint đến vị trí chuột
- **THEN** `forceMagnitude` cập nhật theo khoảng cách từ ForcePoint đến vị trí chuột
- **THEN** mũi tên Gizmo cập nhật real-time trong khi kéo

### Requirement: ApplyForce apply impulse lên target Rigidbody2D
`ForcePoint` SHALL có public method `ApplyForce()` áp dụng `ForceMode2D.Impulse` lên `targetRigidbody` với vector = `forceDirection.normalized * forceMagnitude`.

#### Scenario: Gọi ApplyForce khi targetRigidbody được assign
- **WHEN** `ApplyForce()` được gọi và `targetRigidbody` không null
- **THEN** targetRigidbody nhận impulse force theo hướng và độ lớn đã cấu hình

#### Scenario: Gọi ApplyForce khi targetRigidbody là null
- **WHEN** `ApplyForce()` được gọi và `targetRigidbody` là null
- **THEN** không có exception xảy ra (log warning thay vì crash)

### Requirement: ForcePoint là child object của StickmanRagdoll
Object `ForcePoint` SHALL là child của root `StickmanRagdoll` trong prefab. Vị trí của ForcePoint SHALL là trung tâm Body (0, 0 local) để lực apply tại điểm cân bằng.

#### Scenario: ForcePoint tồn tại trong prefab
- **WHEN** prefab StickmanRagdoll được kéo vào scene
- **THEN** có child object tên `ForcePoint` với script `ForcePoint.cs` được gắn
- **THEN** `targetRigidbody` được assign sẵn vào Body's Rigidbody2D
