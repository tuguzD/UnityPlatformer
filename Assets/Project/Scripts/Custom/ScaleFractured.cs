using UnityEngine;

public class ScaleFractured : ScaleToDestroy
{
    private void Start()
    {
        GetComponentInChildren<Rigidbody>().mass /= 100f;
        GetComponentInChildren<Fracture>().CauseFracture();
    }

    private void FixedUpdate()
    {
        foreach (Transform _ in gameObject.transform)
            foreach (Transform child in _)
            {
                child.transform.localScale -= Vector3.one * speed;
                if (child.transform.localScale.x <= 0f)
                    Destroy(gameObject);
            }
    }
}