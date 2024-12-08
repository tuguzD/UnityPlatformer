using UnityEngine;

/* Source of class:
 * https://gist.github.com/seferciogluecce/e57dd9e884bd38d2925f3de7826f5dd4 */
public class DragAndShoot : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _stopPosition;

    public Rigidbody rb;

    // TODO: switch to 
    private void OnMouseDown()
    {
        _startPosition = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        _stopPosition = Input.mousePosition;

        var force = _startPosition - _stopPosition;
        if (force.y > 0)
            rb.AddForce(new Vector3(force.x, force.y, force.y));
    }
}