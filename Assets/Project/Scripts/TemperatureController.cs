using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    private static readonly int EmissionID =
        Shader.PropertyToID("_EmissionColor");

    private QuantityController _quantities;

    private readonly MinMaxPair
        _glowPower = new(min: 0f, max: 0.2f);

    [SerializeField] private MeshRenderer ballSpike;

    private Material _hotGlowSpikes;
    private Material _hotGlowBall;
    private Color _emissionColor;

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();

        _hotGlowSpikes = ballSpike.materials[1];
        _hotGlowBall = ballSpike.transform.parent
            .GetComponent<MeshRenderer>().materials[2];

        _emissionColor = _hotGlowBall.GetColor(EmissionID) / 5f;
    }

    private void FixedUpdate()
    {
        var temperature = _quantities.temperature.Amount;
        if (temperature > 0)
        {
            var color = _glowPower.Scaled(temperature) * _emissionColor;
            _hotGlowBall.SetColor(EmissionID, color);
            _hotGlowSpikes.SetColor(EmissionID, color);
        }
        else
        {
            _hotGlowBall.SetColor(EmissionID, 0f * _emissionColor);
            _hotGlowSpikes.SetColor(EmissionID, 0f * _emissionColor);
        }
    }
}