using UnityEngine;

/* Source of class:
 * https://gist.github.com/Nova840/32866f7ce17341a9a47bd0480ab7f418 */
public class Follow : MonoBehaviour
{
    [SerializeField] private Transform follow;

    private Vector3 _originalLocalPosition;
    private Quaternion _originalLocalRotation;

    private void Awake()
    {
        _originalLocalPosition = follow.localPosition;
        _originalLocalRotation = follow.localRotation;
    }

    private void Update()
    {
        // move the parent to child's position
        transform.position = follow.position;

        // HAS TO BE IN THIS ORDER to sort of "reverse" the quaternion, so that
        // the local rotation is 0 if it is equal to the original local rotation
        follow.RotateAround(follow.position, follow.forward, -_originalLocalRotation.eulerAngles.z);
        follow.RotateAround(follow.position, follow.right, -_originalLocalRotation.eulerAngles.x);
        follow.RotateAround(follow.position, follow.up, -_originalLocalRotation.eulerAngles.y);

        // rotate the parent
        transform.rotation = follow.rotation;

        // moves the parent by the child's original offset from the parent
        transform.position += -transform.right * _originalLocalPosition.x;
        transform.position += -transform.up * _originalLocalPosition.y;
        transform.position += -transform.forward * _originalLocalPosition.z;

        // resets local rotation, undoing step 2
        follow.localRotation = _originalLocalRotation;

        // reset local position
        follow.localPosition = _originalLocalPosition;
    }
}