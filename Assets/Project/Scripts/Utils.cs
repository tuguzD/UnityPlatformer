using UnityEngine;

public static class Utils
{
    public static float Magnitude(this Component component)
    {
        var result = component.gameObject.GetComponent<MeshFilter>()
            .sharedMesh.bounds.size.magnitude;

        // Debug.Log(result);
        return result;
    }
}