using UnityEngine;

public class PickUpsController : MonoBehaviour
{
    public Transform pickUpParent;
    public GameObject prefab;

    public void Add(PickUpObject pickUp)
    {
        _quantities.spikiness.Amount += _playerController.GetComponentInChildren
            <SpikinessController>().pieceToSpikinessMultiplier;

        Destroy(pickUp.body);
        pickUp.GetComponent<Collider>().enabled = false;
        pickUp.GetComponent<MagneticTool>().IsStatic = true;
    }

    public void Clear()
    {
        foreach (Transform child in pickUpParent)
            Destroy(child.gameObject);
    }

    public void Use(Vector3 forceOpposite)
    {
        Destroy(pickUpParent.GetChild(Random.
            Range(0, pickUpParent.childCount)).gameObject);

        var @object = Instantiate(
            prefab, _playerController.ball.position, _playerController.ball.rotation);
        @object.GetComponent<Rigidbody>().AddForce(forceOpposite);

        var pickUp = @object.GetComponent<PickUpObject>();
        pickUp.Switch(false);
        StartCoroutine(pickUp.Enable());
    }

    private QuantityController _quantities;
    private PlayerController _playerController;

    private void Start()
    {
        _quantities = GetComponent<QuantityController>();
        _playerController = GetComponent<PlayerController>();
    }
}