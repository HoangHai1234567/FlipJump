## Why

Hiện tại camera cố định, không theo dõi nhân vật khi di chuyển, khiến nhân vật ra khỏi màn hình sau mỗi lần nhảy. Cần camera follow nhân vật để người chơi luôn thấy nhân vật trên màn hình.

## What Changes

- Thêm script `CameraFollow.cs` gắn vào Main Camera
- Camera theo dõi vị trí của một Transform được chỉ định (Body của StickmanRagdoll)
- Camera chỉ di chuyển theo trục X (ngang), Y và Z cố định
- Smooth damping để camera di chuyển mượt, không giật
- Có thể cấu hình offsetX và damping time trong Inspector

## Capabilities

### New Capabilities
- `camera-follow`: Camera theo dõi nhân vật với smooth damping, cấu hình offset và tốc độ follow

### Modified Capabilities

## Impact

- Thêm script mới `Assets/Scripts/Camera/CameraFollow.cs`
- Gắn vào Main Camera trong scene
- Kéo thả Body Transform của StickmanRagdoll vào field `target`
