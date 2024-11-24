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

    public static void UniformScale(this Component component, float value)
    {
        component.transform.localScale = new Vector3(value, value, value);
    }

    public static void SetOpacity(this Material material, float opacity)
    {
        material.color = new Color(
            material.color.r, material.color.g, material.color.b, opacity
        );
    }
}