using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    private bool triggered;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;
        if (InputGate.locked) return;
        if (!collision.gameObject.CompareTag("Ground")) return;

        triggered = true;
        InputGate.locked = true;

        ForcePoint fp = transform.root.GetComponentInChildren<ForcePoint>();
        if (fp != null)
            fp.FreezeAll();

        if (GameManager.Instance != null)
            GameManager.Instance.Lose();
    }
}
