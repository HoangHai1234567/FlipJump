using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetX = 0f;
    public float smoothTime = 0.3f;

    private float velocityX;

    private void LateUpdate()
    {
        if (target == null) return;

        float targetX = target.position.x + offsetX;
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocityX, smoothTime);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
