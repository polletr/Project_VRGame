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
    private DamagePopUp pointPopUp;

    [SerializeField]
    private int baseScoreValue = 100;
    private int scoreValue;

    public GameEvent Event;
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    private Renderer objRenderer;
    private Material objMaterial;

    [SerializeField]
    private float force = 100f;
    [SerializeField]
    private GameObject[] shardPeices;

    private float timerToShoot = 0f;
    [SerializeField]
    private float maxTimeToShoot = 1f;


    private void Awake()
    {
        timerToShoot = 0f;
        objRenderer = FullObj.GetComponentInChildren<Renderer>();
        if (objRenderer != null)
        {
            objMaterial = objRenderer.material;
            objMaterial.EnableKeyword("_EMISSION"); // Enable emission
        }
        Switch(true);
        Destroy(gameObject, 7f);
        
    }

    void Update()
    {
        timerToShoot += Time.deltaTime;
        float t = timerToShoot / maxTimeToShoot;
        if (t >= 1f)
            t = 1f;
        // Lerp the color based on the time on screen
        if (objMaterial != null)
        {
            Color emissionColor = Color.Lerp(startColor, endColor, t);
            objMaterial.SetColor("_EmissionColor", emissionColor);
        }
        // Adjust the score value based on the time on screen
        scoreValue = Mathf.CeilToInt(baseScoreValue * (1-t));
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
        Instantiate(pointPopUp, transform.position, Quaternion.identity);

        Event.OnBreak.Invoke(scoreValue);
        pointPopUp.SetDamageText(scoreValue);
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
