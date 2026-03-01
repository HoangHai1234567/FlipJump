## 1. Tạo LiftPoint.cs

- [x] 1.1 Tạo `Assets/Scripts/Ragdoll/LiftPoint.cs` với fields: `public float liftForceY = 1f`, `public float groundCheckRadius = 0.3f`, `public LayerMask groundLayer`
- [x] 1.2 Implement `Update()`: check grounded + `Input.GetMouseButtonDown(0)` → gọi `LiftAll()`
- [x] 1.3 Implement `LiftAll()`: `transform.root.GetComponentsInChildren<Rigidbody2D>()` → foreach apply `AddForce(new Vector2(0f, liftForceY), ForceMode2D.Impulse)`
- [x] 1.4 Implement `CheckGrounded()`: `Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer)`
- [x] 1.5 Implement `OnDrawGizmos()` / `OnDrawGizmosSelected()`: vẽ wire sphere, xanh = grounded, đỏ = không

## 2. Thêm LiftPoint vào StickmanRagdollV2.prefab

- [x] 2.1 Tạo mới fileID cho LiftPoint GameObject và Transform trong YAML V2
- [x] 2.2 Tạo MonoBehaviour entry cho LiftPoint.cs component (cần GUID của script mới)
- [x] 2.3 Gắn LiftPoint GameObject vào root (`m_Father: {fileID: 1080167038}`)
- [x] 2.4 Tắt body lift trên ForcePoint V2: set `bodyLiftEnabled: 0`

## 3. Kiểm tra trong Unity Editor

- [ ] 3.1 Import script, kiểm tra không có lỗi compile
- [ ] 3.2 Kiểm tra LiftPoint Inspector: thấy `liftForceY`, `groundCheckRadius`, `groundLayer`
- [ ] 3.3 Play mode, tap — toàn bộ ragdoll nảy lên đồng đều
- [ ] 3.4 Tune `liftForceY` (default `1f` cho ~10 parts ≈ tổng 10 đơn vị lực)
