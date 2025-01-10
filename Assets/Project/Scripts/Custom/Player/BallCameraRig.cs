using Gaskellgames;
using Gaskellgames.CameraController;
using UnityEngine;

public class BallCameraRig : MonoBehaviour
{
    [SerializeField] private Vector3 position = new(0, 9, -9);
    private const float DeltaZ = -2;
    [SerializeField] private float rotationX = 20f;

    [LabelOverride("Top surface height")] [SerializeField]
    public float heightSurfaceTop = 17.32f;

    private Range _positionRangeY, _rotationRange, _heightMiddle;

    private void FixedUpdate()
    {
        var height = _playerController.ball.position.y;

        if (!_heightMiddle.Includes(height)) return;
        var ratio = _heightMiddle.InverseLerp(height);

        _cameraRig.followOffset = new Vector3(
            x: _cameraRig.followOffset.x,
            y: _positionRangeY.Lerp(ratio),
            z: position.z + (DeltaZ * Range.MiddleInterpolation(ratio))
        );

        _cameraRig.transform.localEulerAngles = new Vector3(
            x: _rotationRange.Lerp(ratio),
            y: _cameraRig.transform.localEulerAngles.y,
            z: _cameraRig.transform.localEulerAngles.z
        );
    }

    private CameraRig _cameraRig;
    private PlayerController _playerController;

    private void Start()
    {
        _cameraRig = FindObjectOfType<CameraRig>();
        _playerController = GetComponent<PlayerController>();
        _cameraRig.CameraFollow = _playerController.ball.transform;

        var localPosition = _playerController.ball.transform.localPosition.y;
        _heightMiddle = new Range(0 + localPosition, heightSurfaceTop - localPosition);

        _positionRangeY = new Range(position.y, -position.y);
        _rotationRange = new Range(rotationX, -rotationX);
    }
}