using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    private static readonly int EmissionID =
        Shader.PropertyToID("_EmissionColor");

    private QuantityController _quantities;

    private readonly MinMaxPair
        _glowPower = new(min: 0f, max: 0.2f);

    private Material _hotGlow;
    private Color _emissionColor;

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();

        _hotGlow = GetComponent<MeshRenderer>().materials[2];
        _emissionColor = _hotGlow.GetColor(EmissionID) / 5f;
    }

    private void FixedUpdate()
    {
        var temperature = _quantities.temperature.Amount;
        if (temperature > 0)
        {
            var color = _glowPower.Scaled(temperature) * _emissionColor;
            _hotGlow.SetColor(EmissionID, color);
        }
        else
        {
            _hotGlow.SetColor(EmissionID, 0f * _emissionColor);
        }
    }
}