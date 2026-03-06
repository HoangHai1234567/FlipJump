## 1. Level Hierarchy Setup

- [x] 1.1 Create "Level" root GameObject in the scene with children "Ground", "Platforms", "Obstacles"
- [x] 1.2 Remove manually placed level objects from scene (LevelLoader will spawn them from JSON)

## 2. Ground Segmentation & Gaps

- [x] 2.1 Split the single Square ground (scale 96x1) into 3+ smaller ground segments with gaps between them
- [x] 2.2 Ensure each ground segment has the "Ground" tag and is on the ground layer
- [x] 2.3 Add GroundLiftZone to each ground segment (auto-sized width)
- [x] 2.4 Create gaps of varying widths: narrow (1-2 units), medium (3 units), wide (4-6 units)

## 3. Obstacle Prefabs

- [x] 3.1 Create Assets/Prefabs/Obstacles/ folder
- [x] 3.2 Create static obstacle prefab: Fridge (SpriteRenderer + BoxCollider2D, no Rigidbody2D)
- [x] 3.3 Create static obstacle prefab: Table (SpriteRenderer + BoxCollider2D, no Rigidbody2D)
- [x] 3.4 Create knockable obstacle prefab: Bar Stool (SpriteRenderer + BoxCollider2D + Rigidbody2D)
- [x] 3.5 Create knockable obstacle prefab: Microwave (SpriteRenderer + BoxCollider2D + Rigidbody2D)
- [x] 3.6 Fit BoxCollider2D to match sprite bounds on each obstacle prefab

## 4. Elevated Platform Prefabs

- [x] 4.1 Create Assets/Prefabs/Platforms/ folder
- [x] 4.2 Create Platform prefab (SpriteRenderer + BoxCollider2D, "Ground" tag, ground layer, no Rigidbody2D)
- [x] 4.3 Verify GroundLanding detects platform collision (tag = "Ground")
- [x] 4.4 Verify ForcePoint.CheckGrounded() works on platform (groundLayer match)

## 5. JSON Level System

- [x] 5.1 Create Assets/Levels/ folder
- [x] 5.2 Create LevelData classes (LevelData, LevelElement) as serializable C# classes for JsonUtility
- [x] 5.3 Create PrefabEntry serializable class with `name` (string) and `prefab` (GameObject) fields
- [x] 5.4 Create LevelLoader.cs MonoBehaviour with: serialized TextAsset for JSON file, PrefabEntry[] array for prefab registry, references to Level containers (Ground, Platforms, Obstacles)
- [x] 5.5 Implement LevelLoader.LoadLevel(): clear existing children, parse JSON, instantiate prefabs at positions/scales/rotations under correct containers
- [x] 5.6 Implement auto-tagging: set "Ground" tag and ground layer on spawned ground segments and platforms
- [x] 5.7 Add warning log for unknown prefab names (skip element, don't crash)
- [x] 5.8 Add LevelLoader to a GameObject in the scene and configure prefab registry in Inspector

## 6. Level JSON Authoring

- [x] 6.1 Create Assets/Levels/level1.json with ground segments (3+ segments with gaps of varying widths)
- [x] 6.2 Add obstacles to level1.json (static fridge/table as barriers, knockable stool/microwave)
- [x] 6.3 Add elevated platforms at 2+ different heights to level1.json
- [x] 6.4 Add PushForce zone before a wide gap to assist momentum
- [x] 6.5 Ensure starting area (leftmost) is flat with no obstacles

## 7. Verification

- [x] 7.1 Play-test: LevelLoader spawns all elements from JSON on scene start
- [ ] 7.2 Play-test: ragdoll lands on platforms and freezes correctly
- [ ] 7.3 Play-test: ragdoll falls through gaps
- [ ] 7.4 Play-test: static obstacles block ragdoll, knockable obstacles react to impact
- [ ] 7.5 Play-test: camera follows ragdoll through the full level
- [ ] 7.6 Verify: unknown prefab name in JSON logs warning and doesn't crash
