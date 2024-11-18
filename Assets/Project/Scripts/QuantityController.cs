using System.Linq;
using Gaskellgames.CameraController;
using Minimalist.Quantity;
using UnityEngine;

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
    public QuantityBhv plasticity;
    public QuantityBhv magnetisation;

    [Header("Pick-ups")]
    public Transform pickUpParent;

    private PlayerController _playerController;
    private bool _fractured;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        velocity.Amount = _playerController.ball.velocity.z;

        pieces.Amount = pickUpParent.childCount;
        size.Amount = _playerController.ball.Magnitude() + pickUpParent
            .GetComponentsInChildren<PickUpController>().Sum(Utils.Magnitude);
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