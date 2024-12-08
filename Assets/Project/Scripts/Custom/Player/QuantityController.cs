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

    private void FixedUpdate()
    {
        velocity.Amount = _playerController.ball.velocity.z;
        pieces.Amount = _pickUps.pickUpParent.childCount;

        size.Amount = _playerController.ball.MeshSize() * _playerController.ball.Scale()
            + _pickUps.pickUpParent.GetComponentsInChildren<PickUpObject>().Sum(Utils.MeshSize);
    }

    public void Restore()
    {
        // Restore position of a ground checking script
        _playerController.groundChecker.GroundChecker
            .SetTransform(_playerController.ball.transform);
        
        // Restore initial ball spikiness
        spikiness.MinimumAmount = 0f;
        spikiness.MaximumAmount = 1f;
        spikiness.FillAmount = 0f;

        // Restore ball temperature and durability
        temperature.FillAmount = 0.5f;
        durability.FillAmount = 1f;

        // Restore player ball size and velocity
        _playerController.ball.velocity = Vector3.zero;
        _playerController.ball.transform.localScale = Vector3.one;
    }

    private PickUpsController _pickUps;
    private PlayerController _playerController;

    private void Start()
    {
        _pickUps = GetComponent<PickUpsController>();
        _playerController = GetComponent<PlayerController>();
    }
}