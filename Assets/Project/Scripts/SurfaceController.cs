using System;
using Minimalist.Quantity;
using UnityEngine;

public class SurfaceController : MonoBehaviour
{
    public QuantityDynamicsType dynamicsType;

    private void OnCollisionStay(Collision ball)
    {
        if (!ball.gameObject.CompareTag("Player")) return;
        var temperature = ball.transform.parent
            .gameObject.GetComponent<PlayerController>().temperature;

        if (dynamicsType.Equals(QuantityDynamicsType.None))
            temperature.PassiveDynamics.Type = Math.Round(temperature.Amount, 2) == 0 ? dynamicsType
                : (temperature.Amount > 0 ? QuantityDynamicsType.Depletion : QuantityDynamicsType.Accumulation);
        else temperature.PassiveDynamics.Type = dynamicsType;
    }

    private void OnCollisionExit(Collision ball)
    {
        if (!ball.gameObject.CompareTag("Player")) return;
        ball.transform.parent.gameObject.GetComponent<PlayerController>()
            .temperature.PassiveDynamics.Type = QuantityDynamicsType.None;
    }
}