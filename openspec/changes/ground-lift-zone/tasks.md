## 1. Tạo GroundLiftZone.cs

- [x] 1.1 Tạo file `Assets/Scripts/Ground/GroundLiftZone.cs` với fields: `liftForceY`, `zoneHeight`, `zoneWidth`, `playerLayer`
- [x] 1.2 Implement `Awake()`: đọc `BoxCollider2D` trên GameObject, nếu có thì gán `zoneWidth = bounds.size.x`
- [x] 1.3 Implement `Update()`: khi `Input.GetMouseButtonDown(0)` → gọi `TryLift()`
- [x] 1.4 Implement `TryLift()`: dùng `Physics2D.OverlapBoxAll` tại zone center (ground top + zoneHeight/2), size `(zoneWidth, zoneHeight)`, filter theo `playerLayer` → dedup roots bằng HashSet → apply `AddForce((0, liftForceY), Impulse)` lên tất cả Rigidbody2D trong mỗi root
- [x] 1.5 Implement `OnDrawGizmosSelected()`: vẽ WireBox xanh tại zone bounds

## 2. Cập nhật Scene và Prefab

- [x] 2.1 Tạo thư mục `Assets/Scripts/Ground/` nếu chưa có
- [x] 2.2 Xóa LiftPoint child GameObject khỏi `StickmanRagdollV2.prefab`
- [ ] 2.3 Thêm `GroundLiftZone` component vào Ground object trong SampleScene, gán `playerLayer`, tune `liftForceY` và `zoneHeight`

## 3. Kiểm tra trong Unity Editor

- [ ] 3.1 Import script, kiểm tra không có lỗi compile
- [ ] 3.2 Kiểm tra gizmo WireBox hiển thị đúng kích thước ground
- [ ] 3.3 Play mode: tap khi đứng trong zone → toàn bộ ragdoll nhận lực nâng
- [ ] 3.4 Play mode: tap khi nhảy cao ra ngoài zone → không có lực
