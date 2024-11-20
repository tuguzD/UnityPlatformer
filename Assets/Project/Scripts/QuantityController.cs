using Gaskellgames.CameraController;
using Minimalist.Quantity;
using System.Linq;
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
    [HideInInspector] public bool fractured;

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
        if (!fractured && Mathf.Approximately(durability.FillAmount, Mathf.Epsilon))
            GameOver();
    }

    public void ProcessPickUp(PickUpController controller)
    {
        spikiness.Amount += _playerController.GetComponentInChildren
            <SpikinessController>().pieceToSpikinessMultiplier;

        Destroy(controller.pickUp);
        controller.gameObject.GetComponent<Collider>().enabled = false;
        controller.gameObject.GetComponent<MagneticTool>().IsStatic = true;
    }

    public void GameOver()
    {
        _playerController.ball.GetComponent<CameraShaker>().Activate();
        _playerController.ball.GetComponent<Fracture>().CauseFracture();
        fractured = true;

        _playerController.enabled = false;
    }
}