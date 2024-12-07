using UnityEngine;

public static class Utils
{
    public static float MeshSize(this Component component)
    {
        var result = component.GetComponent<MeshFilter>()
            .sharedMesh.bounds.size.magnitude;
        return result;
    }

    public static float Scale(this Component component, bool uniform = true)
    {
        var result = uniform
            ? component.transform.lossyScale.x
            : component.transform.lossyScale.magnitude;
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