using Gaskellgames;
using Gaskellgames.CameraController;
using UnityEngine;

public class BallCameraRig : MonoBehaviour
{
    [SerializeField] private Vector3 position = new(0, 9, -9);
    private const float DeltaZ = -2;
    [SerializeField] private float rotationX = 20f;

    [LabelOverride("Top surface height")]
    [SerializeField] private float heightSurfaceTop = 17.32f;

    private MinMaxPair _positionRangeY, _rotationRange, _heightMiddle;

    private void FixedUpdate()
    {
        var height = _playerController.ball.position.y;
        if (height > _heightMiddle.Minimum && height < _heightMiddle.Maximum)
        {
            var cameraRig = GetComponentInChildren<CameraRig>();
            var cameraObject = cameraRig.gameObject;

            var ratio = _heightMiddle.InverseLerp(height);

            cameraRig.followOffset.y = _positionRangeY.Scaled(ratio);
            cameraObject.transform.localEulerAngles = new Vector3(
                _rotationRange.Scaled(ratio),
                cameraObject.transform.localEulerAngles.y,
                cameraObject.transform.localEulerAngles.z
            );

            var ratioMiddle = ratio < 0.5 ? 2 * ratio : 2 * (1 - ratio);
            cameraRig.followOffset.z = position.z + (DeltaZ * ratioMiddle);
        }
    }

    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();

        var localPosition = _playerController.ball.transform.localPosition.y;
        _heightMiddle = new MinMaxPair(0 + localPosition, heightSurfaceTop - localPosition);

        _positionRangeY = new MinMaxPair(position.y, -position.y);
        _rotationRange = new MinMaxPair(rotationX, -rotationX);
    }
}