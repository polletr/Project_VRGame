using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, ITargetable
{
    [SerializeField]
    private GameObject FullObj;
    [SerializeField]
    private GameObject ShatterObj;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Switch(true);
        
    }
    public void OnTriggerEnter(Collider other)
    {
        //if (!other.CompareTag("Objects"))
        if(other.gameObject.layer != LayerMask.NameToLayer("Objects"))
        {
            Switch(false);
            Destroy(gameObject, 3f);
        }
    }

    void ITargetable.OnHit()
    {
        Debug.Log("collided");
        Switch(false);
        Destroy(gameObject, 3f);


        rb.AddForce(new Vector3(1,1,1));

    }


    private void Switch(bool change)
    {
        FullObj.SetActive(change);
        ShatterObj.SetActive(!change);
    }

}
