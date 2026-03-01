## 1. Refactor ForcePoint.cs — đổi API lực

- [x] 1.1 Xóa field `forceDirection` (Vector2) và `forceMagnitude` (float)
- [x] 1.2 Thêm `public float forceX = 5f` và `public float forceY = 12f`
- [x] 1.3 Thêm `public float groundCheckRadius = 0.3f`
- [x] 1.4 Cập nhật Gizmo `DrawArrow()`: dùng vector `new Vector2(forceX, forceY)` thay vì `forceDirection.normalized * forceMagnitude`
- [x] 1.5 Cập nhật Handle `DrawHandle()`: khi kéo, set `forceX = delta.x` và `forceY = delta.y` (không normalize)

## 2. Thêm Ground Detection

- [x] 2.1 Thêm field `public LayerMask groundLayer` và `private bool isGrounded`
- [x] 2.2 Implement `private bool CheckGrounded()`: dùng `Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer)` trả về true/false
- [x] 2.3 Trong `Update()`: set `isGrounded = CheckGrounded()` mỗi frame
- [x] 2.4 Vẽ Gizmo ground check circle (màu xanh khi grounded, đỏ khi không) trong `OnDrawGizmos()`

## 3. Implement Grounded Jump Logic

- [x] 3.1 Trong `ApplyForce()`: thêm guard `if (!isGrounded) return;`
- [x] 3.2 Trước khi apply lực: gọi `UnfreezeAll()` — set `constraints = RigidbodyConstraints2D.None` cho tất cả Rigidbody2D con
- [x] 3.3 Apply force: `targetRigidbody.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse)`
- [x] 3.4 Implement `private void UnfreezeAll()`: `GetComponentsInChildren<Rigidbody2D>()` rồi set None cho từng cái

## 4. Implement Landing Lock

- [x] 4.1 Thêm field `public string groundTag = "Ground"` để detect landing
- [x] 4.2 Implement `private void OnCollisionEnter2D(Collision2D col)` trên script này (gắn vào ForcePoint, nhưng Body mới va chạm — cần gắn thêm script landing lên Body)
- [x] 4.3 Tạo file `Assets/Scripts/Ragdoll/GroundLanding.cs`: script đơn giản gắn vào Body, khi `OnCollisionEnter2D` với tag "Ground" → gọi `ForcePoint.FreezeAll()` trên root
- [x] 4.4 Thêm method `public void FreezeAll()` trong `ForcePoint.cs`: set `constraints = RigidbodyConstraints2D.FreezeAll` và `velocity = Vector2.zero` cho tất cả Rigidbody2D con
- [x] 4.5 Thêm method `public void FreezeAll()` trong `ForcePoint.cs`: reset velocity toàn bộ limb

## 5. Cập nhật Prefab StickmanRagdoll

- [x] 5.1 Tăng `m_GravityScale` của Body Rigidbody2D (fileID 264808334) từ 1 lên 2 trong prefab YAML
- [x] 5.2 Tăng `m_GravityScale` của các limb Rigidbody2D lên 1.5 trong prefab YAML
- [x] 5.3 Cập nhật MonoBehaviour ForcePoint trong prefab: xóa `forceDirection/forceMagnitude`, thêm `forceX: 5`, `forceY: 12`, `groundCheckRadius: 0.3`
- [x] 5.4 Thêm `GroundLanding` MonoBehaviour vào Body GameObject trong prefab (cần thêm YAML block mới)
- [ ] 5.5 Tạo Layer "Ground" trong Unity và assign cho ground objects (thực hiện trong Editor)

## 6. Kiểm tra trong Unity Editor

- [ ] 6.1 Import scripts, kiểm tra không có lỗi compile
- [ ] 6.2 Tạo một platform với tag "Ground" trong scene
- [ ] 6.3 Đặt StickmanRagdoll lên platform, chạy Play mode
- [ ] 6.4 Kiểm tra gizmo: vòng tròn ground check màu xanh khi đứng trên platform
- [ ] 6.5 Click chuột — nhân vật nhảy sang phải và lên theo parabol
- [ ] 6.6 Khi landing — nhân vật đứng yên, không trượt
- [ ] 6.7 Click khi đang trên không — không có gì xảy ra
