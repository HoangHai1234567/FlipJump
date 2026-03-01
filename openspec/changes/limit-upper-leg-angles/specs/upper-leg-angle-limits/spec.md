## ADDED Requirements

### Requirement: HingeJoint2D của Upper Leg có giới hạn góc
HingeJoint2D của **Upper Left Leg** và **Upper Right Leg** SHALL có `m_UseLimits: 1`. `m_LowerAngle` và `m_UpperAngle` SHALL được đặt để ngăn chân xoay tự do 360°. Giá trị mặc định đề xuất: `-90` / `90`. Giới hạn SHALL được apply cho cả `StickmanRagdoll.prefab` và `StickmanRagdollV2.prefab`.

#### Scenario: Chân không xoay quá giới hạn khi ragdoll rơi
- **WHEN** nhân vật nhảy và rơi xuống với physics đang hoạt động
- **THEN** góc của Upper Left Leg và Upper Right Leg so với Body không vượt quá `m_UpperAngle` hoặc thấp hơn `m_LowerAngle`

#### Scenario: Cả 2 prefab đều có giới hạn
- **WHEN** `StickmanRagdoll` hoặc `StickmanRagdollV2` được kéo vào scene
- **THEN** Inspector HingeJoint2D của mỗi upper leg cho thấy `Use Limits: true` với lower/upper angle đã set

#### Scenario: Giá trị có thể điều chỉnh trong Inspector
- **WHEN** designer chọn Upper Leg trong Hierarchy
- **THEN** có thể thay đổi `m_LowerAngle` và `m_UpperAngle` trong Inspector mà không cần sửa code
