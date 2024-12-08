// Total changes: 4

using Ditzelgames;
using System.Collections;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody body;

    /* Source of rotation logic:
     * https://github.com/deviantdear/RollABall/blob/master/Assets/Scripts/Rotator.cs */
    public float anglePerSecond = 30.0f;

    private void Update()
    {
        // Change #1: add physics-based rotation, not transform-based
        if (body) body.angularVelocity = PhysicsHelper.QuaternionToAngularVelocity(
            Quaternion.AngleAxis(anglePerSecond, Vector3.up));
    }

    /* Adapted from source of method:
     * https://www.youtube.com/watch?v=StATWcqq4po&t=950s */
    private void OnCollisionEnter(Collision ball)
    {
        // Change #2: restructure code to avoid unnecessary processing
        if (!ball.gameObject.CompareTag("Player")) return;
        var pickUps = ball.gameObject.GetComponentInParent<PickUpsController>();

        // Change #3: ignore collision with players when pick-up is switched off
        if (!GetComponent<MagneticTool>().TurnOnMagnetism)
        {
            Physics.IgnoreCollision(ball.collider, GetComponent<Collider>());
            StartCoroutine(StopIgnoreCollision(ball.collider));
            return;
        }

        transform.parent = pickUps.pickUpParent;
        // Change #4: move processing code to other class instead of increasing size
        pickUps.Add(this);
    }

    public void Switch(bool enable)
    {
        var magnetism = GetComponent<MagneticTool>();
        magnetism.TurnOnMagnetism = enable;
        magnetism.AffectByMagnetism = enable;
    }

    private IEnumerator StopIgnoreCollision(Collider first, float seconds = 0.1f)
    {
        yield return new WaitForSeconds(seconds);
        Physics.IgnoreCollision(first, GetComponent<Collider>(), false);
    }

    public IEnumerator Enable(float seconds = 0.1f)
    {
        yield return new WaitForSeconds(seconds);
        Switch(true);
    }
}