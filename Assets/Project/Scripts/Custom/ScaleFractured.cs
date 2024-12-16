using UnityEngine;

public class ScaleFractured : ScaleToDestroy
{
    private void Start()
    {
        GetComponentInChildren<Rigidbody>().mass /= 100f;
        GetComponentInChildren<Fracture>().CauseFracture();
    }

    private new void FixedUpdate()
    {
        foreach (Transform _ in gameObject.transform)
            foreach (Transform child in _) ApplyLogic(child);
    }
}