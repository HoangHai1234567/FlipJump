## 1. CreeperAttach Script

- [x] 1.1 Tạo `Scripts/Obstacles/CreeperAttach.cs` với fields: pivot, radius, angleA, angleB, moveSpeed, playerLayer
- [x] 1.2 Implement OnTriggerEnter2D: detect player → freeze ragdoll → parent player → set currentAngle = angleA
- [x] 1.3 Implement Update: di chuyển currentAngle từ A→B, tính vị trí player trên cung tròn
- [x] 1.4 Implement detach: khi đạt angleB → unparent player, unfreeze ragdoll
- [x] 1.5 Implement OnDrawGizmosSelected: vẽ wire circle, sphere A (xanh), sphere B (đỏ), arc A→B

## 2. Custom Editor

- [x] 2.1 Tạo `Editor/CreeperAttachEditor.cs` với OnSceneGUI handles cho A và B
- [x] 2.2 Handle kéo A/B snap về đường tròn (tính angle từ vị trí handle)

## 3. Prefab Setup

- [x] 3.1 Gắn CreeperAttach vào prefab creeper_1, assign Pivot reference
- [x] 3.2 Set default values: radius, angleA, angleB, moveSpeed, playerLayer

## 4. Test

- [ ] 4.1 Play test: player chạm creeper → attach → di chuyển A→B → detach
- [ ] 4.2 Kiểm tra Gizmo + handle A/B trong Editor
