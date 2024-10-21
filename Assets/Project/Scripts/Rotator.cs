// Total changes: 2

using Ditzelgames;
using UnityEngine;

// Source: https://github.com/deviantdear/RollABall/blob/master/Assets/Scripts/Rotator.cs
public class Rotator : MonoBehaviour
{
    public float rotation = 30.0f;

    // Change #0: store rigidbody reference =)
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Change #1: add physics-based rotation, not transform-based
        if (_rigidbody)
            _rigidbody.angularVelocity = PhysicsHelper.QuaternionToAngularVelocity(
                Quaternion.AngleAxis(rotation, Vector3.up));
    }

    // Change #2: disable collider, magnetism and physics processing
    private void OnCollisionEnter(Collision ball)
    {
        if (ball.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<MagneticTool>().IsStatic = true;
            Destroy(_rigidbody);
        }
    }
}