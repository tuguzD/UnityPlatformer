// Total changes: 10

using Ditzelgames;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    /* Source of code below:
     * https://www.youtube.com/watch?v=FsYI9D3aukY&list=PLD8pFQ5A8vv6U4Sm0JKdcNGGOOZNoX2lv&index=2 */
    // Total changes: 3
    public Rigidbody ball;
    
    [Header("Movement")]
    [SerializeField] private float speedup = 10f;

    private void FixedUpdate()
    {
        var range = _quantities.velocity;
        var speed = ball.velocity;

        // Change #1: set minimum speed if it's too low...
        if (speed.z < range.MinimumAmount) PhysicsHelper.ApplyForceToReachVelocity(
            velocity: Vector3.forward * range.MinimumAmount, rigidbody: ball, force: float.MaxValue);
        // ...and maximum speed: https://discussions.unity.com/t/limiting-rigidbody-speed/44191/2
        if (speed.z > range.MaximumAmount) ball.velocity = speed.normalized * range.MaximumAmount;

        // Change #2: disable input movement if not grounded
        if (!IsGrounded) return;
        var movement = _input.Game.Movement.ReadValue<Vector2>();
        AddForce(movement.x, movement.y);

        // Change #3: add alternative movement control by dragging from point on screen
        if (_tapPlayer || !_input.Game.Impulse.inProgress || !IsGrounded) return;
        movement = _input.Game.Position.ReadValue<Vector2>() - _cachedPosition;
        AddForce(Mathf.Sign(movement.x), Mathf.Sign(movement.y));
    }

    private void AddForce(float x, float y)
    {
        ball.AddForce(new Vector3(x, 0, y) * (speedup * _quantities.size.Amount));
    }

    private float Radius => ball.GetComponent<SphereCollider>().radius;
    private bool IsGrounded => Physics.OverlapSphere(
            ball.position, Radius * 1.5f,
            ~0, QueryTriggerInteraction.Ignore)
        .Any(other => !other.CompareTag("Player"));

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(ball.position, Radius * 1.5f);
    }
    
    /* Source of code below:
     * https://gist.github.com/seferciogluecce/e57dd9e884bd38d2925f3de7826f5dd4 */
    // Total changes: 6
    [Header("Impulse")]
    [SerializeField] private float fullImpulsePower = 15.0f;
    [SerializeField] private float fractureImpulsePower = 10.0f;
    private Vector2 _cachedPosition;

    private bool _tapPlayer;

    private void CachePosition(InputAction.CallbackContext context)
    {
        // Change #1: use new input system instead of consuming mouse pointer position
        _cachedPosition = _input.Game.Position.ReadValue<Vector2>();

        // Change #2: allow player to only hit its ball to initiate shooting
        var ray = GetComponent<PlayerInput>().camera.ScreenPointToRay(_cachedPosition);
        var rayCast = Physics.Raycast(ray, out var hit);

        _tapPlayer = rayCast && hit.collider.CompareTag("Player");
        Debug.DrawLine(ray.origin, hit.point, duration: 5f,
            color: _tapPlayer ? Color.green : Color.red);
    }

    private void UseCachedPosition(InputAction.CallbackContext context)
    {
        if (!_tapPlayer) return;
        var difference = _cachedPosition - _input.Game.Position.ReadValue<Vector2>();

        // Change #3: determine direction of impulse based on ball's position
        var direction = ball.transform.position.y < GetComponent<BallCameraRig>().heightSurfaceTop / 2f;
        difference.y *= direction ? 1 : -1;

        // Change #4: prevent shooting if the input is too small to prevent loosing pick-ups
        if (!Mathf.Approximately(Mathf.Abs(difference.magnitude), Mathf.Epsilon))
            Shoot(difference);
    }

    private void Shoot(Vector2 input)
    {
        // Change #5: make force uniform, or dependent only on direction of user input
        var force = new Vector3(input.x, input.y, input.y).
            normalized * (speedup * _quantities.velocity.MinimumAmount);
        var anyPickUps = _pickUps.pickUpParent.childCount > 0;

        // Change #6: spawn pick-up with an opposite force (Newton's Third Law)...
        if (anyPickUps) _pickUps.Use(-force * fullImpulsePower);
        // ...or consume ball's durability for a weaker impulse
        else
        {
            ball.CauseFracture(0.5f);
            ball.UniformScale(ball.transform.localScale.x - 0.05f);

            _quantities.durability.Amount -= _quantities.durability.MaximumAmount / 4f;
        }
        ball.AddForce(force * (anyPickUps ? fullImpulsePower : fractureImpulsePower));
    }

    // Change #7: cancel shooting if player ball isn't active
    private void Update()
    {
        if (!ball.gameObject.activeInHierarchy)
            _tapPlayer = false;
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

    private PickUpsController _pickUps;
    private QuantityController _quantities;

    private void Start()
    {
        _pickUps = GetComponent<PickUpsController>();
        _quantities = GetComponent<QuantityController>();
    }
}