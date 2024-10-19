using UnityEngine;

// Source: https://github.com/deviantdear/RollABall/blob/master/Assets/Scripts/Rotator.cs
public class Rotator : MonoBehaviour
{
    public float rotation = 30.0f;
    void Update()
    {
        transform.Rotate(new Vector3(0, rotation, 0) * Time.deltaTime);
    }
}