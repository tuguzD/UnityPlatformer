using UnityEngine;

public class MagnetismController : MonoBehaviour
{
    private QuantityController _quantities;
    
    private const float FieldOpacityMin = .0f;
    private const float FieldOpacityMax = .2f;
    
    private const float FieldScaleMin = 0f;
    private const float FieldScaleMax = 2f;

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();
    }

    private void FixedUpdate()
    {
        var magnetisation = _quantities.magnetisation.FillAmount;
        
        var value = FieldOpacityMin + magnetisation * (FieldOpacityMax - FieldOpacityMin);
        SetOpacity(value);

        value = FieldScaleMin + magnetisation * (FieldScaleMax - FieldScaleMin);
        transform.localScale = new Vector3(value, value, value);
    }

    private void SetOpacity(float opacity)
    {
        foreach (var material in GetComponent<MeshRenderer>().materials)
            material.color = new Color(
                material.color.r, material.color.g, material.color.b, opacity
            );
    }
}