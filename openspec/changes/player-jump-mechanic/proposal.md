## Why

GDD FlipJump v3 Final xác định cơ chế nhảy cốt lõi: **một tap = một cú nhảy parabol sang phải và lên trên** với lực cố định (X: 300–500N, Y: 600–900N). Cách làm hiện tại (ForcePoint chỉ có một hướng, không có ground detection, nhảy liên tục không giới hạn) không khớp với thiết kế — cần refactor lại đúng theo GDD ngay từ đầu.

## What Changes

- **Sửa `ForcePoint.cs`**: thay `forceDirection + forceMagnitude` bằng hai field riêng `forceX` và `forceY` (tương ứng X và Y impulse), gizmo cập nhật để hiển thị vector parabol
- **Thêm ground detection** vào `ForcePoint.cs`: dùng `Physics2D.OverlapCircle` tại chân nhân vật để kiểm tra isGrounded; chỉ apply lực khi đang đứng trên ground
- **Landing logic**: khi Body Rigidbody2D chạm platform (OnCollisionEnter2D), reset velocity về 0 và freeze rotation để nhân vật đứng yên chờ tap tiếp
- **Cập nhật gravity scale** của các Rigidbody2D trong prefab từ 1 lên 2 (theo GDD: 2.0–2.5) để có cảm giác rơi nhanh hơn
- Cập nhật giá trị mặc định trong prefab: `forceX = 5`, `forceY = 12` (scale nhỏ hơn GDD vì unit physics khác, cần tune trong Editor)

## Capabilities

### New Capabilities
- `grounded-jump`: Cơ chế nhảy parabol sang phải + lên, chỉ hoạt động khi stickman đang đứng trên ground; landing reset velocity và freeze character

### Modified Capabilities
- `force-point`: Thay đổi API từ `forceDirection/forceMagnitude` sang `forceX/forceY` riêng biệt; thêm ground check logic

## Impact

- Sửa `Assets/Scripts/Ragdoll/ForcePoint.cs`
- Cập nhật `Assets/Prefabs/StickmanRagdoll.prefab`: gravity scale + default force values
- Các Rigidbody2D của Body trong prefab cần `m_Constraints` freeze rotation khi landing
