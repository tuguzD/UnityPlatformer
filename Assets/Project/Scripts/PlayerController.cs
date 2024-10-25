// Total changes: 4

using BSGames.Modules.GroundCheck;
using Ditzelgames;
using Gaskellgames.CameraController; // TODO: move to "health" script
using Minimalist.Quantity;
using UnityEngine;
using UnityEngine.InputSystem;

/* Source of class:
 * https://www.youtube.com/watch?v=FsYI9D3aukY&list=PLD8pFQ5A8vv6U4Sm0JKdcNGGOOZNoX2lv&index=2 */
public class PlayerController : MonoBehaviour
{
    public float speedup = 10.0f;

    [Header("Quantities")] // Example from PlayerBhv.cs
    public QuantityBhv temperature;
    public QuantityBhv velocity;
    public QuantityBhv size;

    [Header("Physics")] // Example from "Ground Checking Kit" asset docs
    public Rigidbody ball;
    public GroundCheck groundChecker;

    private void Start()
    {
        // Change #1: set size to initial scale
        size.Amount = ball.transform.localScale.magnitude;

        ball.GetComponent<CameraShaker>().Activate(); // TODO: move to "health" script
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
        // Change #2: update velocity quantity
        velocity.Amount = ball.velocity.z;
        
        // Change #3: set minimum speed if it's too low
        if (ball.velocity.z < velocity.MinimumAmount) PhysicsHelper.ApplyForceToReachVelocity(
            velocity: Vector3.forward * velocity.MinimumAmount, rigidbody: ball, force: float.MaxValue);

        // Change #4: disable input movement if not grounded
        var movement = !groundChecker.IsGrounded() ? new Vector3()
            : new Vector3(_inputRoll, 0.0f, _inputSpeed);
        ball.AddForce(movement * (speedup + size.Amount));
    }
}