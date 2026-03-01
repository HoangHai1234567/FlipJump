## Context

Game FlipJump dùng camera cố định. Nhân vật StickmanRagdoll có thể nhảy và di chuyển tự do. Camera cần theo dõi để nhân vật luôn trong tầm nhìn.

Camera là 2D orthographic. Nhân vật sẽ di chuyển chủ yếu theo trục X (lăn sang phải/trái) và Y (nhảy lên). Target follow sẽ là Body (parent của toàn bộ ragdoll) hoặc root transform của StickmanRagdoll.

## Goals / Non-Goals

**Goals:**
- Camera smooth follow target Transform theo cả X và Y
- Cấu hình được offset (để không che nhân vật)
- Cấu hình được damping time (smooth speed)
- Không thay đổi Z position của camera

**Non-Goals:**
- Camera bounds / clamp (không giới hạn vùng di chuyển camera)
- Camera zoom / ortho size thay đổi
- Parallax background

## Decisions

**D1: Dùng `Vector3.SmoothDamp` thay vì `Lerp`**
- `SmoothDamp` tự xử lý velocity reference, giảm tốc mượt hơn `Lerp` (Lerp luôn nhanh khi xa, chậm khi gần)
- Alternative: `Lerp` đơn giản hơn nhưng có cảm giác "sticky" khi gần đích

**D2: Chỉ follow X, giữ nguyên Y**
- Camera chỉ di chuyển ngang theo nhân vật, Y cố định
- Alternative: follow cả X và Y → không cần thiết theo yêu cầu

**D3: Target là `Transform` (Body hoặc root)**
- Dùng `Transform` thay vì `Rigidbody2D` để tránh coupling với physics
- Gán qua Inspector để linh hoạt (designer chọn target)

**D4: Đặt script trên Main Camera**
- Camera tự kéo mình đến target, không cần script riêng trên nhân vật
- `LateUpdate` để đảm bảo camera update sau khi physics đã giải quyết xong

## Risks / Trade-offs

- [Nếu target bị destroy] Camera sẽ null-check và không crash
- [Damping quá lớn] Camera lag xa nhân vật → tune giá trị mặc định

## Migration Plan

1. Tạo script `CameraFollow.cs`
2. Gắn vào Main Camera trong scene
3. Gán Body Transform vào field `target`
