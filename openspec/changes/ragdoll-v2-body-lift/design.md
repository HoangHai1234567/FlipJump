## Context

`ForcePoint.cs` hiện tại apply 1 lực duy nhất `(forceX, forceY)` vào leaf limb bên trái nhất. Lực này lệch tâm nên tạo spin, nhưng thành phần Y có thể không đủ để đẩy toàn bộ ragdoll lên cao. Cần bổ sung thêm 1 lực thẳng đứng `(0, bodyLiftForceY)` trực tiếp vào Body (center of mass của ragdoll) để nhân vật nảy lên mạnh hơn.

`StickmanRagdoll.prefab` là prefab gốc — không được thay đổi hành vi. Tạo `StickmanRagdollV2.prefab` là bản sao và bật tính năng body lift trên đó.

## Goals / Non-Goals

**Goals:**
- Thêm optional body lift vào `ForcePoint.cs` (off by default)
- V2 prefab bật body lift, giữ toàn bộ tính năng hiện tại (leftmost-limb, grounded guard, landing freeze, camera follow)
- Prefab gốc không bị ảnh hưởng

**Non-Goals:**
- Không thay đổi logic leftmost-limb selection
- Không tạo class mới / kế thừa
- Không thay đổi camera follow

## Decisions

**D1: Thêm fields vào ForcePoint.cs thay vì tạo ForcePointV2.cs**
- Tránh code duplication; body lift là optional extension của cùng behavior
- Alternative: tạo ForcePointV2 subclass → phức tạp, 2 class phải maintain song song

**D2: `bodyLiftEnabled` flag thay vì check `bodyLiftRigidbody != null`**
- Tường minh, designer thấy rõ ý định enable/disable
- Alternative: null-check → ẩn ý định, dễ gây nhầm lẫn

**D3: Apply 2 lực độc lập trong cùng 1 `ApplyForce()` call**
- 2 lực apply cùng frame → physics engine xử lý cộng vector tự nhiên
- Alternative: coroutine / delay → không cần thiết, phức tạp không lý do

**D4: `bodyLiftForceY` riêng, không dùng `forceY`**
- Body lift thường cần giá trị khác spin force; tách fields để tune độc lập
- Alternative: dùng chung `forceY` → kém linh hoạt

**D5: Duplicate prefab bằng copy YAML thủ công**
- Giữ nguyên toàn bộ fileID hierarchy; đổi tên prefab object và set `bodyLiftEnabled = 1`

## Risks / Trade-offs

- [Double force gây quá mạnh] → tune `bodyLiftForceY` trong Inspector, default `8f`
- [Body bị đẩy riêng khỏi chain] → HingeJoint2D giữ các limb kết nối; Body tăng tốc kéo cả ragdoll lên

## Migration Plan

1. Sửa `ForcePoint.cs` — thêm fields, sửa `ApplyForce()`
2. Copy `StickmanRagdoll.prefab` → `StickmanRagdollV2.prefab`
3. Sửa YAML V2: set `bodyLiftEnabled = 1`, `bodyLiftRigidbody`, `bodyLiftForceY`
