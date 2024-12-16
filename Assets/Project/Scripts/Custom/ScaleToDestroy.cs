using UnityEngine;

public class ScaleToDestroy : MonoBehaviour
{
    public float speed = 0.001f;

    protected void FixedUpdate()
    {
        ApplyLogic(transform);
    }

    protected void ApplyLogic(Transform @object)
    {
        @object.localScale -= Vector3.one * speed;
        if (@object.localScale.x <= 0.01f) Destroy(gameObject);
    }
}