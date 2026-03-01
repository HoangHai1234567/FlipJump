## 1. Cập nhật GroundLiftZone.cs

- [x] 1.1 Trong `TryLift()`: sau khi dedup roots, với mỗi root tính `float groundTop = groundCollider != null ? groundCollider.bounds.max.y : transform.position.y`
- [x] 1.2 Tính `float heightAboveGround = root.position.y - groundTop` và `float liftScale = 1f - Mathf.Clamp01(heightAboveGround / zoneHeight)`
- [x] 1.3 Đổi `rb.AddForce(new Vector2(0f, liftForceY), ...)` thành `rb.AddForce(new Vector2(0f, liftForceY * liftScale), ...)`

## 2. Kiểm tra trong Unity Editor

- [ ] 2.1 Import script, kiểm tra không có lỗi compile
- [ ] 2.2 Play mode: tap sát đất → lực tối đa; tap giữa không trung → lực giảm; tap cao hơn zoneHeight → không có lực
