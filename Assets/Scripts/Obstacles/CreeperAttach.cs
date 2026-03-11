using UnityEngine;

public class CreeperAttach : MonoBehaviour
{
    [Header("Setup")]
    public Transform pivot;
    public Transform attachPoint;

    [Header("Arc")]
    public float angleA = 30f;
    public float angleB = -30f;
    public float moveSpeed = 120f;
    public float releaseForce = 5f;

    [Header("Detection")]
    public LayerMask playerLayer;

    private bool isAttached;
    private bool isUsed;
    private float currentAngle;
    private Transform attachedPlayer;
    private Transform attachedBody;
    private ForcePoint attachedForcePoint;
    private Rigidbody2D[] attachedRigidbodies;
    private Vector3 pivotWorldPos;

    private void Start()
    {
        pivotWorldPos = pivot.position;
        currentAngle = angleA;
        ApplyRotation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttached || isUsed) return;
        if (((1 << other.gameObject.layer) & playerLayer) == 0) return;

        Transform playerRoot = other.transform.root;
        ForcePoint fp = playerRoot.GetComponentInChildren<ForcePoint>();
        if (fp == null) return;

        isAttached = true;
        attachedPlayer = playerRoot;
        attachedBody = playerRoot.Find("Body");
        attachedForcePoint = fp;
        attachedRigidbodies = playerRoot.GetComponentsInChildren<Rigidbody2D>();
        currentAngle = angleA;

        // Save pivot world position before rotation
        pivotWorldPos = pivot.position;

        fp.FreezeAll();

        ApplyRotation();
        SnapPlayerToAttachPoint();
    }

    private void Update()
    {
        if (!isAttached || attachedPlayer == null) return;

        float dir = Mathf.Sign(angleB - angleA);
        currentAngle += dir * moveSpeed * Time.deltaTime;

        if ((dir > 0 && currentAngle >= angleB) || (dir < 0 && currentAngle <= angleB))
        {
            currentAngle = angleB;
            ApplyRotation();
            SnapPlayerToAttachPoint();
            Detach();
            return;
        }

        ApplyRotation();
        SnapPlayerToAttachPoint();
    }

    private void ApplyRotation()
    {
        if (pivot == null) return;

        // Rotate creeper root around pivot
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        transform.position = pivotWorldPos - transform.rotation * pivot.localPosition;
    }

    private void SnapPlayerToAttachPoint()
    {
        if (attachedPlayer == null || attachPoint == null) return;

        // Align Body (not root) to attachPoint
        if (attachedBody != null)
        {
            Vector3 bodyOffset = attachedBody.position - attachedPlayer.position;
            attachedPlayer.position = attachPoint.position - bodyOffset;
        }
        else
        {
            attachedPlayer.position = attachPoint.position;
        }

        // Sync rigidbody physics positions with transform positions
        // Without this, physics snaps bodies back to their old frozen positions
        SyncRigidbodyPositions();
    }

    private void SyncRigidbodyPositions()
    {
        if (attachedRigidbodies == null) return;
        foreach (var rb in attachedRigidbodies)
        {
            if (rb == null) continue;
            rb.position = rb.transform.position;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void Detach()
    {
        if (attachedPlayer == null) return;

        // Calculate tangent direction at point B (perpendicular to radius, in movement direction)
        Vector3 radial = (attachPoint.position - pivotWorldPos).normalized;
        float dir = Mathf.Sign(angleB - angleA);
        Vector2 tangent = new Vector2(-radial.y, radial.x) * dir;

        SyncRigidbodyPositions();

        // Unfreeze all rigidbodies and apply tangential velocity
        if (attachedRigidbodies != null)
        {
            foreach (var rb in attachedRigidbodies)
            {
                if (rb == null) continue;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity = tangent * releaseForce;
            }
        }

        isAttached = false;
        attachedPlayer = null;
        attachedBody = null;
        attachedForcePoint = null;
        attachedRigidbodies = null;

        isUsed = true;
        GetComponent<Collider2D>().enabled = false;
    }

    public float GetRadius()
    {
        if (pivot == null || attachPoint == null) return 1f;
        return Vector3.Distance(pivot.localPosition, attachPoint.localPosition);
    }

    private void OnDrawGizmosSelected()
    {
        if (pivot == null || attachPoint == null) return;

        Vector3 pivotPos = pivot.position;
        float r = GetRadius();
        Vector3 offset = attachPoint.localPosition - pivot.localPosition;

        // Wire circle (path of AttachPoint)
        Gizmos.color = new Color(1f, 1f, 1f, 0.3f);
        DrawWireCircle(pivotPos, r, 64);

        // AttachPoint position at angle A
        Vector3 posA = pivotPos + (Vector3)(Quaternion.Euler(0, 0, angleA) * offset);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(posA, 0.1f);

        // AttachPoint position at angle B
        Vector3 posB = pivotPos + (Vector3)(Quaternion.Euler(0, 0, angleB) * offset);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(posB, 0.1f);

        // Arc A→B
        Gizmos.color = Color.yellow;
        DrawAttachArc(pivotPos, offset, angleA, angleB, 32);

        // Pivot center
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(pivotPos, 0.08f);
    }

    private static void DrawWireCircle(Vector3 center, float r, int segments)
    {
        float step = 360f / segments;
        Vector3 prev = center + new Vector3(r, 0f, 0f);
        for (int i = 1; i <= segments; i++)
        {
            float ang = i * step * Mathf.Deg2Rad;
            Vector3 next = center + new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0f) * r;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }

    private static void DrawAttachArc(Vector3 pivotPos, Vector3 offset, float fromAngle, float toAngle, int segments)
    {
        float range = toAngle - fromAngle;
        float step = range / segments;
        Vector3 prev = pivotPos + (Vector3)(Quaternion.Euler(0, 0, fromAngle) * offset);
        for (int i = 1; i <= segments; i++)
        {
            float ang = fromAngle + i * step;
            Vector3 next = pivotPos + (Vector3)(Quaternion.Euler(0, 0, ang) * offset);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}
