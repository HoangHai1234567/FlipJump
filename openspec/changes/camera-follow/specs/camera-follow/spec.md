## ADDED Requirements

### Requirement: Camera smooth follow target theo trục X
Camera SHALL theo dõi một Transform target được chỉ định, sử dụng `Vector3.SmoothDamp` để di chuyển mượt về vị trí X của target + offsetX. Camera SHALL giữ nguyên Y và Z position. Nếu target là null, camera SHALL không di chuyển và không throw exception.

#### Scenario: Camera follow khi target di chuyển ngang
- **WHEN** target Transform thay đổi vị trí theo trục X
- **THEN** camera di chuyển smooth về `target.position.x + offsetX` trên trục X

#### Scenario: Camera giữ Y và Z position
- **WHEN** camera đang follow target
- **THEN** `camera.transform.position.y` và `.z` không thay đổi so với giá trị ban đầu

#### Scenario: Không crash khi target null
- **WHEN** `target` chưa được gán (null)
- **THEN** không có exception; camera đứng yên

### Requirement: Inspector có thể cấu hình follow parameters
`CameraFollow` SHALL expose các fields sau trong Inspector: `target` (Transform), `offsetX` (float), `smoothTime` (float).

#### Scenario: Inspector hiển thị cấu hình
- **WHEN** Main Camera được chọn trong Hierarchy
- **THEN** Inspector hiển thị `target`, `offsetX`, `smoothTime` fields có thể chỉnh sửa

#### Scenario: Thay đổi offsetX dịch camera ngang
- **WHEN** `offsetX` được set thành `2`
- **THEN** camera luôn giữ vị trí X là `target.position.x + 2`
