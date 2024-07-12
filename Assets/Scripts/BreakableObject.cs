using System.Collections;
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

    public Color redColor = Color.red;
    public Color greenColor = Color.green;

    private Renderer objRenderer;
    private Material objMaterial;

    [SerializeField]
    private Collider objCollider;

    [SerializeField]
    private float force = 100f;
    [SerializeField]
    private GameObject[] shardPeices;

    [SerializeField]
    private float timeToShoot = 1f;

    private bool isChanging = false;
    [SerializeField]
    private int redScoreValue;
    // Sine wave parameters
    [SerializeField]
    private float amplitude = 1f; // Amplitude of the sine wave
    private float frequency = 1f; // Frequency of the sine wave
    private float sineWaveOffset = 0f; // Offset for the sine wave calculation
    private bool canMove = true;
    private bool pointsAdded = false;
    private Color emissionColor;

    private void Awake()
    {
        objRenderer = FullObj.GetComponentInChildren<Renderer>();
        objCollider = GetComponent<Collider>();
        if (objRenderer != null)
        {
            objMaterial = objRenderer.material;
            objMaterial.color = redColor;
            objMaterial.EnableKeyword("_EMISSION"); // Enable emission
            objMaterial.SetColor("_EmissionColor", redColor);
        }

        frequency = Random.Range(2f, 5f);
        amplitude = Random.Range(10f, 20f);
        scoreValue = redScoreValue;
        Switch(true);
        Destroy(gameObject, 30f);

    }

    void Update()
    {
        if (canMove)
            Travel();
    }

    private void Travel()
    {
        sineWaveOffset += Time.deltaTime * frequency;
        Vector3 position = transform.position;
        position.y += Mathf.Sin(sineWaveOffset) * amplitude * Time.deltaTime;
        transform.position = position;
    }

    public void OnHit()
    {
        StopAllCoroutines();
        Event.OnBeat.RemoveListener(ChangeColorToGreen);

        Instantiate(pointPopUp, transform.position + new Vector3(0, 0, -4f), Quaternion.identity);
        objCollider.enabled = false;
        Event.OnBreak.Invoke(scoreValue);
        pointPopUp.SetDamageText(scoreValue);
        canMove = false;
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

    private IEnumerator ChangeColor()
    {
        scoreValue = baseScoreValue;

        objMaterial.color = greenColor;
        objMaterial.SetColor("_EmissionColor", greenColor);
        isChanging = true;
        float t = 0;
        while (t < timeToShoot && isChanging)
        {
            t += Time.deltaTime;
            float normalizedTime = t / timeToShoot;
            scoreValue = (int)(baseScoreValue * (1 - normalizedTime));

            emissionColor = Color.Lerp(greenColor, redColor, normalizedTime);
            objMaterial.color = emissionColor;
            objMaterial.SetColor("_EmissionColor", emissionColor);
            yield return null;
        }
        scoreValue = redScoreValue;

    }

    private void ChangeColorToGreen()
    {
        if (!pointsAdded)
        {
          ScoreManager.Instance.maxPointsInSong += baseScoreValue;
          pointsAdded = true;
        }
        StopAllCoroutines();
        isChanging = false;
        StartCoroutine(ChangeColor());
    }

    private void Switch(bool change)
    {
        FullObj.SetActive(change);
        ShatterObj.SetActive(!change);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AABox"))
        {
            Event.OnBeat.AddListener(ChangeColorToGreen); 
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("AABox"))
        {
            Event.OnBeat.RemoveListener(ChangeColorToGreen);
        }
    }


}

