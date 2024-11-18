using UnityEngine;

public static class Utils
{
    public static float Magnitude(this Component component)
    {
        return component.transform.lossyScale.magnitude;
    }
}