using UnityEngine;

[RequireComponent(typeof(Fracture))]
public class CauseFracture : MonoBehaviour
{
    [Tooltip("Minimum contact collision force required to cause the object to fracture.")]
    public float minimumCollisionForce = 500f;

    private Fracture _fracture;

    private void Start()
    {
        _fracture = GetComponent<Fracture>();
    }

    private void OnCollisionEnter(Collision ball)
    {
        if (!ball.gameObject.CompareTag("Player")) return;
        if (CollisionForce(ball) > minimumCollisionForce)
            _fracture.CauseFracture();
    }

    private static float CollisionForce(Collision ball)
    {
        var quantities = ball.gameObject.transform.parent.GetComponent<QuantityController>();

        var collisionForce = ball.impulse.magnitude / Time.fixedDeltaTime;
        var spikinessMultiplier = 1 + quantities.spikiness.Amount;

        var x = quantities.plasticity.Amount;
        // how well energy is transferred to the object - the closer it is to average, the better
        var plasticityMultiplier = 0.5f + (x < 0.5 ? 2*x : 2*(1 - x));

        var result = collisionForce * spikinessMultiplier * plasticityMultiplier;
        Debug.Log($"{collisionForce} * {spikinessMultiplier} * {plasticityMultiplier} = {result}");
        return result;
    }
}