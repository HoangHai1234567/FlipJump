## 1. Copy Assets và Scripts từ project tham khảo

- [x] 1.1 Tạo thư mục `Assets/Scripts/Ragdoll/` trong project FlipJump
- [x] 1.2 Copy `Balance.cs` từ `D:\TestRagdoll\Stickman-Ragdoll-Tutorial\Assets\Scripts\Balance.cs` vào `Assets/Scripts/Ragdoll/Balance.cs`
- [x] 1.3 Copy `IgnoreCollision.cs` từ `D:\TestRagdoll\Stickman-Ragdoll-Tutorial\Assets\Scripts\IgnoreCollision.cs` vào `Assets/Scripts/Ragdoll/IgnoreCollision.cs`
- [x] 1.4 Tạo thư mục `Assets/Art/Stickman/` và copy asset đồ họa `StickmanGraphics.psb` từ project tham khảo
- [x] 1.5 Copy thư mục `Animations/` từ project tham khảo vào `Assets/Animations/Stickman/`

## 2. Tạo Player object trong scene FlipJump

- [x] 2.1 Mở Unity Editor và mở scene main của FlipJump
- [x] 2.2 Tạo empty GameObject tên `StickmanRagdoll` làm root
- [x] 2.3 Tạo child objects theo hierarchy: Head, Body, UpperLeftArm, LowerLeftArm, UpperRightArm, LowerRightArm, LeftLeg, RightLeg (mỗi Leg có UpperLeg và LowerLeg con)
- [x] 2.4 Gắn Rigidbody2D và Collider2D phù hợp vào mỗi body part (tham khảo Main.unity của project tham khảo)
- [x] 2.5 Gắn Sprite Renderer với sprite từ StickmanGraphics vào mỗi body part
- [x] 2.6 Kết nối các body parts bằng HingeJoint2D hoặc FixedJoint2D (tham khảo joint setup trong project tham khảo)

## 3. Gắn Ragdoll Scripts

- [x] 3.1 Gắn `IgnoreCollision.cs` vào root object `StickmanRagdoll`
- [x] 3.2 Gắn `Balance.cs` vào object `Body` với targetRotation = 0, assign Rigidbody2D của Body
- [x] 3.3 Gắn `Balance.cs` vào object `Head` nếu cần, với targetRotation phù hợp
- [x] 3.4 Xác nhận KHÔNG có script `Movement.cs` nào được gắn vào bất kỳ object nào

## 4. Tạo Prefab và kiểm tra

- [x] 4.1 Tạo thư mục `Assets/Prefabs/` trong project FlipJump
- [x] 4.2 Kéo object `StickmanRagdoll` từ scene vào `Assets/Prefabs/` để tạo prefab
- [ ] 4.3 Kiểm tra prefab: chạy Play mode, stickman rơi theo gravity
- [ ] 4.4 Kiểm tra Balance hoạt động: stickman không bị lật hoàn toàn
- [ ] 4.5 Kiểm tra IgnoreCollision: các limb không nảy vào nhau
- [ ] 4.6 Kiểm tra stickman không tự di chuyển khi nhấn phím bất kỳ
