using UnityEngine;

public readonly struct MinMaxPair
{
    public readonly float Minimum;
    public readonly float Maximum;

    public MinMaxPair(float min = 0f, float max = 1f)
    {
        Minimum = min;
        Maximum = max;
    }

    public float Scaled(float value)
    {
        return Mathf.Lerp(Minimum, Maximum, value);
    }

    public float InverseLerp(float value)
    {
        return Mathf.InverseLerp(Minimum, Maximum, value);
    }
}