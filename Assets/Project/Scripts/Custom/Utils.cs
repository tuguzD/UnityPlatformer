using BSGames.Modules.GroundCheck;
using System.Collections;
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

    // Fracture a copy of an object
    public static void SpawnFragments(this MonoBehaviour mono, Component component, float scale = 1f)
    {
        var temp = new GameObject("Fragments");

        var result = Object.Instantiate(
            component, component.transform.position, component.transform.rotation);

        result.transform.localScale =
            component.transform.localScale * scale;
        result.transform.parent = temp.transform;
        result.tag = "Break";

        result.gameObject.GetComponent<Rigidbody>().mass /= 100f;
        result.GetComponent<Fracture>().CauseFracture();

        // Restore position of a ground checking script
        result.GetComponent<GroundCheck>().GroundChecker.
            SetTransform(component.transform);

        mono.StartCoroutine(DestroyFragments(temp));
    }

    private static IEnumerator DestroyFragments(GameObject @object) {
        yield return new WaitForSeconds(1f);

        Object.Destroy(@object);
    }
}