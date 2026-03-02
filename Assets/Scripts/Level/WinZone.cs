using System.Collections;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public GameObject celebratePrefab;
    public Transform celebrateSpawnPoint;
    public float delay = 2f;
    public LayerMask playerLayer;

    private BoxCollider2D boxCollider;
    private bool triggered;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D col = boxCollider != null ? boxCollider : GetComponent<BoxCollider2D>();
        if (col == null) return;

        Vector2 size = col.size * (Vector2)transform.lossyScale;
        Vector3 center = transform.position + (Vector3)(col.offset * (Vector2)transform.lossyScale);

        Gizmos.color = new Color(0.5f, 1f, 0.5f, 0.6f);
        Gizmos.DrawCube(center, new Vector3(size.x, size.y, 0f));
        Gizmos.color = new Color(0.5f, 1f, 0.5f, 0.6f);
        Gizmos.DrawWireCube(center, new Vector3(size.x, size.y, 0f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (((1 << other.gameObject.layer) & playerLayer) == 0) return;

        triggered = true;
        StartCoroutine(WinSequence(other.transform.root));
    }

    private IEnumerator WinSequence(Transform player)
    {
        ForcePoint fp = player.GetComponentInChildren<ForcePoint>();
        if (fp != null)
            fp.FreezeAll();

        yield return new WaitForSeconds(delay);

        player.gameObject.SetActive(false);

        if (celebratePrefab != null)
        {
            Vector3 spawnPos = celebrateSpawnPoint != null ? celebrateSpawnPoint.position : player.position;
            Instantiate(celebratePrefab, spawnPos, Quaternion.identity);
        }
    }
}
