using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BreakableObject : MonoBehaviour, ITargetable
{
    [SerializeField]
    private GameObject FullObj;
    [SerializeField]
    private GameObject ShatterObj;
    [SerializeField]
    private Transform explodePoint;

    public UnityEvent OnBreak;

    [SerializeField]
    private float force = 100f;
    [SerializeField]
    private GameObject[] shardPeices;

    private void Awake()
    {
        Switch(true);
        
    }
    public void OnTriggerEnter(Collider other)
    {
        //if (!other.CompareTag("Objects"))
        if(other.gameObject.layer != LayerMask.NameToLayer("Objects"))
        {
            OnBreak.Invoke();
            Switch(false);
            Destroy(gameObject, 3f);
        }
    }

    public void OnHit()
    {
        OnBreak.Invoke();
        Debug.Log("collided");
        Switch(false);
        Destroy(gameObject, 3f);


        Explode();

    }
     
    public void Explode()
    {
        foreach (GameObject obj in shardPeices)
        {
            Vector3 dir = obj.transform.position - explodePoint.position;
            obj.GetComponent<Rigidbody>().AddForce(dir * force);
        }
    }


    private void Switch(bool change)
    {
        FullObj.SetActive(change);
        ShatterObj.SetActive(!change);
    }

}
