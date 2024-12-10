using UnityEngine;

public class SpikinessController : MonoBehaviour
{
    public float onePieceMultiplier = 0.1f;

    private QuantityController _quantities;

    private readonly Range
        _spikeScale = new(min: 295, max: 385);

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();
    }

    private void FixedUpdate()
    {
        var spikiness = _quantities.spikiness;

        spikiness.MinimumAmount = _quantities.pieces.Amount * onePieceMultiplier;
        spikiness.MaximumAmount = 1 + spikiness.MinimumAmount;

        this.UniformScale(_spikeScale.Lerp(spikiness.FillAmount));
    }
}