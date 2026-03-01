## Context

`LiftPoint.cs` hiện dùng `Physics2D.OverlapCircle` để check grounded và chỉ fire khi grounded. Cần đổi sang `Physics2D.Raycast` downward để đo chính xác khoảng cách Y từ LiftPoint đến mặt đất, từ đó tính hệ số scale cho lực lift.

Raycast từ vị trí LiftPoint (gần chân ragdoll, `y = -1.9` local) hướng xuống `Vector2.down`, distance tối đa `maxLiftHeight`. Nếu hit → `groundDist = hit.distance`, tính scale. Nếu không hit → `scale = 0`, không apply.

## Goals / Non-Goals

**Goals:**
- Lực lift scale tuyến tính: `scale = 1 - groundDist / maxLiftHeight`
- Cho phép tap khi đang trong không khí (nếu dưới `maxLiftHeight`) — lực tự giảm
- Thêm `public float maxLiftHeight` (Inspector)
- Gizmo hiển thị raycast range

**Non-Goals:**
- Không dùng curve phức tạp — linear fade đơn giản và dễ tune
- Không thay đổi `LiftAll()` logic (vẫn apply lên tất cả Rigidbody2D)

## Decisions

**D1: Raycast downward thay vì OverlapCircle**
- Cho biết chính xác khoảng cách đến mặt đất (không chỉ bool grounded)
- `Physics2D.Raycast(transform.position, Vector2.down, maxLiftHeight, groundLayer)`
- Nếu không hit (quá cao) → không apply lực

**D2: Linear scale `1 - dist/maxLiftHeight`**
- Đơn giản, trực quan: on ground = 1.0 (full), giữa = partial, tại maxHeight = 0
- Alternative: `Mathf.SmoothStep` — mượt hơn nhưng khó predict
- Alternative: `AnimationCurve` — linh hoạt nhất nhưng thêm phức tạp

**D3: Bỏ `isGrounded` riêng, thay bằng raycast result**
- Nếu raycast hit → có thể apply (kể cả trên không nếu đủ gần)
- Thống nhất 1 source of truth cho height check

**D4: Giữ `groundCheckRadius` cho gizmo, xóa dùng cho OverlapCircle**
- Gizmo vẫn cần để hiển thị; hoặc dùng gizmo line thay thế cho raycast range

## Risks / Trade-offs

- [Tap trên không khí] Nếu `maxLiftHeight` quá lớn → có thể tap ở độ cao lớn với lực nhỏ — cân nhắc set `maxLiftHeight` vừa phải (~3-5 units)
- [Raycast hướng down bị block bởi collider của chính ragdoll] → set `Physics2D.Raycast` từ layer không va chạm với ragdoll, hoặc dùng `layerMask` chỉ hit Ground

## Migration Plan

1. Sửa `LiftPoint.cs`: thêm `maxLiftHeight`, đổi trigger logic sang raycast + scale
2. Cập nhật V2 prefab: thêm `maxLiftHeight` value
