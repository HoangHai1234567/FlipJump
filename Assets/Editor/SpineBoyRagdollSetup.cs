using System.Collections.Generic;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

public class SpineBoyRagdollSetup : EditorWindow
{
    [MenuItem("Tools/Setup SpineBoy Ragdoll")]
    public static void Setup()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogError("[SpineBoyRagdollSetup] Select the SpineBoy root GameObject first.");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(selected, "Setup SpineBoy Ragdoll");

        int ragdollLayer = LayerMask.NameToLayer("Ragdoll");
        if (ragdollLayer < 0) ragdollLayer = 6;

        // Remove root collider (we'll use per-bone colliders)
        CapsuleCollider2D rootCol = selected.GetComponent<CapsuleCollider2D>();
        if (rootCol != null) Undo.DestroyObjectImmediate(rootCol);

        // Make root RB2D kinematic (anchor only)
        Rigidbody2D rootRb = selected.GetComponent<Rigidbody2D>();
        if (rootRb != null)
        {
            rootRb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Bone config: name -> (mass, gravityScale, collider type, collider size, joint parent, joint lower angle, joint upper angle)
        var boneConfigs = new List<BoneConfig>
        {
            new BoneConfig("hip",        0.8f, ColliderType.Capsule, new Vector2(0.6f, 0.8f),  null,       0, 0),
            new BoneConfig("abdomen",    0.6f, ColliderType.Capsule, new Vector2(0.5f, 0.6f),  "hip",      -30, 30),
            new BoneConfig("chest",      0.8f, ColliderType.Capsule, new Vector2(0.7f, 0.8f),  "abdomen",  -20, 20),
            new BoneConfig("head",       0.5f, ColliderType.Circle,  new Vector2(0.5f, 0f),    "chest",    -53, 30),
            new BoneConfig("L-arm",      0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.8f),  "chest",    -90, 90),
            new BoneConfig("L-forearm",  0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.7f),  "L-arm",    0, 150),
            new BoneConfig("R-arm",      0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.8f),  "chest",    -90, 90),
            new BoneConfig("R-forearm",  0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.7f),  "R-arm",    0, 150),
            new BoneConfig("L-thigh",    0.6f, ColliderType.Capsule, new Vector2(0.2f, 1.0f),  "hip",      -55, 90),
            new BoneConfig("L-foot",     0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.9f),  "L-thigh",  -10, 130),
        };

        // Find all bones
        Dictionary<string, Transform> bones = new Dictionary<string, Transform>();
        foreach (Transform t in selected.GetComponentsInChildren<Transform>(true))
        {
            if (!bones.ContainsKey(t.name))
                bones[t.name] = t;
        }

        // Track created RB2Ds for joint linking
        Dictionary<string, Rigidbody2D> rbMap = new Dictionary<string, Rigidbody2D>();

        foreach (var cfg in boneConfigs)
        {
            if (!bones.TryGetValue(cfg.boneName, out Transform bone))
            {
                Debug.LogWarning($"[SpineBoyRagdollSetup] Bone '{cfg.boneName}' not found, skipping.");
                continue;
            }

            // Set layer
            bone.gameObject.layer = ragdollLayer;

            // Add Rigidbody2D
            Rigidbody2D rb = bone.GetComponent<Rigidbody2D>();
            if (rb == null) rb = Undo.AddComponent<Rigidbody2D>(bone.gameObject);
            rb.mass = cfg.mass;
            rb.gravityScale = 0.4f;
            rb.angularDrag = 0.05f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rbMap[cfg.boneName] = rb;

            // Add Collider
            if (cfg.colliderType == ColliderType.Circle)
            {
                CircleCollider2D circle = bone.GetComponent<CircleCollider2D>();
                if (circle == null) circle = Undo.AddComponent<CircleCollider2D>(bone.gameObject);
                circle.radius = cfg.colliderSize.x;
            }
            else
            {
                CapsuleCollider2D capsule = bone.GetComponent<CapsuleCollider2D>();
                if (capsule == null) capsule = Undo.AddComponent<CapsuleCollider2D>(bone.gameObject);
                capsule.size = cfg.colliderSize;
                capsule.direction = CapsuleDirection2D.Vertical;
            }

            // Add HingeJoint2D
            if (cfg.jointParent != null && rbMap.TryGetValue(cfg.jointParent, out Rigidbody2D parentRb))
            {
                HingeJoint2D joint = bone.GetComponent<HingeJoint2D>();
                if (joint == null) joint = Undo.AddComponent<HingeJoint2D>(bone.gameObject);
                joint.connectedBody = parentRb;
                joint.useLimits = true;
                joint.limits = new JointAngleLimits2D
                {
                    min = cfg.jointLowerAngle,
                    max = cfg.jointUpperAngle
                };
                joint.enableCollision = false;
            }

            // Switch SkeletonUtilityBone to Override mode
            SkeletonUtilityBone sub = bone.GetComponent<SkeletonUtilityBone>();
            if (sub != null)
                sub.mode = SkeletonUtilityBone.Mode.Override;
        }

        // Add ForcePoint to root
        ForcePoint fp = selected.GetComponent<ForcePoint>();
        if (fp == null) fp = Undo.AddComponent<ForcePoint>(selected);
        fp.bodyLiftEnabled = true;
        if (rbMap.TryGetValue("hip", out Rigidbody2D hipRb))
            fp.bodyLiftRigidbody = hipRb;
        fp.bodyLiftForceY = 8f;
        fp.spinTorque = 0f;

        // Add HeadCollision to head
        if (bones.TryGetValue("head", out Transform headBone))
        {
            HeadCollision hc = headBone.GetComponent<HeadCollision>();
            if (hc == null) Undo.AddComponent<HeadCollision>(headBone.gameObject);
        }

        // Set root layer
        selected.layer = ragdollLayer;

        EditorUtility.SetDirty(selected);
        Debug.Log("[SpineBoyRagdollSetup] Done! Added ragdoll physics to SpineBoy. Fine-tune collider sizes and joint limits as needed.");
    }

    private enum ColliderType { Circle, Capsule }

    private class BoneConfig
    {
        public string boneName;
        public float mass;
        public ColliderType colliderType;
        public Vector2 colliderSize;
        public string jointParent;
        public float jointLowerAngle;
        public float jointUpperAngle;

        public BoneConfig(string boneName, float mass, ColliderType colliderType, Vector2 colliderSize,
            string jointParent, float jointLowerAngle, float jointUpperAngle)
        {
            this.boneName = boneName;
            this.mass = mass;
            this.colliderType = colliderType;
            this.colliderSize = colliderSize;
            this.jointParent = jointParent;
            this.jointLowerAngle = jointLowerAngle;
            this.jointUpperAngle = jointUpperAngle;
        }
    }
}
