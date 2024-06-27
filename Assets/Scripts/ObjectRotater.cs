using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotater : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool canRotate;
    private Rigidbody rb;

    void Start()
    {
        canRotate = true;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (canRotate)
            RotateObject();
    }

    void RotateObject()
    {
        rb.transform.Rotate(0, 0, speed);
        rb.transform.Rotate(0, speed, 0);
    }

    public void ToggleRotation()
    {
        canRotate = !canRotate;
    }
}
