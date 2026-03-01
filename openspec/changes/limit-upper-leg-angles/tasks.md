## 1. Sửa StickmanRagdoll.prefab

- [x] 1.1 Tìm HingeJoint2D của Upper Left Leg (`&149356426`): set `m_UseLimits: 1`, `m_LowerAngle: -90`, `m_UpperAngle: 90`
- [x] 1.2 Tìm HingeJoint2D của Upper Right Leg (`&1357506824`): set `m_UseLimits: 1`, `m_LowerAngle: -90`, `m_UpperAngle: 90`

## 2. Sửa StickmanRagdollV2.prefab

- [x] 2.1 Tìm HingeJoint2D của Upper Left Leg trong V2 (cùng fileID `&149356426`): set `m_UseLimits: 1`, `m_LowerAngle: -90`, `m_UpperAngle: 90`
- [x] 2.2 Tìm HingeJoint2D của Upper Right Leg trong V2 (cùng fileID `&1357506824`): set `m_UseLimits: 1`, `m_LowerAngle: -90`, `m_UpperAngle: 90`

## 3. Kiểm tra trong Unity Editor

- [ ] 3.1 Import prefab, kiểm tra Inspector HingeJoint2D của Upper Left/Right Leg: `Use Limits: true`
- [ ] 3.2 Play mode, tap — chân không xoay quá 90° so với Body
- [ ] 3.3 Tune `m_LowerAngle` / `m_UpperAngle` nếu cần (quá cứng → tăng, xuyên body → giảm)
