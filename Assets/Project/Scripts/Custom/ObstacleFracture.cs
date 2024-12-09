using UnityEngine;

[RequireComponent(typeof(Fracture))]
public class ObstacleFracture : MonoBehaviour
{
    public float durability = 30f;

    private Fracture _fracture;

    private void Start()
    {
        _fracture = GetComponent<Fracture>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var @object = other.gameObject;

        var player = @object.CompareTag("Player") && CollisionForce(@object) > durability;
        var pickUp = @object.CompareTag("PickUp") && other.impulse.magnitude > durability / 5f;
        if (player || pickUp) _fracture.CauseFracture();
    }

    private static float CollisionForce(GameObject ball)
    {
        var quantities = ball.GetComponentInParent<QuantityController>();
        if (!quantities) return 0f;

        var mass = quantities.size.Amount;
        var velocity = quantities.velocity.Amount;
        var spikiness = 1 + quantities.spikiness.Amount;

        var x = quantities.plasticity.Amount;
        // how well energy is transferred to the object - the closer it is to average, the better
        var plasticity = 0.5f + (x < 0.5 ? 2*x : 2*(1 - x));

        var result = mass * velocity * spikiness * plasticity;
        Debug.Log($"{mass} * {velocity} * {spikiness} * {plasticity} = {result}");
        return result;
    }
}