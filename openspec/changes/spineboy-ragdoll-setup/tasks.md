## 1. Create Editor Setup Script

- [x] 1.1 Create `Assets/Editor/SpineBoyRagdollSetup.cs` — an Editor script with menu item `Tools > Setup SpineBoy Ragdoll` that:
  - Finds SpineBoy prefab or selected GameObject
  - Removes root CapsuleCollider2D (keep root RB2D as kinematic)
  - For each major bone (hip, abdomen, chest, head, L/R-arm, L/R-forearm, L/R-thigh, L/R-foot): adds Rigidbody2D (gravityScale 0.4, mass per design table), adds collider (Circle for head, Capsule for others)
  - Adds HingeJoint2D linking child bone to parent bone with angle limits
  - Switches SkeletonUtilityBone mode to Override (1) on physics bones
  - Sets all bones to Layer 6
  - Adds ForcePoint to root (bodyLiftEnabled=true, bodyLiftRigidbody=hip RB2D)
  - Adds HeadCollision to head bone

## 2. Run and Verify

- [ ] 2.1 Open SpineBoy prefab in Unity, run `Tools > Setup SpineBoy Ragdoll`, verify components are added correctly
- [ ] 2.2 Fine-tune collider sizes and joint angle limits in the prefab to match SpineBoy's visual proportions
- [ ] 2.3 Test in Play mode: verify ragdoll falls, click applies force, head-ground triggers lose
