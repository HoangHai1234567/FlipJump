## Context

Prefab `creeper_1` đã có sẵn: `CircleCollider2D` (isTrigger), child `Pivot` (Transform). Player ragdoll `StickmanRagdollV3` có Head nối Body qua `HingeJoint2D`.

## Goals / Non-Goals

**Goals:**
- Khi player chạm creeper → attach vào AttachPoint → di chuyển theo cung tròn từ A đến B
- Đến B thì detach (thả player ra)
- Gizmo + handle kéo thả A, B trên đường tròn trong Editor

**Non-Goals:**
- Hiệu ứng hình ảnh (stretch, animate creeper)
- Gây damage cho player
- Lắc qua lại (pendulum) — chỉ đi 1 chiều A→B

## Decisions

**1. Script CreeperAttach trên prefab creeper_1**

Fields:
- `Transform pivot` — child Pivot (tâm đường tròn)
- `float radius` — bán kính, tính từ Pivot (điều chỉnh trong Inspector)
- `float angleA, angleB` — góc (degrees) của 2 điểm trên đường tròn
- `float moveSpeed` — tốc độ di chuyển (degrees/second)
- `LayerMask playerLayer`

Flow:
1. `OnTriggerEnter2D`: detect player → set `isAttached = true`, freeze ragdoll, parent player vào creeper, đặt player tại vị trí A trên cung
2. `Update`: nếu attached, di chuyển `currentAngle` từ `angleA` → `angleB` theo `moveSpeed`
3. Đạt `angleB` → detach: unparent, unfreeze ragdoll, `isAttached = false`

Vị trí trên cung:
```
x = pivot.x + radius * cos(angle * Deg2Rad)
y = pivot.y + radius * sin(angle * Deg2Rad)
```

**2. Attach/Detach cơ chế**
- Attach: tìm Head-Body joint point (AttachPoint), parent player root vào creeper, freeze tất cả Rigidbody2D
- Detach: unparent, unfreeze Rigidbody2D

**3. Custom Editor CreeperAttachEditor**
- `OnSceneGUI`: vẽ disc handles cho A và B, snap về circle
- CreeperAttach có `OnDrawGizmosSelected`: wire circle, sphere A (xanh), sphere B (đỏ), arc A→B

## Risks / Trade-offs

- [Player jitter khi attached] → Freeze tất cả Rigidbody2D để tránh physics conflict
- [Hướng detach] → Có thể thêm lực tiếp tuyến tại B khi release (optional)

## Files

| File | Action |
|------|--------|
| `Scripts/Obstacles/CreeperAttach.cs` | Tạo mới |
| `Editor/CreeperAttachEditor.cs` | Tạo mới |
| `Prefabs/Demo Prefab/Obstacles/creeper_1.prefab` | Gắn CreeperAttach script |
