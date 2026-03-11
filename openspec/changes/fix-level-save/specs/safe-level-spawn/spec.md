## ADDED Requirements

### Requirement: Collider bị tắt trong khi apply dữ liệu
Khi spawn một level element, hệ thống PHẢI tắt tất cả `Collider2D` trên object ngay sau `Instantiate()` và chỉ bật lại sau khi đã apply xong components và spline data.

#### Scenario: Terrain prefab lớn hơn dữ liệu JSON
- **WHEN** terrain prefab gốc có collider lớn và level JSON chứa spline nhỏ hơn
- **THEN** player KHÔNG va chạm với collider prefab gốc vì collider bị tắt cho đến khi spline đã được apply

#### Scenario: Object không có spline data
- **WHEN** một level element không có splinePoints trong JSON
- **THEN** collider vẫn bị tắt rồi bật lại sau khi apply components

### Requirement: Player bị freeze trong lúc load level
Hệ thống PHẢI freeze player (tắt physics) trước khi bắt đầu spawn level elements, và unfreeze sau khi tất cả elements đã spawn và apply xong + đợi ít nhất 1 frame.

#### Scenario: Load level bình thường
- **WHEN** LevelLoader bắt đầu load level
- **THEN** player bị freeze, level spawn + apply, đợi 1 frame, player unfreeze

#### Scenario: Player không rơi trong lúc load
- **WHEN** player đang freeze trong lúc load
- **THEN** player giữ nguyên vị trí, không bị gravity kéo xuống
