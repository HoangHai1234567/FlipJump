using UnityEngine;

// Task 4.3: Attached to Body GameObject — detects landing on ground and freezes all limbs
public class GroundLanding : MonoBehaviour
{
    // Task 4.1: Tag used to identify ground objects
    public string groundTag = "Ground";

    private ForcePoint forcePoint;

    private void Awake()
    {
        // Find ForcePoint on the root of the ragdoll
        forcePoint = GetComponentInParent<ForcePoint>();
    }

    // Task 4.2: When Body hits ground, freeze entire ragdoll
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag(groundTag)) return;
        if (forcePoint == null) return;

        forcePoint.FreezeAll();
    }
}
