## Context

Trong Unity 2D physics:
- `AddForce(v, Impulse)` → tác động linear velocity + angular velocity (nếu điểm đặt lực lệch tâm)
- `AddTorque(t, Impulse)` → chỉ tác động **angular velocity**, không ảnh hưởng linear velocity gì cả
- `AddForceAtPosition(f, pos)` → tác động cả linear lẫn angular tùy vị trí

Khớp hông (hip joint) là `m_ConnectedAnchor` của Upper Leg HingeJoint2D, kết nối với Body ở khoảng `y = -0.69` (đáy Body). `AddTorque` trên Body sẽ xoay Body quanh center of mass của nó. Vì Body nằm trên hip joint, nhân vật sẽ trông như đang xoay quanh vùng hông.

Hiện tại `forceX: 4, forceY: 0` vào leaf limb vẫn tạo một lực ngang nhỏ. Để tách biệt hoàn toàn spin với translation: có thể set `forceX = 0` và dùng thuần `spinTorque`, hoặc giữ `forceX` nhỏ để tạo hiệu ứng "hất sang" đồng thời xoay.

## Goals / Non-Goals

**Goals:**
- Thêm `spinTorque` field: apply `AddTorque` lên `bodyLiftRigidbody` (Body)
- Spin mạnh, tách biệt hoàn toàn khỏi `forceX` và `bodyLiftForceY`
- Có thể tune riêng trong Inspector

**Non-Goals:**
- Không đổi pivot vật lý của HingeJoint2D
- Không thay đổi `bodyLiftForceY` hay `forceX` logic

## Decisions

**D1: Dùng `AddTorque` thay vì `AddForceAtPosition` tại hip joint**
- `AddTorque` = pure rotation, 0 linear effect → đúng yêu cầu "không ảnh hưởng lực đẩy ngang"
- `AddForceAtPosition` tại bất kỳ điểm nào vẫn tạo linear component
- `AddTorque` trên Body kéo cả ragdoll chain xoay theo do HingeJoint2D

**D2: Apply `spinTorque` lên cùng `bodyLiftRigidbody` (Body)**
- Tái dụng field sẵn có; không cần thêm reference mới
- Nếu `bodyLiftRigidbody` null → log warning, skip (consistent với existing pattern)

**D3: Giá trị dương = xoay ngược chiều kim đồng hồ (CCW) trong Unity 2D**
- Với nhân vật nhảy sang phải: torque âm (`-spinTorque`) = xoay CW = tự nhiên hơn
- Expose `spinTorque` không có dấu; dùng âm trong YAML nếu muốn CW

**D4: Giữ `forceX` vào leaf limb**
- Tạo hiệu ứng "hất" nhỏ vào bộ phận bên trái, kết hợp với torque tạo cảm giác tự nhiên
- Designer có thể set `forceX = 0` để xoay thuần nếu muốn

## Risks / Trade-offs

- [Torque quá lớn] Nhân vật xoay quá nhanh → tune `spinTorque`, default `200f`
- [Torque nhỏ] Spin không đáng kể → tăng lên
- [Hướng xoay] Phụ thuộc dấu của `spinTorque`; nếu sai hướng → đổi dấu trong prefab
