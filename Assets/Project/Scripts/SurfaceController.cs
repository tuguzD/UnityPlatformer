using System;
using Minimalist.Quantity;
using UnityEngine;

// Completely new class, no source provided
public class SurfaceController : MonoBehaviour
{
    [Header("Surface types")] // Using existing class from "Minimalist Quantity System"
    public QuantityDynamicsType frictionType;
    public QuantityDynamicsType temperatureType;

    /* Source of method:
     * https://discussions.unity.com/t/is-there-a-way-to-alter-friction-and-bounciness-
     * from-a-script-that-is-a-component-of-a-gameobject-that-uses-them/253091/2 */
    private void Start()
    {
        var surface = GetComponent<Collider>();
        if (!surface) return;
        var material = surface.material;
        
        // Change #1: Set friction based on surface type
        switch (frictionType)
        {
            case QuantityDynamicsType.Accumulation:
                UpdateFriction(material, 0.75f);
                break;
            case QuantityDynamicsType.None:
                UpdateFriction(material, 0.5f);
                break;
            case QuantityDynamicsType.Depletion:
                UpdateFriction(material, 0.25f);
                break;
        }
    }

    private static void UpdateFriction(PhysicMaterial material, float friction)
    {
        material.staticFriction = friction;
        material.dynamicFriction = friction;
    }
    
    private void OnCollisionStay(Collision ball)
    {
        if (!ball.gameObject.CompareTag("Player")) return;
        var temperature = ball.transform.parent
            .gameObject.GetComponent<QuantityController>().temperature;

        if (temperatureType.Equals(QuantityDynamicsType.None))
            temperature.PassiveDynamics.Type = Math.Round(temperature.Amount, 2) == 0 ? temperatureType
                : (temperature.Amount > 0 ? QuantityDynamicsType.Depletion : QuantityDynamicsType.Accumulation);
        else temperature.PassiveDynamics.Type = temperatureType;
    }

    private void OnCollisionExit(Collision ball)
    {
        if (!ball.gameObject.CompareTag("Player")) return;
        ball.transform.parent.gameObject.GetComponent<QuantityController>()
            .temperature.PassiveDynamics.Type = QuantityDynamicsType.None;
    }
}