using Gaskellgames.CameraController; // TODO: move to "health" script
using UnityEngine;
using UnityEngine.InputSystem;

/* Source: https://www.youtube.com/watch?v=FsYI9D3aukY&list=PLD8pFQ5A8vv6U4Sm0JKdcNGGOOZNoX2lv&index=2
 * Changes: 2
 */
public class PlayerController : MonoBehaviour
{
    public float acceleration = 10.0f;
    public float minimalSpeed = 5.0f; // Change: add speed minimum
    private Rigidbody _rigidbody;

    private float _movementX;
    private float _movementY;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        GetComponent<CameraShaker>().Activate(); // TODO: move to "health" script
    }

    private void OnMove(InputValue movementValue)
    {
        var movementVector = movementValue.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        var movement = new Vector3(_movementX, 0.0f,
            _rigidbody.velocity.z < minimalSpeed ? minimalSpeed : _movementY); // Change: set speed to minimum
        _rigidbody.AddForce(movement * acceleration);
    }
}