## Why

Game cần thêm obstacle tương tác: khi người chơi chạm vào creeper, nó bám vào điểm nối đầu-thân và xoay lắc quanh tâm Pivot, tạo hiệu ứng dây leo bám vào người chơi.

## What Changes

- Thêm script `CreeperAttach` vào prefab `creeper_1`: khi player va vào trigger collider → creeper gắn vào AttachPoint (điểm nối Head-Body) của người chơi
- Creeper xoay quanh tâm Pivot với bán kính = khoảng cách từ Pivot đến AttachPoint
- Vẽ Gizmo hiển thị Pivot, AttachPoint, và bán kính xoay để dễ điều chỉnh trong editor
- Prefab `creeper_1` đã có sẵn CircleCollider2D (trigger) và child `Pivot`

## Capabilities

### New Capabilities
- `creeper-attach`: Creeper bám vào người chơi khi va chạm, xoay quanh tâm Pivot

### Modified Capabilities

## Impact

- Thêm file: `Scripts/Obstacles/CreeperAttach.cs`
- Sửa prefab: `Prefabs/Demo Prefab/Obstacles/creeper_1.prefab` — gắn script CreeperAttach
- Cần tham chiếu đến player tag hoặc layer để phát hiện va chạm
