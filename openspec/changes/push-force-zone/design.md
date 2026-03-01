## Context

`ForcePoint.cs` hiện apply `forceX` trực tiếp lên `bodyLiftRigidbody` (Body). Cần thay thế bằng một zone object riêng biệt tương tự `GroundLiftZone`, nhưng cho lực ngang. Zone gắn vào PushForce object (không gắn vào ragdoll), tự định nghĩa kích thước.

## Goals / Non-Goals

**Goals:**
- `PushForceZone.cs` gắn lên bất kỳ world object nào, zone hình chữ nhật `zoneWidth × zoneHeight`
- Khi tap: `Physics2D.OverlapBoxAll` tại zone center → dedup roots → apply `(forceX, 0)` lên tất cả Rigidbody2D
- Zone center = `transform.position` + offset nếu cần
- Xóa `forceX` apply trong `ForcePoint.ApplyForce()`

**Non-Goals:**
- Không scale lực theo vị trí trong zone (flat force)
- Không auto-size từ collider (zone tự định nghĩa width/height)

## Decisions

**D1: PushForceZone tương tự GroundLiftZone nhưng không dùng BoxCollider2D để auto-size**
- Zone size hoàn toàn manual (`zoneWidth`, `zoneHeight`) — không cần ground collider
- Zone center = `transform.position` (người dùng tự đặt object ở đúng vị trí)

**D2: Xóa forceX khỏi ForcePoint**
- ForcePoint giữ: `spinTorque`, `bodyLiftEnabled/bodyLiftForceY`
- ForcePoint bỏ: block `bodyLiftRigidbody.AddForce(new Vector2(forceX, 0f), ...)`
- Field `forceX` vẫn giữ nhưng không dùng trong ApplyForce (hoặc xóa luôn — tùy)

**D3: playerLayer thay vì tag**
- Nhất quán với GroundLiftZone

## Risks / Trade-offs

- [forceX field còn trong Inspector nhưng không dùng] → nên xóa field luôn để tránh nhầm lẫn
- [Zone position phải đặt thủ công] → người dùng phải đặt PushForceZone object đúng chỗ
