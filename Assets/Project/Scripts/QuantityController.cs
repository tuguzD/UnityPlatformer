using Minimalist.Quantity;
using UnityEngine;

public class QuantityController : MonoBehaviour
{
    // [Header("Friction parameters")]
    // public float frictionBase = 0.5f;
    // public float frictionDelta = 0.25f;
    //
    // [Header("Temperature parameters")]
    // public QuantityBhv temperature;
    // public float temperatureLowerBound = 0.25f;
    // public float temperatureUpperBound = 0.75f;
    // public float temperatureDelta = 0.05f;
    
    [Header("Subjective Quantities")] // Example from PlayerBhv.cs
    public QuantityBhv temperature;
    public QuantityBhv durability;
    public QuantityBhv velocity;
    public QuantityBhv size;

    [Header("Objective Quantities")] // Example from PlayerBhv.cs
    public QuantityBhv spikiness;
    public QuantityBhv bounciness;
    public QuantityBhv plasticity;
    public QuantityBhv magnetisation;

    [Header("Pick-ups")] public Transform pickUpParent;
}