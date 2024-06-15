using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotater : MonoBehaviour
{
    public bool canRotate;
    [SerializeField] private float speed;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
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
