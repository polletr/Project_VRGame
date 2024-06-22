using UnityEngine;

public class BreakableObject : MonoBehaviour, ITargetable
{
    [SerializeField]
    private GameObject FullObj;
    [SerializeField]
    private GameObject ShatterObj;
    [SerializeField]
    private Transform explodePoint;

    [SerializeField]
    private int scoreValue = 1;

    public GameEvent Event;

    [SerializeField]
    private float force = 100f;
    [SerializeField]
    private GameObject[] shardPeices;

    private void Awake()
    {
        Switch(true);
        Destroy(gameObject,7f);

    }
    public void OnTriggerEnter(Collider other)
    {
        //if (!other.CompareTag("Objects"))
        if (other.gameObject.layer != LayerMask.NameToLayer("Objects"))
        {
            //Event.OnBreak.Invoke(scoreValue);
            Switch(false);
            Destroy(gameObject, 3f);
        }
    }

    public void OnHit()
    {
        Event.OnBreak.Invoke(scoreValue);
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
