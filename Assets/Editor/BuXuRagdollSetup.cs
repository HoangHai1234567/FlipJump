using System.Collections.Generic;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

public class BuXuRagdollSetup : EditorWindow
{
    [MenuItem("Tools/Setup Bu Xu Ragdoll")]
    public static void Setup()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogError("[BuXuRagdollSetup] Select the Bu Xu root GameObject first.");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(selected, "Setup Bu Xu Ragdoll");

        int ragdollLayer = LayerMask.NameToLayer("Ragdoll");
        if (ragdollLayer < 0) ragdollLayer = 6;

        // Remove root collider if any
        CapsuleCollider2D rootCol = selected.GetComponent<CapsuleCollider2D>();
        if (rootCol != null) Undo.DestroyObjectImmediate(rootCol);

        // Make root RB2D kinematic (anchor only)
        Rigidbody2D rootRb = selected.GetComponent<Rigidbody2D>();
        if (rootRb == null) rootRb = Undo.AddComponent<Rigidbody2D>(selected);
        rootRb.bodyType = RigidbodyType2D.Kinematic;

        // Bone config: name -> (mass, gravityScale, collider type, collider size, joint parent, joint lower angle, joint upper angle)
        // Mapped from SpineBoy bone names to Bu Xu bone names
        var boneConfigs = new List<BoneConfig>
        {
            //                  boneName       mass  colliderType           colliderSize          jointParent   lower  upper
            new BoneConfig("hip",          0.8f, ColliderType.Capsule, new Vector2(0.6f, 0.8f),  null,         0,     0),
            new BoneConfig("body",         0.6f, ColliderType.Capsule, new Vector2(0.5f, 0.6f),  "hip",        -30,   30),   // = abdomen
            new BoneConfig("body2",        0.8f, ColliderType.Capsule, new Vector2(0.7f, 0.8f),  "body",       -20,   20),   // = chest
            new BoneConfig("Head",         0.5f, ColliderType.Circle,  new Vector2(0.5f, 0f),    "body2",      -53,   30),   // = head
            new BoneConfig("R_arm",        0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.8f),  "body2",      -90,   90),   // = R-arm
            new BoneConfig("R_farm",       0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.7f),  "R_arm",      0,     150),  // = R-forearm
            new BoneConfig("R_arm2",       0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.8f),  "body2",      -90,   90),   // = L-arm
            new BoneConfig("R_farm2",      0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.7f),  "R_arm2",     0,     150),  // = L-forearm
            new BoneConfig("R_thigh",      0.6f, ColliderType.Capsule, new Vector2(0.2f, 1.0f),  "hip",        -55,   90),   // = R-thigh
            new BoneConfig("R_leg",        0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.9f),  "R_thigh",    -10,   130),  // = R-foot
            new BoneConfig("R_thigh2",     0.6f, ColliderType.Capsule, new Vector2(0.2f, 1.0f),  "hip",        -55,   90),   // = L-thigh
            new BoneConfig("R_leg2",       0.6f, ColliderType.Capsule, new Vector2(0.2f, 0.9f),  "R_thigh2",   -10,   130),  // = L-foot
            new BoneConfig("IK_R_leg",     0.4f, ColliderType.Capsule, new Vector2(0.3f, 0.4f),  "R_leg",      -30,   30),   // = R ankle
            new BoneConfig("R_foot",       0.3f, ColliderType.Capsule, new Vector2(0.3f, 0.3f),  "IK_R_leg",   -20,   40),   // = R foot
            new BoneConfig("R_foot2",      0.2f, ColliderType.Capsule, new Vector2(0.2f, 0.2f),  "R_foot",     -10,   20),   // = R toe
            new BoneConfig("IK_L_leg",     0.4f, ColliderType.Capsule, new Vector2(0.3f, 0.4f),  "R_leg2",     -30,   30),   // = L ankle
            new BoneConfig("R_foot3",      0.3f, ColliderType.Capsule, new Vector2(0.3f, 0.3f),  "IK_L_leg",   -20,   40),   // = L foot
            new BoneConfig("R_foot4",      0.2f, ColliderType.Capsule, new Vector2(0.2f, 0.2f),  "R_foot3",    -10,   20),   // = L toe
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
                Debug.LogWarning($"[BuXuRagdollSetup] Bone '{cfg.boneName}' not found, skipping.");
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

        // Switch intermediate bones (no physics, but must not fight ragdoll) to Override mode
        string[] intermediateBones = { "root", "main", "hip2", "hip3", "body3" };
        foreach (string boneName in intermediateBones)
        {
            if (bones.TryGetValue(boneName, out Transform bone))
            {
                SkeletonUtilityBone sub = bone.GetComponent<SkeletonUtilityBone>();
                if (sub != null)
                    sub.mode = SkeletonUtilityBone.Mode.Override;
            }
        }

        // Disable colliders on hip and body2 (chest) — same as SpineBoy
        DisableCollider(bones, "hip");
        DisableCollider(bones, "body2");

        // Add ForcePoint to root
        ForcePoint fp = selected.GetComponent<ForcePoint>();
        if (fp == null) fp = Undo.AddComponent<ForcePoint>(selected);
        fp.bodyLiftEnabled = true;
        if (rbMap.TryGetValue("hip", out Rigidbody2D hipRb))
            fp.bodyLiftRigidbody = hipRb;
        fp.bodyLiftForceY = 8f;
        fp.spinTorque = 0f;

        // Add HeadCollision to Head
        if (bones.TryGetValue("Head", out Transform headBone))
        {
            HeadCollision hc = headBone.GetComponent<HeadCollision>();
            if (hc == null) Undo.AddComponent<HeadCollision>(headBone.gameObject);
        }

        // Add IgnoreCollision to root (prevents ragdoll bones from colliding with each other)
        IgnoreCollision ic = selected.GetComponent<IgnoreCollision>();
        if (ic == null) Undo.AddComponent<IgnoreCollision>(selected);

        // Set root layer
        selected.layer = ragdollLayer;

        EditorUtility.SetDirty(selected);
        Debug.Log("[BuXuRagdollSetup] Done! Added ragdoll physics to Bu Xu. Fine-tune collider sizes and joint limits as needed.");
    }

    private static void DisableCollider(Dictionary<string, Transform> bones, string boneName)
    {
        if (bones.TryGetValue(boneName, out Transform bone))
        {
            Collider2D col = bone.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
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
