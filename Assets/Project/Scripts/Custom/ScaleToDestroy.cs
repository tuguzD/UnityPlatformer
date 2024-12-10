using UnityEngine;

public class ScaleToDestroy : MonoBehaviour
{
    public float speed = 0.001f;

    private void FixedUpdate()
    {
        transform.localScale -= Vector3.one * speed;
        if (transform.localScale.x <= 0f) Destroy(gameObject);
    }
}