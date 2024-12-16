using UnityEngine;

public class ScaleToDestroy : MonoBehaviour
{
    public float speed = 0.05f;

    protected void FixedUpdate()
    {
        ApplyLogic(transform);
    }

    protected void ApplyLogic(Transform @object)
    {
        if (@object.gameObject.GetComponent<MeshFilter>().mesh.vertexCount == 0)
            Destroy(gameObject);
        @object.localScale -= Vector3.one * speed;
        if (@object.localScale.x <= 0f) Destroy(gameObject);
    }
}