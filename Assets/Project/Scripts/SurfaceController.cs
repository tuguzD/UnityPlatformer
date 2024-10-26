using System;
using Minimalist.Quantity;
using UnityEngine;

// Completely new class, no source provided
public class SurfaceController : MonoBehaviour
{
    [Header("Surface types")] // Using existing class from "Minimalist Quantity System"
    public QuantityDynamicsType frictionType;
    public QuantityDynamicsType temperatureType;

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