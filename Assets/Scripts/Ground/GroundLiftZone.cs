using System.Collections.Generic;
using UnityEngine;

public class GroundLiftZone : MonoBehaviour
{
    public float liftForceY = 10f;
    public float zoneHeight = 3f;
    public float zoneWidth = 5f;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    private Collider2D groundCollider;

    private void Awake()
    {
        groundCollider = GetComponent<Collider2D>();
        if (groundCollider != null)
            zoneWidth = groundCollider.bounds.size.x;
    }

    private void Update()
    {
        if (InputGate.IsBlocked) return;
        if (Input.GetMouseButtonDown(0))
            TryLift();
    }

    private float GetSurfaceY(float worldX)
    {
        if (groundCollider == null)
            return transform.position.y;

        Vector2 rayOrigin = new Vector2(worldX, groundCollider.bounds.max.y + 1f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCollider.bounds.size.y + 2f, groundLayer);
        if (hit.collider != null && hit.collider == groundCollider)
            return hit.point.y;

        return groundCollider.bounds.max.y;
    }

    private void TryLift()
    {
        Vector2 center = GetZoneCenter();
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, new Vector2(zoneWidth, zoneHeight), 0f, playerLayer);

        HashSet<Transform> roots = new HashSet<Transform>();
        foreach (Collider2D hit in hits)
            roots.Add(hit.transform.root);

        foreach (Transform root in roots)
        {
            float surfaceY = GetSurfaceY(root.position.x);
            float heightAboveGround = root.position.y - surfaceY;

            if (heightAboveGround < 0f || heightAboveGround > zoneHeight)
                continue;

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
        if (groundCollider == null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(GetZoneCenter(), new Vector3(zoneWidth, zoneHeight, 0f));
            return;
        }

        Gizmos.color = Color.green;
        Bounds bounds = groundCollider.bounds;
        int segments = 20;
        float xMin = bounds.min.x;
        float xMax = bounds.max.x;
        float step = (xMax - xMin) / segments;

        // Draw surface-following lift zone outline
        Vector3 prevBottom = Vector3.zero;
        Vector3 prevTop = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float x = xMin + step * i;
            float surfaceY = GetSurfaceY(x);
            Vector3 bottom = new Vector3(x, surfaceY, 0f);
            Vector3 top = new Vector3(x, surfaceY + zoneHeight, 0f);

            if (i > 0)
            {
                Gizmos.DrawLine(prevBottom, bottom);
                Gizmos.DrawLine(prevTop, top);
            }

            prevBottom = bottom;
            prevTop = top;
        }

        // Draw side edges
        float leftSurfaceY = GetSurfaceY(xMin);
        float rightSurfaceY = GetSurfaceY(xMax);
        Gizmos.DrawLine(new Vector3(xMin, leftSurfaceY, 0f), new Vector3(xMin, leftSurfaceY + zoneHeight, 0f));
        Gizmos.DrawLine(new Vector3(xMax, rightSurfaceY, 0f), new Vector3(xMax, rightSurfaceY + zoneHeight, 0f));
    }
}
