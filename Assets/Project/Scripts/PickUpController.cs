// Total changes: 4

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
        var scale = transform.localScale.magnitude;
        var quantities = ball.transform.parent.gameObject.GetComponent<QuantityController>();

        if (scale > quantities.size.Amount) return;
        transform.parent = quantities.pickUpParent;
        quantities.size.Amount += scale;

        // Change #3: increase player ball pieces counter
        quantities.pieces.Amount++;

        // Change #4: disable collider, magnetism and physics processing
        GetComponent<Collider>().enabled = false;
        GetComponent<MagneticTool>().IsStatic = true;
        Destroy(pickUp);
    }
}