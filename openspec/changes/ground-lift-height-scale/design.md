## Context

`GroundLiftZone.TryLift()` hiện apply `liftForceY` phẳng cho tất cả Rigidbody2D của player trong zone. Cần tính `liftScale` theo độ cao của ragdoll root so với đỉnh ground collider.

## Goals / Non-Goals

**Goals:**
- Scale lực tuyến tính: `liftScale = 1 - Clamp01(heightAboveGround / zoneHeight)`
- `heightAboveGround = ragdollRoot.position.y - groundTop` (groundTop = `groundCollider.bounds.max.y`, hoặc `transform.position.y` nếu không có collider)
- Mỗi root riêng có scale riêng (nhiều player trong cùng zone mỗi người 1 scale khác nhau)

**Non-Goals:**
- Không dùng curve — linear đơn giản, dễ tune
- Không thay đổi `zoneHeight` hay `liftForceY` — chỉ nhân thêm scale

## Decisions

**D1: Tính scale theo ragdollRoot.position.y**
- Root = `hit.transform.root` đã được dedup
- `heightAboveGround = root.position.y - groundTop`
- Nếu âm (root dưới ground): `Clamp01` cho ra 0 → scale = 1.0 (full force)
- Nếu bằng zoneHeight: scale = 0 (không apply)

**D2: Cache groundTop**
- `groundTop = groundCollider != null ? groundCollider.bounds.max.y : transform.position.y`
- Tính lại trong `TryLift()` mỗi lần (ground có thể di chuyển, nhưng thường static)

## Risks / Trade-offs

- [Multiple roots ở độ cao khác nhau] → mỗi root tính scale riêng → đúng behavior
- [Root Y không phản ánh chân nhân vật] → có thể dùng `root.position.y` hoặc lowest Rigidbody2D; dùng root đủ đơn giản
