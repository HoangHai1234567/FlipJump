## 1. Tạo script ForcePoint.cs

- [x] 1.1 Tạo file `Assets/Scripts/Ragdoll/ForcePoint.cs` với class `ForcePoint : MonoBehaviour`
- [x] 1.2 Thêm các field: `public Rigidbody2D targetRigidbody`, `public Vector2 forceDirection`, `public float forceMagnitude`
- [x] 1.3 Implement method `public void ApplyForce()` — apply `ForceMode2D.Impulse` lên targetRigidbody với vector `forceDirection.normalized * forceMagnitude`, null-check trước khi apply

## 2. Implement Gizmo và Editor Handle

- [x] 2.1 Thêm `OnDrawGizmos()`: vẽ mũi tên màu vàng nhạt (`Color(1f, 1f, 0f, 0.4f)`) từ `transform.position` theo hướng `forceDirection.normalized * forceMagnitude`
- [x] 2.2 Thêm `OnDrawGizmosSelected()`: vẽ mũi tên màu vàng sáng (`Color.yellow`) khi object được select
- [x] 2.3 Thêm `#if UNITY_EDITOR` guard và `using UnityEditor;` bên trong guard
- [x] 2.4 Trong `OnDrawGizmosSelected()`, dùng `Handles.FreeMoveHandle` tại vị trí đầu mũi tên (`transform.position + (Vector3)(forceDirection.normalized * forceMagnitude)`)
- [x] 2.5 Khi handle bị kéo (vị trí thay đổi): tính vector từ `transform.position` đến vị trí handle mới, cập nhật `forceMagnitude = vector.magnitude`, `forceDirection = vector.normalized`
- [x] 2.6 Wrap handle logic trong `EditorGUI.BeginChangeCheck()` / `EndChangeCheck()` và gọi `Undo.RecordObject` để hỗ trợ Ctrl+Z

## 3. Cập nhật Prefab StickmanRagdoll

- [x] 3.1 Thêm child object `ForcePoint` vào prefab `Assets/Prefabs/StickmanRagdoll.prefab` (thêm YAML block cho GameObject + Transform mới)
- [x] 3.2 Gắn MonoBehaviour `ForcePoint.cs` vào child object đó, assign `targetRigidbody` = Body's Rigidbody2D (fileID: 264808334)
- [x] 3.3 Set `forceDirection = (0, 1)` (hướng lên), `forceMagnitude = 10` làm giá trị mặc định

## 4. Kiểm tra trong Unity Editor

- [ ] 4.1 Mở Unity, để nó import `ForcePoint.cs`
- [ ] 4.2 Kéo `StickmanRagdoll` prefab vào scene, kiểm tra ForcePoint child object tồn tại
- [ ] 4.3 Select `ForcePoint` trong scene — kiểm tra mũi tên Gizmo hiển thị đúng hướng/độ lớn
- [ ] 4.4 Kéo handle đầu mũi tên — kiểm tra `forceDirection` và `forceMagnitude` cập nhật trong Inspector real-time
- [ ] 4.5 Chạy Play mode, gọi `ApplyForce()` qua Debug hoặc test script — kiểm tra nhân vật bật lên theo hướng mũi tên
