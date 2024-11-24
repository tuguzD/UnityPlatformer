using UnityEngine;

public class DurabilityController : MonoBehaviour
{
    public float threshold = 10;

    private QuantityController _quantities;
    private Material _damageMaterial;

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();
        _damageMaterial = GetComponent<MeshRenderer>().materials[1];
    }

    private void FixedUpdate()
    {
        _damageMaterial.SetOpacity(
            1 - _quantities.durability.FillAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PickUp")
            || (collision.gameObject.CompareTag("Break") && !collision.gameObject.GetComponent<Fracture>())
        ) return;

        var relativeVelocity = Vector3.Scale(collision.relativeVelocity, collision.contacts[0].normal);
        // combine both height (y) and forward speed (z)
        var damageVelocity = Mathf.Abs(relativeVelocity.y) + Mathf.Abs(relativeVelocity.z);
        if (collision.gameObject.CompareTag("Break")) damageVelocity /= 2;

        if (damageVelocity < threshold) return;
        // apply "damage" to the player ball
        Debug.Log($"Hit {collision.gameObject.name} with power {damageVelocity}");
        _quantities.durability.Amount -= DamageApplied(damageVelocity);
    }

    private float DamageApplied(float velocity)
    {
        var mass = threshold * _quantities.size.Amount;
        // max plasticity equals min brittleness, and vice versa
        var brittleness = 1 - _quantities.plasticity.Amount;

        var result = velocity * brittleness / mass;
        return result;
    }
}