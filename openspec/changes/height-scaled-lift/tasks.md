## 1. Sửa LiftPoint.cs

- [x] 1.1 Thêm field `public float maxLiftHeight = 3f`
- [x] 1.2 Xóa `isGrounded` field và `CheckGrounded()` method
- [x] 1.3 Thay `Update()`: dùng `Physics2D.Raycast(transform.position, Vector2.down, maxLiftHeight, groundLayer)` để lấy hit; nếu `Input.GetMouseButtonDown(0)` và hit → tính `liftScale = 1f - Mathf.Clamp01(hit.distance / maxLiftHeight)` → gọi `LiftAll(liftScale)`
- [x] 1.4 Sửa `LiftAll()` nhận param `float scale`: apply `liftForceY * scale` thay vì `liftForceY`
- [x] 1.5 Cập nhật `OnDrawGizmos()` / `OnDrawGizmosSelected()`: vẽ thêm line downward dài `maxLiftHeight` (xanh khi hit, đỏ khi không)

## 2. Cập nhật StickmanRagdollV2.prefab

- [x] 2.1 Trong LiftPoint MonoBehaviour của V2: thêm `maxLiftHeight: 3`

## 3. Kiểm tra trong Unity Editor

- [ ] 3.1 Import script, kiểm tra không có lỗi compile
- [ ] 3.2 Kiểm tra Inspector LiftPoint: thấy `maxLiftHeight`; gizmo vẽ line downward
- [ ] 3.3 Play mode: tap trên mặt đất → lực mạnh; tap sau khi nhảy (gần đất) → lực vừa; tap cao → không có lực
- [ ] 3.4 Tune `maxLiftHeight` (3 = ~3 units cao cho phép tap); tune `liftForceY` cho cảm giác mong muốn
