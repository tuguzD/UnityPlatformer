using Minimalist.Quantity;
using UnityEngine;

public class QuantityController : MonoBehaviour
{
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