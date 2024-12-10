using UnityEngine;

public class SpikinessController : MonoBehaviour
{
    public float onePieceMultiplier = 0.1f;

    private readonly Range
        _spikeScale = new(min: 295, max: 385);

    private void FixedUpdate()
    {
        var spikiness = _quantities.spikiness;

        spikiness.MinimumAmount = _quantities.pieces.Amount * onePieceMultiplier;
        spikiness.MaximumAmount = 1 + spikiness.MinimumAmount;

        this.UniformScale(_spikeScale.Lerp(spikiness.FillAmount));
    }

    public void ReduceQuantity()
    {
        var spikiness = _quantities.spikiness;
        spikiness.MinimumAmount -= onePieceMultiplier;
        spikiness.Amount -= onePieceMultiplier;
    }

    private QuantityController _quantities;

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();
    }
}