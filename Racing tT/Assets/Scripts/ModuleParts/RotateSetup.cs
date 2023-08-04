using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSetup : MonoBehaviour
{
    public float rotationSpeed;

    public bool x;
    public bool y;
    public bool z;

    void Update()
    {
        float s = rotationSpeed * Time.deltaTime;
        if (y)
            transform.Rotate(Vector3.up * s);
        if (x)
            transform.Rotate(Vector3.right * s);
        if (z)
            transform.Rotate(Vector3.forward * s);
    }
}
