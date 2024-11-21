using UnityEngine;

public class DurabilityController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning(collision.gameObject.name);
    }
}