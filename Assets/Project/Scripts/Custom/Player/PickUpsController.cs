using UnityEngine;

public class PickUpsController : MonoBehaviour
{
    public Transform pickUpParent;
    public GameObject projectile;

    private QuantityController _quantities;
    private PlayerController _playerController;

    private void Start()
    {
        _quantities = GetComponent<QuantityController>();
        _playerController = GetComponent<PlayerController>();
    }

    public void ProcessPickUp(PickUpObject pickUp)
    {
        _quantities.spikiness.Amount += _playerController.GetComponentInChildren
            <SpikinessController>().pieceToSpikinessMultiplier;

        Destroy(pickUp.body);
        pickUp.gameObject.GetComponent<Collider>().enabled = false;
        pickUp.gameObject.GetComponent<MagneticTool>().IsStatic = true;
    }

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
}