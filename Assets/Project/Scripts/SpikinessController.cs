using UnityEngine;

public class SpikinessController : MonoBehaviour
{
    public float pieceToSpikinessMultiplier = 0.1f;

    private QuantityController _quantities;

    private const float SpikeScaleMin = 295;
    private const float SpikeScaleMax = 385;

    private void Start()
    {
        _quantities = transform.parent.parent.GetComponent<QuantityController>();
    }

    private void FixedUpdate()
    {
        var spikiness = _quantities.spikiness;

        spikiness.MinimumAmount = _quantities.pieces.Amount * pieceToSpikinessMultiplier;
        spikiness.MaximumAmount = 1 + spikiness.MinimumAmount;

        var value = SpikeScaleMin + spikiness.FillAmount * (SpikeScaleMax - SpikeScaleMin);
        transform.localScale = new Vector3(value, value, value);
    }
}