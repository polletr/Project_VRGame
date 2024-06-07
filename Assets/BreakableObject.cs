using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField]
    private MeshCollider meshCollider;

    [SerializeField]
    private GameObject FullObj;
    [SerializeField]
    private GameObject ShatterObj;


    private void Awake()
    {
        Switch(true);
        
    }
    public void OnTriggerEnter(Collider other)
    {
        //if (!other.CompareTag("Objects"))
        if(other.gameObject.layer != LayerMask.NameToLayer("Objects"))
        {
            Switch(false);
            Debug.Log("collided");
            Destroy(gameObject, 3f);
        }
    }

    private void Switch(bool change)
    {
        FullObj.SetActive(change);
        ShatterObj.SetActive(!change);
    }

}
