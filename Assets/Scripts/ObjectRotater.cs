using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ObjectRotater : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool rotateX;
    [SerializeField] private bool rotateY;
    [SerializeField] private bool rotateZ;

    private bool canRotate = true;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (canRotate)
       RotateObject();
    }

    void RotateObject()
    {
        if (rotateX) rb.transform.Rotate(speed, 0, 0);
        if (rotateY) rb.transform.Rotate(0, speed, 0);
        if (rotateZ) rb.transform.Rotate(0, 0, speed);
    }

    public void ToggleRotation()
    {
        canRotate = !canRotate;
    }

}
