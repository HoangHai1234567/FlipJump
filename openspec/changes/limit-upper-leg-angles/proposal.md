## Why

Hiện tại HingeJoint2D của Upper Left Leg và Upper Right Leg không có giới hạn góc (`m_UseLimits: 0`), cho phép chân xoay tự do 360°. Điều này tạo ra tư thế không tự nhiên — chân có thể xoay ngược lên trên, vào thân, hoặc xuyên qua các bộ phận khác trong khi nhảy/rơi.

## What Changes

- Bật `m_UseLimits: 1` trên HingeJoint2D của **Upper Left Leg** (fileID `149356426`)
- Bật `m_UseLimits: 1` trên HingeJoint2D của **Upper Right Leg** (fileID `1357506824`)
- Đặt `m_LowerAngle` và `m_UpperAngle` phù hợp để chân chỉ xoay trong phạm vi tự nhiên
- Áp dụng cho cả **StickmanRagdoll.prefab** và **StickmanRagdollV2.prefab**

## Capabilities

### New Capabilities
- `upper-leg-angle-limits`: Giới hạn góc xoay HingeJoint2D của 2 khớp chân trên kết nối với Body

### Modified Capabilities

## Impact

- Sửa `Assets/Prefabs/StickmanRagdoll.prefab` — 2 HingeJoint2D blocks
- Sửa `Assets/Prefabs/StickmanRagdollV2.prefab` — 2 HingeJoint2D blocks (cùng cấu trúc YAML)
- Không thay đổi script nào
