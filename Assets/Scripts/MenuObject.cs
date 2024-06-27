using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(ObjectRotater))]
public class MenuObject : MonoBehaviour , ITargetable
{
    [SerializeField]
    private GameObject FullObj;
    [SerializeField]
    private GameObject ShatterObj;
    [SerializeField]
    private Transform explodePoint;
    [SerializeField]
    private float force = 100f;
    [SerializeField]
    private GameObject[] shardPeices;

     public UnityEvent OnHitEvent;

    private ObjectRotater objRotater;

    private void Awake()
    {
        objRotater = GetComponent<ObjectRotater>();
        Switch(true);
    }

    public void OnHit()
    {

        objRotater.ToggleRotation();
        Debug.Log("collided");
        Switch(false);
        Destroy(gameObject, 3f);

        Explode();
       OnHitEvent.Invoke();
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

