using UnityEngine;

public class DurabilityController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PickUp")) return;
        foreach (var contact in collision.contacts)
        {
            var result = Vector3.Scale(collision.relativeVelocity, contact.normal);
            // combine both height (y) and forward speed (z)
            var power = Mathf.Abs(result.y) + Mathf.Abs(result.z);
            if (collision.gameObject.CompareTag("Break")) power /= 3;

            // apply "damage" to the player ball
            if (power > 5)
            {
                Debug.Log($"Hit {collision.gameObject.name} with power {power}");
            }
        }
    }
}