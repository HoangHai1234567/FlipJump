## 1. Refactor ForcePoint.cs — đổi sang dynamic target

- [x] 1.1 Xóa field `public Rigidbody2D targetRigidbody`
- [x] 1.2 Thêm `public Transform[] leafLimbs` để cấu hình các bộ phận đầu cuối
- [x] 1.3 Thêm `public Rigidbody2D fallbackRigidbody` (Body) dùng khi leafLimbs rỗng
- [x] 1.4 Implement `private Rigidbody2D FindLeftmostLeaf()`: duyệt `leafLimbs`, tìm Transform có `position.x` nhỏ nhất, trả về `GetComponent<Rigidbody2D>()` của nó; nếu rỗng trả về `fallbackRigidbody`
- [x] 1.5 Trong `ApplyForce()`: thay `targetRigidbody.AddForce(...)` bằng `FindLeftmostLeaf().AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse)`

## 2. Cập nhật Prefab StickmanRagdoll

- [x] 2.1 Trong ForcePoint MonoBehaviour (fileID 500000003): xóa `targetRigidbody`, thêm `fallbackRigidbody: {fileID: 264808334}`
- [x] 2.2 Thêm `leafLimbs` array vào ForcePoint MonoBehaviour trong prefab YAML với 5 entries: Lower Left Arm, Lower Right Arm, Lower Left Leg, Lower Right Leg, Head
- [x] 2.3 Tìm fileID của các Transform: Lower Left Arm (1844977465), Lower Right Arm (425729288), Lower Left Leg (1681722773), Lower Right Leg (291774122), Head (1117775303)

## 3. Kiểm tra trong Unity Editor

- [ ] 3.1 Import scripts, kiểm tra không có lỗi compile
- [ ] 3.2 Kiểm tra ForcePoint Inspector: thấy `leafLimbs` array và `fallbackRigidbody`
- [ ] 3.3 Drag 5 leaf limb Transforms vào `leafLimbs` array trong Inspector (nếu prefab YAML chưa set đúng)
- [ ] 3.4 Chạy Play mode, tap chuột — nhân vật nảy lên và xoay vòng
- [ ] 3.5 Kiểm tra: khi đứng thẳng trên ground, lực apply vào tay/chân bên trái nhất → nhân vật xoay theo chiều kim đồng hồ hoặc ngược lại tự nhiên
- [ ] 3.6 Kiểm tra fallback: tạm xóa leafLimbs array → click vẫn hoạt động (fallback về Body)
