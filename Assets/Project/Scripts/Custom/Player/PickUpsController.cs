using System.Linq;
using UnityEngine;

public class PickUpsController : MonoBehaviour
{
    public Transform pickUpParent;
    public GameObject prefab;

    private void FixedUpdate()
    {
        if (_quantities.temperature.Amount < 0.75f) return;

        var ball = _playerController.ball;
        ball.UniformScale(ball.transform.localScale.x + ChildrenMeshSize());

        // TODO: fix on consumption
        _quantities.spikiness.Amount -=
            pickUpParent.childCount * _spikinessController.pieceToSpikinessMultiplier;

        Clear();
        // foreach (Transform child in pickUpParent) StartCoroutine(Remove(child));
    }

    public void Add(PickUpObject pickUp)
    {
        _quantities.spikiness.Amount += _spikinessController.pieceToSpikinessMultiplier;

        Destroy(pickUp.body);
        pickUp.GetComponent<Collider>().enabled = false;
        pickUp.GetComponent<MagneticTool>().IsStatic = true;

        // if (_quantities.temperature.Amount >= 0.75f) StartCoroutine(Remove(pickUp));
    }

    // private IEnumerator Remove(Component component)
    // {
    //     yield return new WaitForSeconds(1);
    //     
    //     var ball = _playerController.ball;
    //     ball.UniformScale(ball.transform.localScale.x + component.MeshSize());
    //     Destroy(component.gameObject);
    // }

    public void Use(Vector3 force)
    {
        Destroy(pickUpParent.GetChild(Random.
            Range(0, pickUpParent.childCount)).gameObject);

        // TODO: fix on first pick-up
        _quantities.spikiness.Amount -= _spikinessController.pieceToSpikinessMultiplier;

        var @object = Instantiate(
            prefab, _playerController.ball.position, _playerController.ball.rotation);
        @object.GetComponent<Rigidbody>().AddForce(force);

        var pickUp = @object.GetComponent<PickUpObject>();
        pickUp.Switch(false);
        StartCoroutine(pickUp.Enable());
    }

    public void Clear()
    {
        foreach (Transform child in pickUpParent)
            Destroy(child.gameObject);
    }

    public float ChildrenMeshSize()
    {
        return pickUpParent.GetComponentsInChildren<PickUpObject>().Sum(Utils.MeshSize);
    }

    private QuantityController _quantities;
    private PlayerController _playerController;
    private SpikinessController _spikinessController;

    private void Start()
    {
        _quantities = GetComponent<QuantityController>();
        _playerController = GetComponent<PlayerController>();
        _spikinessController = _playerController.GetComponentInChildren<SpikinessController>();
    }
}