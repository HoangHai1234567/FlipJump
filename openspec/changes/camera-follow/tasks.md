## 1. Tạo CameraFollow.cs

- [x] 1.1 Tạo thư mục `Assets/Scripts/Camera/` (nếu chưa có)
- [x] 1.2 Tạo script `CameraFollow.cs` với fields: `public Transform target`, `public float offsetX = 0f`, `public float smoothTime = 0.3f`
- [x] 1.3 Implement `LateUpdate()`: null-check target, dùng `Mathf.SmoothDamp` để move camera.x về `target.position.x + offsetX`, giữ nguyên Y và Z

## 2. Kiểm tra trong Unity Editor

- [ ] 2.1 Import script, kiểm tra không có lỗi compile
- [ ] 2.2 Gắn `CameraFollow` component vào Main Camera
- [ ] 2.3 Gán Body Transform của StickmanRagdoll vào field `target`
- [ ] 2.4 Chạy Play mode, tap chuột — camera theo nhân vật smooth
- [ ] 2.5 Kiểm tra fallback: xóa `target` → không có NullReferenceException
