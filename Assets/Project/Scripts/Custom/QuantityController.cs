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

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _checkPointController = FindObjectOfType<CheckPointController>();
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

        // TODO: kill player if it falls below the ground
        if (Mathf.Approximately(durability.FillAmount, Mathf.Epsilon))
            // || _playerController.ball.transform.position.y < 0f)
            GameOver();
    }

    private void GameOver()
    {
        var ball = _playerController.ball.gameObject;
        if (!ball.activeInHierarchy) return;

        Debug.Log("Game Over!");
        ball.GetComponent<CameraShaker>().Activate();

        // Fracture not the player ball, but it's copy instead
        var temp = Instantiate(ball, ball.transform.position, ball.transform.rotation);
        temp.transform.parent = null;
        temp.gameObject.GetComponent<Rigidbody>().mass /= 100f;
        temp.GetComponent<Fracture>().CauseFracture();

        // Destroy all objects picked up and disable the ball itself
        foreach (Transform child in pickUpParent)
            Destroy(child.gameObject);
        ball.SetActive(false);

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn(float seconds = 3)
    {
        yield return new WaitForSeconds(seconds);

        // Enable player ball back and teleport it to checkpoint
        _playerController.ball.gameObject.SetActive(true);
        _checkPointController.TeleportToRecentlyActivated(
            _playerController.ball.gameObject);

        // Restore temperature, spikiness and durability
        spikiness.FillAmount = 0f;
        temperature.FillAmount = 0.5f;
        durability.FillAmount = 1f;

        // Restore player ball size
        _playerController.ball.transform.localScale = Vector3.one;
    }
}