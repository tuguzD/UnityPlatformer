// Total changes: 4

using BSGames.Modules.GroundCheck;
using Ditzelgames;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public QuantityController quantities;

    [Header("Physics")]
    public Rigidbody ball;
    public float speedup = 7.5f;
    public GroundCheck groundChecker;

    [Header("Inputs")]
    [SerializeField] private InputActionReference movementInput;
    [SerializeField] private InputActionReference impulseInput;
    [SerializeField] private InputActionReference positionInput;

    private Vector2 _cachedPosition;
    public GameObject projectile;

    /* Source of methods:
     * https://gist.github.com/seferciogluecce/e57dd9e884bd38d2925f3de7826f5dd4 */
    // Total changes: 2
    private void Start()
    {
        /* Change #1: use new input system, using examples borrowed from:
         * https://www.youtube.com/watch?v=-c_LYQgG8BY
         * https://www.youtube.com/watch?v=kP7BawiCCZU */
        impulseInput.action.started += _ => _cachedPosition = positionInput.action.ReadValue<Vector2>();
        impulseInput.action.canceled += _ => Shoot(
            _cachedPosition - positionInput.action.ReadValue<Vector2>());
    }

    private void Shoot(Vector2 input)
    {
        var force = new Vector3(input.x, input.y, input.y);
        ball.AddForce(force);

        // Change #2: Spawn projectile with opposite force (due to Newton's Third Law)
        var smth = Instantiate(projectile, ball.position, ball.rotation);
        smth.AddComponent<Rigidbody>();
        smth.GetComponent<Rigidbody>().AddForce(-force);
    }

    /* Source of method:
     * https://www.youtube.com/watch?v=FsYI9D3aukY&list=PLD8pFQ5A8vv6U4Sm0JKdcNGGOOZNoX2lv&index=2 */
    // Total changes: 2
    private void FixedUpdate()
    {
        // Change #1: set minimum speed if it's too low
        if (ball.velocity.z < quantities.velocity.MinimumAmount) PhysicsHelper.ApplyForceToReachVelocity(
            velocity: Vector3.forward * quantities.velocity.MinimumAmount, rigidbody: ball, force: float.MaxValue);

        // Change #2: disable input movement if not grounded
        if (!groundChecker.IsGrounded()) return;
        var movement = movementInput.action.ReadValue<Vector2>();

        ball.AddForce(new Vector3(movement.x, 0.0f, movement.y) * (speedup * quantities.size.Amount));
    }
}