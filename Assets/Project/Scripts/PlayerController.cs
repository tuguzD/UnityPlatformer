// Total changes: 3

using BSGames.Modules.GroundCheck;
using Ditzelgames;
using UnityEngine;
using UnityEngine.InputSystem;

/* Source of class:
 * https://www.youtube.com/watch?v=FsYI9D3aukY&list=PLD8pFQ5A8vv6U4Sm0JKdcNGGOOZNoX2lv&index=2 */
public class PlayerController : MonoBehaviour
{
    public QuantityController quantities;

    [Header("Physics")] // Example from "Ground Checking Kit" asset docs
    public Rigidbody ball;
    public float speedup = 10.0f;
    public GroundCheck groundChecker;

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
        // Change #1: update velocity quantity
        quantities.velocity.Amount = ball.velocity.z;
        
        // Change #2: set minimum speed if it's too low
        if (ball.velocity.z < quantities.velocity.MinimumAmount) PhysicsHelper.ApplyForceToReachVelocity(
            velocity: Vector3.forward * quantities.velocity.MinimumAmount, rigidbody: ball, force: float.MaxValue);

        // Change #3: disable input movement if not grounded
        var movement = !groundChecker.IsGrounded() ? new Vector3()
            : new Vector3(_inputRoll, 0.0f, _inputSpeed);
        ball.AddForce(movement * (speedup + quantities.size.Amount));
    }
}