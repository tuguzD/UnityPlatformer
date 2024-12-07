public readonly struct MinMaxPair
{
    private readonly float _minimum;
    private readonly float _maximum;

    public MinMaxPair(float min = 0f, float max = 1f)
    {
        _minimum = min;
        _maximum = max;
    }

    public float Scaled(float value)
    {
        return _minimum + value * (_maximum - _minimum);
    }
}