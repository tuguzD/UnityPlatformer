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
        
        var collisionForce = ball.impulse.magnitude / Time.fixedDeltaTime;
        // Debug.Log(collisionForce);

        if (collisionForce > minimumCollisionForce)
            _fracture.CauseFracture();
    }
}