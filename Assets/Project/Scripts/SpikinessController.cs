using UnityEngine;

public class SpikinessController : MonoBehaviour
{
    public float pieceToSpikinessMultiplier = 0.1f;

    private QuantityController _quantities;

    private readonly MinMaxPair
        _spikeScale = new(min: 295, max: 385);

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();
    }

    private void FixedUpdate()
    {
        var spikiness = _quantities.spikiness;

        spikiness.MinimumAmount = _quantities.pieces.Amount * pieceToSpikinessMultiplier;
        spikiness.MaximumAmount = 1 + spikiness.MinimumAmount;

        this.UniformScale(_spikeScale.Scaled(spikiness.FillAmount));
    }
}