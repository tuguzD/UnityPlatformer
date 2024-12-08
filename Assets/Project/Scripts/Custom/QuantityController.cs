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

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        velocity.Amount = _playerController.ball.velocity.z;
        pieces.Amount = pickUpParent.childCount;

        size.Amount = _playerController.ball.MeshSize() * _playerController.ball.Scale()
            + pickUpParent.GetComponentsInChildren<PickUpController>().Sum(Utils.MeshSize);
    }

    public void ProcessPickUp(PickUpController controller)
    {
        spikiness.Amount += _playerController.GetComponentInChildren
            <SpikinessController>().pieceToSpikinessMultiplier;

        Destroy(controller.pickUp);
        controller.gameObject.GetComponent<Collider>().enabled = false;
        controller.gameObject.GetComponent<MagneticTool>().IsStatic = true;
    }

    public GameObject projectile;

    public bool ConsumePickUp(Vector3 forceOpposite)
    {
        var smth = Instantiate(
            projectile, _playerController.ball.position, _playerController.ball.rotation);
        smth.AddComponent<Rigidbody>();
        smth.GetComponent<Rigidbody>().AddForce(forceOpposite);

        return true;
    }

    public void RemovePickUps()
    {
        foreach (Transform child in pickUpParent)
            Destroy(child.gameObject);
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
}