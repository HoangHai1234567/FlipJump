## Context

StickmanRagdoll hiện tại là pure physics — không có cơ chế nào để apply lực từ bên ngoài. Cần một điểm phát lực (ForcePoint) có thể cấu hình được để làm nền tảng cho jump mechanic. Project là Unity 2022.3 2D với physics Rigidbody2D.

## Goals / Non-Goals

**Goals:**
- Script `ForcePoint.cs` với `forceDirection` (Vector2) và `forceMagnitude` (float) expose ra Inspector
- Gizmo hiển thị mũi tên lực trong Scene view (origin tại vị trí ForcePoint, đầu mũi tên theo hướng × magnitude)
- Handle Gizmo cho phép kéo đầu mũi tên để chỉnh `forceDirection` và `forceMagnitude` ngay trong scene
- Public method `ApplyForce()` apply `ForceMode2D.Impulse` lên target Rigidbody2D
- Child object `ForcePoint` được thêm vào prefab `StickmanRagdoll`

**Non-Goals:**
- Không xử lý input (tap/click) — ForcePoint chỉ là "nguồn lực", caller tự quyết khi nào gọi
- Không làm cooldown, animation trigger, hay sound
- Không làm multiple force points

## Decisions

**D1: Dùng `ForceMode2D.Impulse` thay vì `Force`**
- Lý do: Impulse apply lực tức thì trong một frame — phù hợp với cú jump snappy. Force trải đều qua nhiều frame — quá mềm cho jump.
- Thay thế xem xét: `AddForce` với Force mode → nhân vật nhảy chậm, không crisp.

**D2: Target Rigidbody2D là field public, không tự tìm**
- Lý do: ForcePoint là child của Player root (không có Rigidbody2D), cần assign Body's Rigidbody2D thủ công trong Inspector
- Thay thế: Tự tìm `GetComponentInParent<Rigidbody2D>()` → có thể lấy nhầm limb nếu đặt sai vị trí

**D3: Gizmo dùng `OnDrawGizmos` + `OnDrawGizmosSelected` + Custom Handle**
- `OnDrawGizmos`: luôn hiển thị mũi tên (màu nhạt) ngay cả khi không select
- `OnDrawGizmosSelected`: hiển thị handle kéo được khi select object
- Dùng `Handles.FreeMoveHandle` tại đầu mũi tên để kéo chỉnh direction + magnitude

**D4: Normalize direction tự động, magnitude là scalar riêng**
- Lý do: Tách biệt hướng và độ lớn dễ tune hơn là dùng một Vector2 duy nhất
- `forceDirection` luôn được normalize trước khi apply: `forceDirection.normalized * forceMagnitude`

## Risks / Trade-offs

- [Risk: Handles API chỉ hoạt động trong Editor] → Bọc toàn bộ Gizmo/Handle code trong `#if UNITY_EDITOR`
- [Risk: ForcePoint apply lực lên Body nhưng ragdoll có nhiều Rigidbody2D] → Document rõ: assign Body's Rigidbody2D; các limb sẽ bị kéo theo qua joint
