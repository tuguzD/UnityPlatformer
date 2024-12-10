using System.Collections;
using System.Linq;
using UnityEngine;

public class PickUpsController : MonoBehaviour
{
    public Transform pickUpParent;
    public GameObject prefab;

    public void Add(PickUpObject pickUp)
    {
        _quantities.spikiness.Amount += _spikes.onePieceMultiplier;

        Destroy(pickUp.body);
        pickUp.GetComponent<Collider>().enabled = false;
        pickUp.GetComponent<MagneticTool>().IsStatic = true;

        ConsumeVeryHot(pickUp);
    }

    private void FixedUpdate()
    {
        foreach (Transform child in pickUpParent) ConsumeVeryHot(child);
    }

    private void ConsumeVeryHot(Component component)
    {
        if (_quantities.temperature.Amount >= 0.75f)
            StartCoroutine(RemoveCoroutine(component));
    }

    private IEnumerator RemoveCoroutine(Component component)
    {
        yield return new WaitForSeconds(1);
        if (!component) yield break;

        var ball = _playerController.ball;
        ball.UniformScale(ball.transform.localScale.x + component.MeshSize());

        Destroy(component.gameObject);
        _spikes.ReduceQuantity();
    }

    public void Use(Vector3 force)
    {
        Destroy(pickUpParent.GetChild(Random.
            Range(0, pickUpParent.childCount)).gameObject);
        _spikes.ReduceQuantity();

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

    private SpikinessController _spikes;
    private QuantityController _quantities;
    private PlayerController _playerController;

    private void Start()
    {
        _quantities = GetComponent<QuantityController>();
        _playerController = GetComponent<PlayerController>();
        _spikes = _playerController.GetComponentInChildren<SpikinessController>();
    }
}