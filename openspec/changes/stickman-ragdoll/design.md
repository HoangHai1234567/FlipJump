## Context

Project FlipJump là một Unity 2D game. Cần thiết lập nhân vật stickman với hệ thống ragdoll physics từ project tham khảo `D:\TestRagdoll\Stickman-Ragdoll-Tutorial`. Project tham khảo sử dụng Unity 2D physics (Rigidbody2D, Collider2D) với 3 scripts: `Movement.cs` (di chuyển), `Balance.cs` (cân bằng ragdoll), `IgnoreCollision.cs` (ngăn va chạm nội bộ).

## Goals / Non-Goals

**Goals:**
- Copy cấu trúc hierarchy Player (Head, Body, 4 limbs) sang FlipJump
- Mang `Balance.cs` và `IgnoreCollision.cs` sang project mới
- Tạo prefab `StickmanRagdoll` không có logic input/movement
- Stickman phải bị ảnh hưởng bởi gravity và va chạm với environment

**Non-Goals:**
- Không implement input handling hay movement
- Không tích hợp với gameplay (jump trigger, obstacle detection)
- Không thay đổi animation system

## Decisions

**D1: Copy scripts thủ công thay vì dùng Package**
- Lý do: Project tham khảo không phải Unity Package, chỉ là scene tutorial
- Thay thế đã xem xét: Extract thành .unitypackage → phức tạp hơn cần thiết
- Quyết định: Copy file .cs trực tiếp vào `Assets/Scripts/Ragdoll/`

**D2: Tạo Prefab thay vì chỉ có scene object**
- Lý do: Prefab cho phép instantiate nhiều lần và dễ maintain
- Thay thế: Đặt trực tiếp vào scene → không linh hoạt
- Quyết định: Tạo `Assets/Prefabs/StickmanRagdoll.prefab`

**D3: Xóa Movement.cs hoàn toàn, không comment out**
- Lý do: Code sạch, tránh nhầm lẫn khi implement gameplay sau
- Thay thế: Giữ lại với `[Disabled]` attribute → vẫn gây confusion
- Quyết định: Không copy Movement.cs sang project mới

**D4: Copy assets đồ họa stickman (StickmanGraphics.psb)**
- Lý do: Cần sprite/texture để nhìn thấy stickman trong game
- Thay thế: Dùng Unity primitive shapes → mất visual fidelity
- Quyết định: Copy asset đồ họa sang `Assets/Art/Stickman/`

## Risks / Trade-offs

- [Risk: Unity version mismatch giữa 2 project] → Kiểm tra `.unity` version header trước khi copy; nếu mismatch, tạo lại object thủ công trong scene
- [Risk: Script GUIDs conflict] → Copy vào thư mục mới, Unity sẽ tạo GUID mới tự động
- [Risk: Animation controller references bị mất] → Copy toàn bộ Animations folder; nếu references bị break thì reassign thủ công trong Inspector
