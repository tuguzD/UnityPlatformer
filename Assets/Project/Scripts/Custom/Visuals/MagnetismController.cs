using UnityEngine;

public class MagnetismController : MonoBehaviour
{
    [SerializeField] private MeshRenderer forceField;

    private QuantityController _quantities;

    private readonly Range
        _fieldOpacity = new(min: .0f, max: .05f);

    private readonly Range
        _fieldScale = new(min: 0f, max: 2f);

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();
    }

    private void FixedUpdate()
    {
        var magnetisation = _quantities.magnetisation.FillAmount;

        foreach (var material in forceField.materials)
            material.SetOpacity(_fieldOpacity.Lerp(magnetisation));
        forceField.UniformScale(_fieldScale.Lerp(magnetisation));
    }
}