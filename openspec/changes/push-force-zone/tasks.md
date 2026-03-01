## 1. Tạo PushForceZone.cs

- [x] 1.1 Tạo `Assets/Scripts/Ground/PushForceZone.cs` với fields: `forceX`, `zoneWidth`, `zoneHeight`, `playerLayer`
- [x] 1.2 Implement `Update()`: khi `Input.GetMouseButtonDown(0)` → gọi `TryPush()`
- [x] 1.3 Implement `TryPush()`: `Physics2D.OverlapBoxAll` tại `transform.position`, size `(zoneWidth, zoneHeight)`, filter `playerLayer` → dedup roots → apply `AddForce((forceX, 0), Impulse)` lên tất cả Rigidbody2D trong mỗi root
- [x] 1.4 Implement `OnDrawGizmosSelected()`: vẽ WireBox màu vàng tại `transform.position`

## 2. Sửa ForcePoint.cs

- [x] 2.1 Xóa block `if (bodyLiftRigidbody != null) { bodyLiftRigidbody.AddForce(new Vector2(forceX, 0f), ...) }` khỏi `ApplyForce()`
- [x] 2.2 Xóa field `public float forceX` và `public float forceY` khỏi ForcePoint (không còn dùng)

## 3. Kiểm tra trong Unity Editor

- [ ] 3.1 Import script, kiểm tra không có lỗi compile
- [ ] 3.2 Gắn `PushForceZone` vào PushForce object trong scene, tune `forceX`, `zoneWidth`, `zoneHeight`, gán `playerLayer`
- [ ] 3.3 Play mode: tap trong zone → ragdoll nhận lực đẩy sang phải; tap ngoài zone → không có lực
