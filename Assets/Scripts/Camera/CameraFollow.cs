using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetX = 0f;
    public float smoothTime = 0.3f;

    private float velocityX;
    private bool hasMaxX;
    private float maxX;
    private bool sliding;
    private float slideTargetX;

    public void SetMaxX(float x)
    {
        hasMaxX = true;
        maxX = x;
    }

    public void SlideTo(float x)
    {
        sliding = true;
        slideTargetX = x;
    }

    private void LateUpdate()
    {
        float newX;

        if (sliding)
        {
            newX = Mathf.SmoothDamp(transform.position.x, slideTargetX, ref velocityX, smoothTime);
        }
        else
        {
            if (target == null) return;
            float targetX = target.position.x + offsetX;
            newX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocityX, smoothTime);
        }

        if (hasMaxX && newX > maxX)
            newX = maxX;

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
