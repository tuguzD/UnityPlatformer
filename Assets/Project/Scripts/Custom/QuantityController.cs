using Gaskellgames.CameraController;
using Minimalist.Quantity;
using SmartPoint;
using System.Collections;
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

    private CheckPointController _checkPointController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _checkPointController = FindObjectOfType<CheckPointController>();
    }

    private void FixedUpdate()
    {
        velocity.Amount = _playerController.ball.velocity.z;
        pieces.Amount = pickUpParent.childCount;

        size.Amount = _playerController.ball.MeshSize() * _playerController.ball.Scale()
            + pickUpParent.GetComponentsInChildren<PickUpController>().Sum(Utils.MeshSize);
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

    // ReSharper disable once MemberCanBePrivate.Global
    public void GameOver()
    {
        _playerController.ball.GetComponent<CameraShaker>().Activate();
        // TODO: fracture not the player ball, but it's copy
        _playerController.ball.GetComponent<Fracture>().CauseFracture();

        fractured = true;
        _playerController.enabled = false;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn(float seconds = 3)
    {
        yield return new WaitForSeconds(seconds);

        _checkPointController.TeleportToRecentlyActivated(
            _playerController.ball.gameObject);
        fractured = false;
        _playerController.enabled = true;

        Reset();
    }

    private void Reset()
    {
        // Restore temperature, spikiness, and durability
        spikiness.FillAmount = 0f;
        temperature.FillAmount = 0.5f;
        durability.FillAmount = 1f;

        // Restore player ball size and destroy all objects picked up
        _playerController.ball.transform.localScale = Vector3.one;
        foreach (Transform child in pickUpParent) Destroy(child.gameObject);

        // _state.nest.SwitchToMacro(stateMachine);
    }
}