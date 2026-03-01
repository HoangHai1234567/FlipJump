## Context

`HingeJoint2D` trong Unity 2D hỗ trợ giới hạn góc qua:
- `m_UseLimits: 1` — bật chế độ giới hạn
- `m_AngleLimits.m_LowerAngle` — góc nhỏ nhất (tính từ rest pose)
- `m_AngleLimits.m_UpperAngle` — góc lớn nhất

Ragdoll hiện tại: Upper Left Leg (`&149356426`) và Upper Right Leg (`&1357506824`) kết nối với Body qua HingeJoint2D, anchor gần đỉnh upper leg, connected anchor ở đáy Body. Rest pose là khi nhân vật đứng thẳng.

## Goals / Non-Goals

**Goals:**
- Giới hạn góc xoay 2 khớp hông để chân không xoay quá 360°
- Giới trị mặc định hợp lý, dễ tune trong Unity Inspector
- Áp dụng cho cả prefab gốc và V2

**Non-Goals:**
- Không giới hạn lower leg (knee joint) — yêu cầu chỉ nói upper
- Không thay đổi script hay logic

## Decisions

**D1: Góc giới hạn mặc định `-90` / `90`**
- Cho phép chân xoay từ -90° (ra sau) đến +90° (ra trước) so với rest pose — phạm vi tự nhiên của khớp hông người
- Alternative: `-60` / `60` → bảo thủ hơn nhưng có thể cứng khi nhảy; `-120` / `120` → rộng hơn nhưng vẫn ngăn xoay ngược hoàn toàn
- Giá trị này có thể điều chỉnh trong Inspector sau khi test

**D2: Chỉnh thẳng trong YAML prefab, không dùng script**
- Không cần runtime code — giới hạn là thuộc tính tĩnh của joint
- Thay đổi minimal, không thêm component mới

**D3: Áp dụng cùng giá trị cho cả left và right leg**
- Ragdoll đối xứng; nếu cần bất đối xứng có thể tune riêng trong Inspector

## Risks / Trade-offs

- [Góc quá hẹp] Chân trông cứng khi ragdoll rơi → tăng lên ±120 nếu cần
- [Góc quá rộng] Vẫn có thể xuyên body → giảm xuống ±60 nếu cần
- [Rest pose offset] Nếu nhân vật không đứng ở 0° khi tạo joint, góc thực tế sẽ lệch → cần test trong Editor và điều chỉnh

## Migration Plan

1. Sửa YAML `StickmanRagdoll.prefab`: set `m_UseLimits: 1`, `m_LowerAngle: -90`, `m_UpperAngle: 90` cho 2 joint
2. Sửa YAML `StickmanRagdollV2.prefab`: cùng cập nhật
3. Test trong Play mode, tune nếu cần
