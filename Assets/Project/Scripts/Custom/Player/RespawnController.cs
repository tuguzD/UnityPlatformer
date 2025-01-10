using Gaskellgames.CameraController;
using SmartPoint;
using Mirror;
using System.Collections;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public float respawnTime = 3f;
    public float killHeight = -float.MaxValue;

    private void Update()
    {
        var height = _ball.transform.position.y;
        var fellUp = height > GetComponent<BallCameraRig>().
            heightSurfaceTop - killHeight;
        var fellDown = height < killHeight;

        var destroyed = Mathf.Approximately(
            _quantities.durability.FillAmount, Mathf.Epsilon);

        // Check for "game over" conditions
        if (fellUp || fellDown || destroyed) GameOver();
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
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);
        Respawn();
    }
    
    private void Respawn()
    {
        _ball.SetActive(true);

        // Teleport player ball back to checkpoint
        var checkpoint = _checkPoints.GetRecentlyActivatedCheckpoint();
        _checkPoints.TeleportToCheckpoint(checkpoint.GetIndex(), _ball);

        _quantities.Restore(checkpoint.GetPosition().y);
    }

    private PlayerController _playerController;
    private CheckPointController _checkPoints;
    private PickUpsController _pickUps;

    private QuantityController _quantities;
    private GameObject _ball;

    private void Start()
    {
        _checkPoints = FindObjectOfType<NetworkManager>().
            GetComponentInChildren<CheckPointController>();
        _playerController = GetComponent<PlayerController>();
        _pickUps = GetComponent<PickUpsController>();

        _quantities = GetComponent<QuantityController>();
        _ball = GetComponent<PlayerController>().ball.gameObject;

        _ball.SetActive(false);
        Respawn();
    }
}