using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    private static readonly int EmissionID =
        Shader.PropertyToID("_EmissionColor");
    private static readonly int NormalMapScale =
        Shader.PropertyToID("_BumpScale");

    private QuantityController _quantities;

    [SerializeField] private MeshRenderer ballSpike;

    [Header("Ice Coverage")] private readonly Range
        _iceCoverage = new(min: 0f, max: 0.2f);
    [Header("Ice Roughness")] private readonly Range
        _iceRoughness = new(min: 0f, max: 4f);

    private Material _iceCoverSpikes;
    private Material _iceCoverBall;

    [Header("Hot Glowing")] private readonly Range
        _glowPower = new(min: 0f, max: 0.05f);

    private Material _hotGlowSpikes;
    private Material _hotGlowBall;
    private Color _emissionColor;

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();

        _iceCoverSpikes = ballSpike.materials[1];
        _iceCoverBall = ballSpike.transform.parent
            .GetComponent<MeshRenderer>().materials[1];

        _hotGlowSpikes = ballSpike.materials[2];
        _hotGlowBall = ballSpike.transform.parent
            .GetComponent<MeshRenderer>().materials[3];

        _emissionColor = _hotGlowBall.GetColor(EmissionID) / 5f;
    }

    private void FixedUpdate()
    {
        var hotColor = _emissionColor;
        var iceOpacity = 0f;
        var iceRoughness = 0f;

        var temperature = _quantities.temperature.Amount;
        if (temperature < 0)
        {
            hotColor *= 0f;
            iceOpacity = _iceCoverage.Lerp(-1 * temperature);
            iceRoughness = _iceRoughness.Lerp(-1 * temperature);
        }
        else hotColor *= _glowPower.Lerp(temperature);

        _iceCoverBall.SetOpacity(iceOpacity);
        _iceCoverBall.SetFloat(NormalMapScale, iceRoughness);
        _iceCoverSpikes.SetOpacity(iceOpacity);

        _hotGlowBall.SetColor(EmissionID, hotColor);
        _hotGlowSpikes.SetColor(EmissionID, hotColor);
    }
}