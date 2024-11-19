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

    private void OnCollisionEnter(Collision ball)
    {
        if (!ball.gameObject.CompareTag("Player")) return;
        if (CollisionForce(ball) > durability)
            _fracture.CauseFracture();
    }

    private static float CollisionForce(Collision ball)
    {
        var quantities = ball.gameObject.transform.parent.GetComponent<QuantityController>();

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