using UnityEngine;

public class MagnetismController : MonoBehaviour
{
    [SerializeField] private MeshRenderer forceField;

    private readonly Range _fieldOpacity =
        new(min: .0f, max: .05f);
    private readonly Range _fieldScale =
        new(min: 0f, max: 2f);

    private void FixedUpdate()
    {
        var magnetisation = _quantities.magnetisation.FillAmount;

        forceField.materials[0].SetOpacity(_fieldOpacity.Lerp(magnetisation));
        forceField.materials[1].SetOpacity(_fieldOpacity.Lerp(magnetisation));
        
        forceField.UniformScale(_fieldScale.Lerp(magnetisation));

        DefyGravity(magnetisation, _playerController.ball);
    }

    [SerializeField] private float gravityModifier = 0.375f;

    private void DefyGravity(float magnetisation, Rigidbody body)
    {
        var modifier = _quantities.temperature.Amount > -0.75f
            ? magnetisation * gravityModifier * 1.25f
            : magnetisation + gravityModifier;
        var force = -Physics.gravity * body.mass;

        body.AddForce(force * modifier);
    }

    private QuantityController _quantities;
    private PlayerController _playerController;

    private void Start()
    {
        _quantities = GetComponentInParent<QuantityController>();
        _playerController = GetComponentInParent<PlayerController>();
    }
}