using UnityEngine;

public class LiftPoint : MonoBehaviour
{
    public float liftForceY = 1f;
    public float maxLiftHeight = 5f;
    public Rigidbody2D bodyRigidbody;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (bodyRigidbody == null) return;

        float height = bodyRigidbody.transform.position.y - transform.position.y;
        float liftScale = 1f - Mathf.Clamp01(height / maxLiftHeight);
        if (liftScale > 0f)
            LiftAll(liftScale);
    }

    private void LiftAll(float scale)
    {
        Rigidbody2D[] rbs = transform.root.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rb in rbs)
        {
            rb.AddForce(new Vector2(0f, liftForceY * scale), ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        float height = bodyRigidbody != null
            ? bodyRigidbody.transform.position.y - transform.position.y
            : 0f;
        Gizmos.color = height < maxLiftHeight
            ? new Color(0f, 1f, 0f, 0.3f)
            : new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxLiftHeight);
    }

    private void OnDrawGizmosSelected()
    {
        float height = bodyRigidbody != null
            ? bodyRigidbody.transform.position.y - transform.position.y
            : 0f;
        Gizmos.color = height < maxLiftHeight ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxLiftHeight);
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
