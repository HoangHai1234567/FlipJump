using System.Collections.Generic;
using UnityEngine;

public class GroundLiftZone : MonoBehaviour
{
    public float liftForceY = 10f;
    public float zoneHeight = 3f;
    public float zoneWidth = 5f;
    public LayerMask playerLayer;

    private BoxCollider2D groundCollider;

    private void Awake()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        if (groundCollider != null)
            zoneWidth = groundCollider.bounds.size.x;
    }

    private void Update()
    {
        if (InputGate.locked) return;
        if (Input.GetMouseButtonDown(0))
            TryLift();
    }

    private void TryLift()
    {
        Vector2 center = GetZoneCenter();
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, new Vector2(zoneWidth, zoneHeight), 0f, playerLayer);

        HashSet<Transform> roots = new HashSet<Transform>();
        foreach (Collider2D hit in hits)
            roots.Add(hit.transform.root);

        float groundTop = groundCollider != null ? groundCollider.bounds.max.y : transform.position.y;

        foreach (Transform root in roots)
        {
            float heightAboveGround = root.position.y - groundTop;
            float liftScale = 1f - Mathf.Clamp01(heightAboveGround / zoneHeight);

            Rigidbody2D[] rbs = root.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rb in rbs)
                rb.AddForce(new Vector2(0f, liftForceY * liftScale), ForceMode2D.Impulse);
        }
    }

    private Vector2 GetZoneCenter()
    {
        if (groundCollider != null)
            return new Vector2(groundCollider.bounds.center.x, groundCollider.bounds.max.y + zoneHeight * 0.5f);
        return (Vector2)transform.position + Vector2.up * zoneHeight * 0.5f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GetZoneCenter(), new Vector3(zoneWidth, zoneHeight, 0f));
    }
}
