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
    private CheckPointController _checkPointController;
    private GameObject _ball;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _checkPointController = FindObjectOfType<CheckPointController>();

        _ball = _playerController.ball.gameObject;
    }

    public void ProcessPickUp(PickUpController controller)
    {
        spikiness.Amount += _playerController.GetComponentInChildren
            <SpikinessController>().pieceToSpikinessMultiplier;

        Destroy(controller.pickUp);
        controller.gameObject.GetComponent<Collider>().enabled = false;
        controller.gameObject.GetComponent<MagneticTool>().IsStatic = true;
    }

    private void FixedUpdate()
    {
        velocity.Amount = _playerController.ball.velocity.z;
        pieces.Amount = pickUpParent.childCount;

        size.Amount = _playerController.ball.MeshSize() * _playerController.ball.Scale()
            + pickUpParent.GetComponentsInChildren<PickUpController>().Sum(Utils.MeshSize);

        if (Mathf.Approximately(durability.FillAmount, Mathf.Epsilon)
            || _playerController.ball.transform.position.y < -2f)
            GameOver();
    }

    private void GameOver()
    {
        if (!_ball.activeInHierarchy) return;

        Debug.Log("Game Over!");
        _ball.GetComponent<CameraShaker>().Activate();

        // Fracture not the player ball, but it's copy instead
        var temp = Instantiate(
            _ball, _ball.transform.position, _ball.transform.rotation);
        temp.transform.parent = null;
        temp.gameObject.GetComponent<Rigidbody>().mass /= 100f;
        temp.GetComponent<Fracture>().CauseFracture();

        // Destroy all objects picked up and disable the ball itself
        foreach (Transform child in pickUpParent)
            Destroy(child.gameObject);
        _ball.SetActive(false);

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn(float seconds = 3)
    {
        yield return new WaitForSeconds(seconds);

        // Enable player ball back and teleport it to checkpoint
        _ball.SetActive(true);
        _checkPointController.TeleportToRecentlyActivated(_ball);
        // Restore position of a ground checking script
        _playerController.groundChecker.GroundChecker
            .SetTransform(_ball.transform);

        // Restore initial ball spikiness
        spikiness.MinimumAmount = 0f;
        spikiness.MaximumAmount = 1f;
        spikiness.FillAmount = 0f;

        // Restore ball temperature and durability
        temperature.FillAmount = 0.5f;
        durability.FillAmount = 1f;

        // Restore player ball size and velocity
        _ball.transform.localScale = Vector3.one;
        _playerController.ball.velocity = Vector3.zero;
    }
}