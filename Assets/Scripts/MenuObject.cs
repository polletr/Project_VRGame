using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MenuObject : MonoBehaviour, ITargetable
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
    [SerializeField]
    private AudioClip menuBreakClip;

    public UnityEvent OnHitEvent;
    private Collider col;


    private void Awake()
    {
        col = GetComponent<Collider>();
        Switch(true);
    }

    public void OnHit()
    {
        col.enabled = false;
        Switch(false);
        Destroy(gameObject, 3f);
        AudioManager.Instance.PlayAudio(menuBreakClip);
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

