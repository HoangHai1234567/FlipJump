## Why

Apply `forceX` trực tiếp vào Body rigidbody không hiệu quả — lực chỉ tác động vào một điểm, không phản ánh đúng tương tác vật lý. Cần một vùng phát lực đẩy ngang (sang phải) hoạt động giống `GroundLiftZone`: khi tap và player nằm trong zone, apply impulse ngang lên toàn bộ ragdoll. Zone có kích thước tuỳ chỉnh được (width, height).

## What Changes

- Tạo script `PushForceZone.cs`: zone hình chữ nhật với `zoneWidth` và `zoneHeight` tùy chỉnh
- Khi tap và player trong zone: apply `AddForce((forceX, 0), Impulse)` lên tất cả Rigidbody2D của ragdoll
- Gizmo vẽ WireBox để visualize zone trong Editor
- `ForcePoint.cs`: xóa block apply `forceX` lên `bodyLiftRigidbody` (thay thế bằng PushForceZone)

## Capabilities

### New Capabilities
- `push-force-zone`: Zone phát lực đẩy ngang sang phải, tác động lên toàn bộ ragdoll khi tap và player nằm trong zone

### Modified Capabilities
- `leftmost-limb-jump`: `ForcePoint.ApplyForce()` không còn apply `forceX` lên body — chỉ giữ `spinTorque` và `bodyLiftForceY`

## Impact

- Tạo mới `Assets/Scripts/Ground/PushForceZone.cs`
- Sửa `Assets/Scripts/Ragdoll/ForcePoint.cs`: xóa block `bodyLiftRigidbody.AddForce(forceX, 0)`
- PushForceZone object trong scene cần được thiết lập thủ công
