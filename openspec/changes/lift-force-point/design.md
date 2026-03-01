## Context

`ForcePoint.cs` hiện xử lý cả spin logic (leftmost leaf selection) lẫn body lift. `LiftPoint.cs` sẽ là script đơn giản hơn: không cần leaf selection, chỉ apply `(0, liftForceY)` lên tất cả Rigidbody2D trong ragdoll khi tap + grounded.

LiftPoint sẽ là một child GameObject của ragdoll root (tương tự ForcePoint). Dùng `transform.root.GetComponentsInChildren<Rigidbody2D>()` để tìm tất cả parts mà không cần reference thêm.

## Goals / Non-Goals

**Goals:**
- Script độc lập: `liftForceY`, `groundCheckRadius`, `groundLayer`
- Apply `(0, liftForceY)` lên tất cả Rigidbody2D trong hierarchy khi tap + grounded
- Gizmo sphere để hiển thị vị trí và trạng thái grounded

**Non-Goals:**
- Không có spin/torque logic
- Không cần biết về ForcePoint
- Không quản lý freeze/unfreeze (ForcePoint đã làm trong cùng frame)

## Decisions

**D1: Dùng `transform.root` để tìm root ragdoll**
- Không cần `public Transform` reference thêm
- Hoạt động đúng miễn là LiftPoint là child của root ragdoll
- Alternative: `public Transform ragdollRoot` — linh hoạt hơn nhưng thêm setup

**D2: LiftPoint respond `Input.GetMouseButtonDown(0)` độc lập**
- Cùng input trigger như ForcePoint → cả 2 fire cùng frame
- Alternative: ForcePoint gọi LiftPoint trực tiếp → coupling không cần thiết

**D3: Chỉ apply khi grounded (riêng LiftPoint có ground check của mình)**
- Nhất quán với ForcePoint; không nâng khi đang ở trên không
- Dùng cùng `groundCheckRadius` + `groundLayer` pattern

## Risks / Trade-offs

- [Double ground check] Cả ForcePoint và LiftPoint đều check grounded cùng frame → overhead nhỏ, không đáng kể
- [liftForceY cần tune] Áp dụng cho ~10 parts → cần giá trị nhỏ hơn so với apply vào 1 Body

## Migration Plan

1. Tạo `LiftPoint.cs`
2. Thêm GameObject `LiftPoint` vào V2 prefab YAML, gắn component
3. Set `bodyLiftEnabled: 0` trên ForcePoint V2 (lift nay do LiftPoint)
