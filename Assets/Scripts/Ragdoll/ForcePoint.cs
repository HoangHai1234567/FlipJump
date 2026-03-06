using UnityEngine;

public class ForcePoint : MonoBehaviour
{
    // Task 1.2: Leaf limbs to select from (end-effectors)
    public Transform[] leafLimbs;

    // Task 1.3: Fallback when leafLimbs is empty
    public Rigidbody2D fallbackRigidbody;

    // Body lift (optional, enabled in V2 prefab)
    public bool bodyLiftEnabled = false;
    public Rigidbody2D bodyLiftRigidbody;
    public float bodyLiftForceY = 8f;
    public float spinTorque = 0f;

    private void Update()
    {
        if (InputGate.locked) return;
        if (Input.GetMouseButtonDown(0))
            ApplyForce();
    }

    private Rigidbody2D FindLeftmostLeaf()
    {
        if (leafLimbs == null || leafLimbs.Length == 0)
            return fallbackRigidbody;

        Transform leftmost = null;
        float minX = float.MaxValue;

        foreach (Transform t in leafLimbs)
        {
            if (t == null) continue;
            if (t.position.x < minX)
            {
                minX = t.position.x;
                leftmost = t;
            }
        }

        if (leftmost == null) return fallbackRigidbody;
        return leftmost.GetComponent<Rigidbody2D>();
    }

    public void ApplyForce()
    {
        UnfreezeAll();

        if (bodyLiftEnabled)
        {
            if (bodyLiftRigidbody == null)
            {
                Debug.LogWarning("[ForcePoint] bodyLiftEnabled but bodyLiftRigidbody is not assigned.");
            }
            else
            {
                bodyLiftRigidbody.AddForce(new Vector2(0f, bodyLiftForceY), ForceMode2D.Impulse);
            }
        }

        if (spinTorque != 0f)
        {
            if (bodyLiftRigidbody == null)
            {
                Debug.LogWarning("[ForcePoint] spinTorque != 0 but bodyLiftRigidbody is not assigned.");
            }
            else
            {
                bodyLiftRigidbody.AddTorque(spinTorque, ForceMode2D.Impulse);
            }
        }

    }

    private void UnfreezeAll()
    {
        Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rb in rbs)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    public void FreezeAll()
    {
        Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rb in rbs)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

}
