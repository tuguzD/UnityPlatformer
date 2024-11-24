using UnityEngine;

public class MagnetismController : MonoBehaviour
{
    private QuantityController _quantities;

    private readonly MinMaxPair
        _fieldOpacity = new(min: .0f, max: .15f);

    private readonly MinMaxPair
        _fieldScale = new(min: 0f, max: 2f);

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();
    }

    private void FixedUpdate()
    {
        var magnetisation = _quantities.magnetisation.FillAmount;

        SetOpacity(_fieldOpacity.Scaled(magnetisation));
        this.UniformScale(_fieldScale.Scaled(magnetisation));
    }

    private void SetOpacity(float opacity)
    {
        foreach (var material in GetComponent<MeshRenderer>().materials)
            material.color = new Color(
                material.color.r, material.color.g, material.color.b, opacity
            );
    }
}