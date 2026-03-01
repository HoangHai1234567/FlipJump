## 1. Sửa ForcePoint.cs — thêm body lift

- [x] 1.1 Thêm 3 fields: `public bool bodyLiftEnabled`, `public Rigidbody2D bodyLiftRigidbody`, `public float bodyLiftForceY = 8f`
- [x] 1.2 Trong `ApplyForce()`: sau khi apply lực spin, thêm block `if (bodyLiftEnabled)` → null-check `bodyLiftRigidbody` → apply `AddForce(new Vector2(0, bodyLiftForceY), ForceMode2D.Impulse)`

## 2. Tạo StickmanRagdollV2.prefab

- [x] 2.1 Copy file `Assets/Prefabs/StickmanRagdoll.prefab` → `Assets/Prefabs/StickmanRagdollV2.prefab`
- [x] 2.2 Copy file `Assets/Prefabs/StickmanRagdoll.prefab.meta` → tạo `.meta` mới cho V2 với GUID mới
- [x] 2.3 Trong YAML của V2: sửa tên prefab (m_Name) thành `StickmanRagdollV2`
- [x] 2.4 Trong ForcePoint MonoBehaviour của V2: thêm `bodyLiftEnabled: 1`, `bodyLiftRigidbody: {fileID: 264808334}`, `bodyLiftForceY: 8`

## 3. Kiểm tra trong Unity Editor

- [ ] 3.1 Import scripts + prefab, kiểm tra không có lỗi compile
- [ ] 3.2 Kiểm tra ForcePoint Inspector trên StickmanRagdollV2: thấy `bodyLiftEnabled`, `bodyLiftRigidbody`, `bodyLiftForceY`
- [ ] 3.3 Kéo `StickmanRagdollV2` vào scene, Play mode, tap — nhân vật nhảy cao hơn và xoay vòng
- [ ] 3.4 Kiểm tra prefab gốc `StickmanRagdoll` không bị ảnh hưởng (`bodyLiftEnabled = false`)
