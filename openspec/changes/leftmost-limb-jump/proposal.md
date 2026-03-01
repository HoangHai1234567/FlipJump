## Why

Cơ chế nhảy hiện tại apply lực vào Body Rigidbody2D theo hướng cố định, không tạo ra hiệu ứng xoay vòng tự nhiên như trong game gốc FlipJump. GDD yêu cầu khi tap, nhân vật nảy lên và xoay vòng — điều này chỉ xảy ra nếu lực được apply lệch tâm vào một bộ phận cuối ở phía trái, tạo torque.

## What Changes

- **Xóa `ForcePoint` concept cũ**: Không còn apply lực vào Body (điểm trung tâm). Thay bằng logic tìm limb phù hợp rồi apply lực vào đó.
- **Tìm leftmost leaf limb**: Mỗi khi tap, script tìm bộ phận "lá" (không có child ragdoll) đang có vị trí X nhỏ nhất (nằm bên trái nhất) trong số: Lower Left Arm, Lower Right Arm, Lower Left Leg, Lower Right Leg, Head.
- **Apply lực vào limb đó**: `AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse)` lên Rigidbody2D của limb đó, tạo ra cả translational và rotational motion (spin).
- **Giữ nguyên grounded check và landing freeze**: Logic từ `player-jump-mechanic` vẫn còn nguyên.
- **ForcePoint.cs refactor**: Đổi `targetRigidbody` thành auto-select — tìm leftmost leaf Rigidbody2D tại thời điểm tap.

## Capabilities

### New Capabilities
- `leftmost-limb-selection`: Logic tìm bộ phận lá ở bên trái nhất và apply lực vào đó để tạo spin effect

### Modified Capabilities
- `force-point`: Thay đổi từ fixed targetRigidbody sang dynamic limb selection; giữ nguyên grounded guard và freeze/unfreeze

## Impact

- Sửa `Assets/Scripts/Ragdoll/ForcePoint.cs`: bỏ `public Rigidbody2D targetRigidbody`, thêm leaf detection logic
- Thêm `public Transform[] leafLimbs` để cấu hình danh sách bộ phận lá trong Inspector
- Prefab `StickmanRagdoll.prefab`: cập nhật ForcePoint MonoBehaviour — xóa `targetRigidbody`, thêm array `leafLimbs`
