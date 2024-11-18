using System.Linq;
using Gaskellgames.CameraController;
using Minimalist.Quantity;
using UnityEngine;
using UnityEngine.Serialization;

public class QuantityController : MonoBehaviour
{
    [Header("Main Quantities")] // Example from PlayerBhv.cs
    public QuantityBhv temperature;
    public QuantityBhv pieces;
    
    [Header("Subjective Quantities")] // Example from PlayerBhv.cs
    public QuantityBhv durability;
    public QuantityBhv velocity;
    public QuantityBhv size;

    [Header("Objective Quantities")] // Example from PlayerBhv.cs
    public QuantityBhv spikiness;
    public float pieceToSpikinessMultiplier = 0.1f;
    public QuantityBhv plasticity;
    public QuantityBhv magnetisation;

    [Header("Pick-ups")]
    public Transform pickUpParent;
    public Transform spikes;

    private PlayerController _playerController;
    private const float SpikeScaleMin = 295;
    private const float SpikeScaleMax = 385;
    private bool _fractured;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        pieces.Amount = pickUpParent.childCount;
        size.Amount = _playerController.ball.Magnitude() + pickUpParent
            .GetComponentsInChildren<PickUpController>().Sum(Utils.Magnitude);

        velocity.Amount = _playerController.ball.velocity.z;

        spikiness.MinimumAmount = pieces.Amount * pieceToSpikinessMultiplier;
        spikiness.MaximumAmount = 1 + spikiness.MinimumAmount;
        
        var value = SpikeScaleMin + spikiness.FillAmount * (SpikeScaleMax - SpikeScaleMin);
        spikes.localScale = new Vector3(value, value, value);
    }

    private void Update()
    {
        if (!_fractured && Mathf.Approximately(durability.FillAmount, Mathf.Epsilon))
            GameOver();
    }

    private void GameOver()
    {
        _playerController.ball.GetComponent<CameraShaker>().Activate();
        _playerController.ball.GetComponent<Fracture>().CauseFracture();
        _fractured = true;

        _playerController.enabled = false;
    }
}