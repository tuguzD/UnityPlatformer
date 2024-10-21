// Total changes: 5

using BSGames.Modules.GroundCheck;
using Ditzelgames;
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
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheck = GetComponent<GroundCheck>();

        // Change #2: set size to initial scale
        _size = _rigidbody.transform.localScale.magnitude;

        GetComponent<CameraShaker>().Activate(); // TODO: move to "health" script
    }

    private float _inputRoll;
    private float _inputSpeed;

    private void OnMove(InputValue value)
    {
        var movementVector = value.Get<Vector2>();
        _inputRoll = movementVector.x;
        _inputSpeed = movementVector.y;
    }

    private void FixedUpdate()
    {
        // Change #3: set minimum speed if it's too low
        if (_rigidbody.velocity.z < minimalSpeed)
            PhysicsHelper.ApplyForceToReachVelocity(
                _rigidbody, Vector3.forward * minimalSpeed, float.MaxValue);

        // Change #4: disable input movement if not grounded
        var movement = _groundCheck.IsGrounded()
            ? new Vector3(_inputRoll, 0.0f, _inputSpeed)
            : new Vector3();
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

            // Change #5: disable collider
            pickUp.gameObject.GetComponent<Collider>().enabled = false;

            /* Source:
             * https://github.com/deviantdear/RollABall/blob/master/Assets/Scripts/PlayerController.cs#L74 */
            pickUp.gameObject.GetComponent<Rotator>().enabled = false;
        }
    }
}