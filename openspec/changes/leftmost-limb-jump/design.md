## Context

Trong FlipJump, ragdoll stickman có 10+ Rigidbody2D kết nối qua HingeJoint2D. Cơ chế hiện tại apply lực vào Body (center of mass), tạo ra chuyển động tịnh tiến nhưng không tạo spin. Game gốc FlipJump tạo hiệu ứng spin bằng cách apply lực lệch tâm vào bộ phận đầu cuối (tay/chân) ở phía trái — torque tạo ra từ moment arm (khoảng cách từ body center đến điểm apply lực).

Ragdoll hierarchy: StickmanRagdoll (root) → Body (pivot), Head, Upper/Lower Arms, Upper/Lower Legs. "Leaf limbs" = Lower Left Arm, Lower Right Arm, Lower Left Leg, Lower Right Leg, Head (các limb không có child physics body).

## Goals / Non-Goals

**Goals:**
- Tự động tìm leaf limb ở bên trái nhất (X position nhỏ nhất) tại thời điểm tap
- Apply impulse force vào limb đó để tạo spin effect
- Giữ nguyên grounded check, unfreeze/freeze logic từ player-jump-mechanic
- Cho phép cấu hình danh sách leaf limbs trong Inspector (`Transform[] leafLimbs`)

**Non-Goals:**
- Không làm camera follow
- Không thay đổi Balance.cs hay IgnoreCollision.cs
- Không implement multiple tap patterns hay combo
- Không thay đổi HingeJoint2D configuration

## Decisions

**D1: Dùng `Transform[] leafLimbs` thay vì auto-detect từ hierarchy**
- Lý do: Ragdoll có cả upper và lower limbs — chỉ muốn apply lực vào "đầu cuối" (Lower Arms, Lower Legs, Head). Auto-detect bằng "no child Rigidbody2D" sẽ lấy đúng nhưng dễ sai nếu hierarchy thay đổi.
- Array trong Inspector dễ kiểm soát, designer có thể chọn limbs nào tham gia
- Thay thế: Tag-based detection → phức tạp hơn, cần thêm tags

**D2: So sánh `transform.position.x` trong world space**
- Lý do: Ragdoll đứng thẳng khi grounded, tất cả limbs ở vị trí tự nhiên. World X position phản ánh đúng "bên trái" vs "bên phải".
- Thay thế: Local space X → bị ảnh hưởng bởi rotation của parent container

**D3: Giữ nguyên `ForcePoint.cs` làm entry point, chỉ đổi target selection**
- Lý do: ForcePoint đã có grounded check, Input.GetMouseButtonDown(0), FreezeAll/UnfreezeAll. Tái sử dụng toàn bộ, chỉ thay phần `targetRigidbody` cố định thành dynamic selection.
- Đổi `public Rigidbody2D targetRigidbody` thành `public Transform[] leafLimbs` + method `FindLeftmostLeaf()`

**D4: Apply force cả forceX và forceY vào leaf limb**
- Khi lực apply vào điểm lệch so với center of mass của toàn ragdoll, HingeJoint2D truyền phản lực ngược lại qua chain → tạo spin tự nhiên
- `forceX` = horizontal impulse (đẩy sang phải), `forceY` = vertical impulse (đẩy lên)
- Không cần thêm `AddTorque` riêng — spin tự nhiên từ offset force

## Risks / Trade-offs

- [Risk: Khi nhân vật đứng thẳng, tất cả leaf limbs có X gần bằng nhau] → Vẫn sẽ chọn được 1 limb; khi character asymmetric (sau nhiều jumps) sẽ luôn đúng
- [Risk: leafLimbs array rỗng/null → NullReferenceException] → Guard check: nếu array rỗng, fallback về Body Rigidbody2D
- [Risk: Spin quá mạnh khiến ragdoll bay lên mà không kiểm soát] → Expose `forceX` và `forceY` để tune; Angular Drag của HingeJoint2D giúp giảm spin tự nhiên
