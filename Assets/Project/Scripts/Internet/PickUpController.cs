// Total changes: 3

using Ditzelgames;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody pickUp;

    /* Source of rotation logic:
     * https://github.com/deviantdear/RollABall/blob/master/Assets/Scripts/Rotator.cs */
    public float anglePerSecond = 30.0f;

    private void Update()
    {
        // Change #1: add physics-based rotation, not transform-based
        if (pickUp) pickUp.angularVelocity = PhysicsHelper
            .QuaternionToAngularVelocity(Quaternion.AngleAxis(anglePerSecond, Vector3.up));
    }

    /* Adapted from source of method:
     * https://www.youtube.com/watch?v=StATWcqq4po&t=950s */
    private void OnCollisionEnter(Collision ball)
    {
        // Change #2: restructure code to avoid unnecessary processing
        if (!ball.gameObject.CompareTag("Player")) return;
        var quantities = ball.gameObject.GetComponentInParent<QuantityController>();

        if (this.Magnitude() > quantities.size.Amount) return;
        transform.parent = quantities.pickUpParent;
        // Change #3: move processing code to other class instead of increasing size
        quantities.ProcessPickUp(this);
    }
}