// Total changes: 1

using UnityEngine;

/* Source of class:
 * https://gist.github.com/seferciogluecce/e57dd9e884bd38d2925f3de7826f5dd4 */
public class DragAndShoot : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _stopPosition;

    public Rigidbody rb;
    public GameObject projectile;

    // TODO: switch to new input system
    private void OnMouseDown()
    {
        _startPosition = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        _stopPosition = Input.mousePosition;
        Shoot(_startPosition - _stopPosition);
    }

    private void Shoot(Vector3 input)
    {
        var force = new Vector3(input.x, input.y, input.y);
        rb.AddForce(force);

        // Change #1: Spawn projectile with opposite force (due to Newton's Third Law)
        var smth = Instantiate(projectile, transform.position, transform.rotation);
        smth.AddComponent<Rigidbody>();
        smth.GetComponent<Rigidbody>().AddForce(-force);
    }
}