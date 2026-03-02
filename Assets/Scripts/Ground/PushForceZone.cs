using System.Collections.Generic;
using UnityEngine;

public class PushForceZone : MonoBehaviour
{
    public float forceX = 10f;
    public float zoneWidth = 5f;
    public float zoneHeight = 3f;
    public float maxVelocityX = 15f;
    public LayerMask playerLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryPush();
    }

    private void TryPush()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(zoneWidth, zoneHeight), 0f, playerLayer);

        HashSet<Transform> roots = new HashSet<Transform>();
        foreach (Collider2D hit in hits)
            roots.Add(hit.transform.root);

        foreach (Transform root in roots)
        {
            Rigidbody2D[] rbs = root.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rb in rbs)
            {
                rb.AddForce(new Vector2(forceX, 0f), ForceMode2D.Impulse);
                Vector2 v = rb.velocity;
                v.x = Mathf.Clamp(v.x, -maxVelocityX, maxVelocityX);
                rb.velocity = v;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(zoneWidth, zoneHeight, 0f));
    }
}
