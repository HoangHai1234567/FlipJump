## Context

StickmanRagdoll hiện có `ForcePoint.cs` với một hướng lực duy nhất và không ground detection. GDD yêu cầu nhảy parabol (X+Y riêng biệt), chỉ nhảy khi đứng trên đất, và landing phải khóa nhân vật lại. Đây là Unity 2022.3 2D, dùng Rigidbody2D physics.

## Goals / Non-Goals

**Goals:**
- `ForcePoint.cs` có `forceX` (float) và `forceY` (float) độc lập — dễ tune từng chiều
- Ground check bằng `Physics2D.OverlapCircle` tại gốc transform, với layer mask `Ground`
- Chỉ gọi `AddForce` khi `isGrounded == true`
- Khi Body landing (OnCollisionEnter2D với layer Ground): set `velocity = Vector2.zero`, freeze constraints
- Khi nhảy: unfreeze constraints để ragdoll tự do
- Gizmo cập nhật: vẽ vector tổng hợp `(forceX, forceY)` thay vì hướng cũ
- Gravity scale tất cả Rigidbody2D trong prefab = 2 (tune-able)

**Non-Goals:**
- Không làm camera follow
- Không làm obstacle/platform system
- Không làm win/lose condition
- Không thay đổi Balance.cs hay IgnoreCollision.cs

## Decisions

**D1: Tách `forceX` và `forceY` thay vì dùng `forceDirection + forceMagnitude`**
- Lý do: GDD chỉ rõ X và Y là hai giá trị độc lập có range khác nhau. Designer cần tune chúng riêng (ví dụ: tăng height không ảnh hưởng horizontal distance)
- Thay thế: giữ Vector2 duy nhất → khó control cảm giác parabol

**D2: Ground check bằng `Physics2D.OverlapCircle` thay vì Raycast**
- Lý do: Stickman là ragdoll với nhiều collider, circle overlap detect ground tốt hơn single raycast vốn dễ miss khi body part che khuất
- `groundCheckRadius = 0.3f`, vị trí tại `transform.position` (root)
- Thay thế: OnCollisionStay2D → có false positive khi chạm wall

**D3: Landing freeze dùng `RigidbodyConstraints2D.FreezeAll` tạm thời**
- Khi landing: Body Rigidbody2D freeze hoàn toàn → nhân vật đứng yên
- Khi tap (trước AddForce): unfreeze Body Rigidbody2D → ragdoll tự do
- Lý do: GDD nói "velocity resets to ~0, locking the character in place"
- Thay thế: set `bodyType = Kinematic` → mất physics hoàn toàn, ragdoll sẽ lạ

**D4: Landing detect qua tag "Ground" thay vì layer**
- Lý do: Đơn giản hơn cho scope hiện tại, không cần setup Layer trong project settings
- Ground check OverlapCircle cũng dùng tag comparison hoặc layer mask config-able

**D5: Gravity scale = 2 cho Body, 1.5 cho các limb**
- Body rơi nhanh hơn tạo cảm giác weight
- Limb nhẹ hơn một chút để ragdoll tự nhiên hơn

## Risks / Trade-offs

- [Risk: Ragdoll nhiều Rigidbody2D, freeze chỉ Body có thể không đủ] → Freeze tất cả limb khi landing; unfreeze tất cả khi nhảy — cần `GetComponentsInChildren<Rigidbody2D>()`
- [Risk: Ground check radius quá lớn detect false positive] → Expose `groundCheckRadius` trong Inspector để tune
- [Risk: Physics unit scale khác GDD (GDD dùng 600-900N)] → Expose `forceX/forceY` với giá trị mặc định nhỏ (5/12), comment rõ để tune
