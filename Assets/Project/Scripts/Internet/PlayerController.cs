// Total changes: 5

using BSGames.Modules.GroundCheck;
using Ditzelgames;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public QuantityController quantities;

    /* Source of code below:
     * https://www.youtube.com/watch?v=FsYI9D3aukY&list=PLD8pFQ5A8vv6U4Sm0JKdcNGGOOZNoX2lv&index=2 */
    // Total changes: 2
    [Header("Physics")]
    public Rigidbody ball;
    public GroundCheck groundChecker;
    [SerializeField] private float speedup = 7.5f;

    private void FixedUpdate()
    {
        // Change #1: set minimum speed if it's too low
        if (ball.velocity.z < quantities.velocity.MinimumAmount) PhysicsHelper.ApplyForceToReachVelocity(
            velocity: Vector3.forward * quantities.velocity.MinimumAmount, rigidbody: ball, force: float.MaxValue);

        // Change #2: disable input movement if not grounded
        if (!groundChecker.IsGrounded()) return;
        var movement = _input.Game.Movement.ReadValue<Vector2>();

        ball.AddForce(new Vector3(movement.x, 0.0f, movement.y) * (speedup * quantities.size.Amount));
    }
    
    /* Source of code below:
     * https://gist.github.com/seferciogluecce/e57dd9e884bd38d2925f3de7826f5dd4 */
    // Total changes: 3
    private Vector2 _cachedPosition;

    // Change #1: use new input system instead of consuming mouse pointer position
    private void CachePosition(InputAction.CallbackContext context)
    {
        _cachedPosition = _input.Game.Position.ReadValue<Vector2>();
    }

    private void UseCachedPosition(InputAction.CallbackContext context)
    {
        var difference = _cachedPosition - _input.Game.Position.ReadValue<Vector2>();
        // Change #2: prevent shooting if the input is too small to prevent loosing pick-ups
        if (!Mathf.Approximately(Mathf.Abs(difference.magnitude), Mathf.Epsilon))
            Shoot(difference);
    }

    private void Shoot(Vector2 input)
    {
        var force = new Vector3(input.x, input.y, input.y);

        // Change #3: Try spawning pick-up with an opposite force (Newton's Third Law)
        if (quantities.ConsumePickUp(-force)) ball.AddForce(force);
    }

    /* Use new input system with examples borrowed from:
     * https://www.youtube.com/watch?v=kP7BawiCCZU */
    private GameInput _input;

    private void Awake()
    {
        _input = new GameInput();
        _input.Enable();
    }

    private void OnEnable()
    {
        _input.Game.Impulse.started += CachePosition;
        _input.Game.Impulse.canceled += UseCachedPosition;
    }

    private void OnDisable()
    {
        _input.Game.Impulse.started -= CachePosition;
        _input.Game.Impulse.canceled -= UseCachedPosition;
    }
}