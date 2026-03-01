## Why

Trò chơi FlipJump yêu cầu nhân vật stickman có thể ragdoll khi nhảy qua chướng ngại vật. Cần thiết lập hệ thống ragdoll physics 2D cho nhân vật ngay từ đầu để làm nền tảng cho toàn bộ gameplay.

## What Changes

- Copy nhân vật Player (stickman) từ project tham khảo `D:\TestRagdoll\Stickman-Ragdoll-Tutorial` sang project FlipJump
- Copy các script ragdoll (`Balance.cs`, `IgnoreCollision.cs`) sang project FlipJump
- **Loại bỏ** script `Movement.cs` và toàn bộ logic di chuyển/input
- Tạo Prefab `StickmanRagdoll` sẵn sàng để tích hợp vào gameplay

## Capabilities

### New Capabilities
- `stickman-ragdoll-physics`: Hệ thống ragdoll physics 2D cho nhân vật stickman, bao gồm cân bằng thân thể (Balance), ngăn va chạm nội bộ giữa các limb (IgnoreCollision), và cấu trúc hierarchy đầy đủ (Head, Body, Arms, Legs)

### Modified Capabilities

## Impact

- Thêm thư mục `Assets/Scripts/Ragdoll/` trong project FlipJump
- Thêm thư mục `Assets/Prefabs/` chứa prefab `StickmanRagdoll`
- Thêm assets đồ họa stickman từ project tham khảo
- Không ảnh hưởng đến các hệ thống khác (chưa có hệ thống nào)
