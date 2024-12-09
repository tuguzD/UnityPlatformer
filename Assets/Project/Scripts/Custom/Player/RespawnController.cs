using Gaskellgames.CameraController;
using SmartPoint;
using System.Collections;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public float respawnTime = 3f;
    public float killHeight = -5f;

    private void Update()
    {
        var destroyed = Mathf.Approximately(
            _quantities.durability.FillAmount, Mathf.Epsilon);
        var fellOff = _ball.transform.position.y < killHeight;

        // Check for "game over" conditions
        if (destroyed || fellOff) GameOver();
    }

    private void GameOver()
    {
        if (!_ball.activeInHierarchy) return;
        Debug.Log("Game Over!");

        _ball.GetComponent<CameraShaker>().Activate();
        _playerController.ball.CauseFracture();

        // Destroy all objects picked up and disable the ball itself
        _pickUps.Clear();
        _ball.SetActive(false);

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        // Enable player ball back and teleport it to checkpoint
        _ball.SetActive(true);
        _checkPoints.TeleportToRecentlyActivated(_ball);

        _quantities.Restore();
    }

    private PlayerController _playerController;
    private CheckPointController _checkPoints;
    private PickUpsController _pickUps;

    private QuantityController _quantities;
    private GameObject _ball;

    private void Start()
    {
        _checkPoints = GetComponentInParent<CheckPointController>();
        _playerController = GetComponent<PlayerController>();
        _pickUps = GetComponent<PickUpsController>();

        _quantities = GetComponent<QuantityController>();
        _ball = GetComponent<PlayerController>().ball.gameObject;
    }
}