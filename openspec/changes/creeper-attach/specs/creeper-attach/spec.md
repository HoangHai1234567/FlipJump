## ADDED Requirements

### Requirement: Creeper bám vào player khi va chạm
Khi player chạm vào trigger collider của creeper, creeper PHẢI gắn player vào AttachPoint rồi di chuyển theo cung tròn từ điểm A đến điểm B.

#### Scenario: Player chạm creeper
- **WHEN** player va vào CircleCollider2D (trigger) của creeper_1
- **THEN** player gắn vào AttachPoint và creeper bắt đầu đưa player từ điểm A đến điểm B trên cung tròn (tâm Pivot, bán kính = khoảng cách Pivot→AttachPoint)

#### Scenario: Đến điểm B
- **WHEN** creeper di chuyển đến điểm B
- **THEN** player được thả ra (detach), không còn gắn vào creeper

#### Scenario: Creeper đang chở player
- **WHEN** creeper đã attached và đang di chuyển từ A→B
- **THEN** trigger collision mới KHÔNG tạo thêm attachment

### Requirement: Điểm A và B điều chỉnh được trên đường tròn
A và B PHẢI nằm trên đường tròn tâm Pivot, bán kính = chiều dài creeper. Có thể kéo thả A, B trong Editor.

#### Scenario: Điều chỉnh A trong Editor
- **WHEN** kéo handle A trong Scene view
- **THEN** A PHẢI luôn nằm trên đường tròn (snap về circle), lưu dưới dạng góc (angle)

#### Scenario: Điều chỉnh B trong Editor
- **WHEN** kéo handle B trong Scene view
- **THEN** B PHẢI luôn nằm trên đường tròn (snap về circle), lưu dưới dạng góc (angle)

### Requirement: Gizmo hiển thị trong Editor
Script PHẢI vẽ Gizmo để thấy đường tròn, điểm A, điểm B và cung di chuyển.

#### Scenario: Chọn creeper trong Scene
- **WHEN** creeper được chọn trong Editor
- **THEN** hiển thị: đường tròn (wire circle) tại Pivot, điểm A (xanh), điểm B (đỏ), cung từ A→B
