// Total changes: 4

using BSGames.Modules.GroundCheck;
using Gaskellgames.CameraController; // TODO: move to "health" script
using UnityEngine;
using UnityEngine.InputSystem;

/* Source of class:
 * https://www.youtube.com/watch?v=FsYI9D3aukY&list=PLD8pFQ5A8vv6U4Sm0JKdcNGGOOZNoX2lv&index=2 */
public class PlayerController : MonoBehaviour
{
    // Change #1: add speed minimum
    public float minimalSpeed = 5.0f;
    public float acceleration = 10.0f;

    // Documentation of "Ground Checking Kit" asset
    private GroundCheck _groundCheck;

    private Rigidbody _rigidbody;
    private float _size;

    private void Start()
    {
        _groundCheck = GetComponent<GroundCheck>();
        _rigidbody = GetComponent<Rigidbody>();

        // Change #2: set size to initial scale
        _size = _rigidbody.transform.localScale.magnitude;

        GetComponent<CameraShaker>().Activate(); // TODO: move to "health" script
    }

    private float _movementX;
    private float _movementY;

    private void OnMove(InputValue value)
    {
        var movementVector = value.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        var movement = new Vector3(
            // Documentation of "Ground Checking Kit" asset
            _groundCheck.IsGrounded() ? _movementX : 0.0f, 0.0f,
            // Change #3: set speed to minimum
            _rigidbody.velocity.z < minimalSpeed ? minimalSpeed : _movementY);
        _rigidbody.AddForce(movement * (acceleration + _size));
    }

    /* Source of method:
     * https://www.youtube.com/watch?v=StATWcqq4po&t=950s */
    private void OnCollisionEnter(Collision pickUp)
    {
        var size = pickUp.transform.localScale.magnitude;
        if (pickUp.gameObject.CompareTag("PickUp") && size <= _size)
        {
            pickUp.transform.parent = transform;
            _size += size;

            // Change #4: disable collider
            pickUp.gameObject.GetComponent<Collider>().enabled = false;

            /* Source:
             * https://github.com/deviantdear/RollABall/blob/master/Assets/Scripts/PlayerController.cs#L74 */
            pickUp.gameObject.GetComponent<Rotator>().enabled = false;
        }
    }
}