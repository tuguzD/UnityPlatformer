using UnityEngine;

public readonly struct Range
{
    public static float MiddleInterpolation(float interpolation)
    {
        var middleInterpolation =
            interpolation < 0.5 ? 2 * interpolation : 2 * (1 - interpolation);
        return middleInterpolation;
    }

    private readonly float _minimum;
    private readonly float _maximum;

    public Range(float min = 0f, float max = 1f)
    {
        _minimum = min;
        _maximum = max;
    }

    public bool Includes(float point)
    {
        var result = point > _minimum && point < _maximum;
        return result;
    }

    public float Lerp(float interpolation)
    {
        var rangePoint = Mathf.Lerp(_minimum, _maximum, interpolation);
        return rangePoint;
    }

    public float InverseLerp(float point)
    {
        var interpolation = Mathf.InverseLerp(_minimum, _maximum, point);
        return interpolation;
    }
}