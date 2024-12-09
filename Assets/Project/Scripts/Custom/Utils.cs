using BSGames.Modules.GroundCheck;
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

    public static void CauseFracture(this Component self, float scale = 1f, bool copy = true)
    {
        var temp = new GameObject("Fragments");
        var transform = self.transform;

        // Fracture a copy of an object
        self = !copy ? self : Object.Instantiate(
            self, transform.position, transform.rotation);

        self.transform.localScale = transform.localScale * scale;
        self.transform.parent = temp.transform;
        self.tag = "Break";

        self.gameObject.GetComponent<Rigidbody>().mass /= 100f;
        self.GetComponent<Fracture>().CauseFracture();
        Object.Destroy(temp, 1f);

        // Restore position of a ground checking script
        if (self.GetComponent<GroundCheck>())
            self.GetComponent<GroundCheck>().GroundChecker.SetTransform(transform);
    }
}