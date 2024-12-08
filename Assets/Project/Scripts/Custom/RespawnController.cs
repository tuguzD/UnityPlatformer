using Gaskellgames.CameraController;
using SmartPoint;
using System.Collections;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public float killHeight = -5f;
    
    private CheckPointController _checkPointController;
    private QuantityController _quantities;
    private GameObject _ball;

    private void Start()
    {
        _checkPointController =
            GetComponentInParent<CheckPointController>();

        _quantities = GetComponent<QuantityController>();
        _ball = GetComponent<PlayerController>().ball.gameObject;
    }

    private void Update()
    {
        var destroyed = Mathf.Approximately(_quantities.durability.FillAmount, Mathf.Epsilon);
        var fellOff = _ball.transform.position.y < killHeight;

        // Check for "game over" conditions
        if (destroyed || fellOff) GameOver();
    }

    // Fracture not the player ball, but it's copy instead
    private void BallFracture()
    {
        var temp = Instantiate(
            _ball, _ball.transform.position, _ball.transform.rotation);
        temp.transform.parent = null;
        temp.tag = "Break";

        temp.gameObject.GetComponent<Rigidbody>().mass /= 100f;
        temp.GetComponent<Fracture>().CauseFracture();
    }

    private void GameOver()
    {
        if (!_ball.activeInHierarchy) return;
        Debug.Log("Game Over!");

        _ball.GetComponent<CameraShaker>().Activate();
        BallFracture();

        // Destroy all objects picked up and disable the ball itself
        _quantities.RemovePickUps();
        _ball.SetActive(false);

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn(float seconds = 3)
    {
        yield return new WaitForSeconds(seconds);

        // Enable player ball back and teleport it to checkpoint
        _ball.SetActive(true);
        _checkPointController.TeleportToRecentlyActivated(_ball);

        _quantities.Restore();
    }
}