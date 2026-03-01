## 1. Sửa ForcePoint.cs

- [x] 1.1 Thêm field `public float spinTorque = 0f`
- [x] 1.2 Trong `ApplyForce()`: sau block `bodyLiftEnabled`, thêm block `if (spinTorque != 0f)` → null-check `bodyLiftRigidbody` → gọi `bodyLiftRigidbody.AddTorque(spinTorque, ForceMode2D.Impulse)`

## 2. Cập nhật StickmanRagdollV2.prefab

- [x] 2.1 Trong ForcePoint MonoBehaviour của V2: thêm `spinTorque: -200` (âm = CW khi nhảy sang phải)

## 3. Kiểm tra trong Unity Editor

- [ ] 3.1 Import script, kiểm tra không có lỗi compile
- [ ] 3.2 Kiểm tra ForcePoint Inspector trên StickmanRagdollV2: thấy field `spinTorque`
- [ ] 3.3 Play mode, tap — nhân vật xoay mạnh quanh vùng hông, không ảnh hưởng lực nâng/đẩy
- [ ] 3.4 Tune `spinTorque`: tăng magnitude nếu xoay chưa đủ mạnh; đổi dấu nếu sai chiều
