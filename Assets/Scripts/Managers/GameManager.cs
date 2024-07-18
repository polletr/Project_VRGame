using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Volume volume; // Reference to the global volume in your scene
    [SerializeField] private float bloomIntensityMin;
    [SerializeField] private float bloomIntensityMax;
    [SerializeField] private float bloomTime = 1f;


    [SerializeField] private AudioClip EndGameClip;

    [Header("Menus")]
    [SerializeField] private GameObject ScoreUI;
    [SerializeField] private GameObject GradeUI;

    [Header("Events")]
    public GameEvent Event;


    private Bloom bloom;

    void Start()
    {
        // Get the Bloom component from the volume
        if (volume.profile.TryGet<Bloom>(out bloom))
        {
            bloom.intensity.value = bloomIntensityMin;
        }
        else
        {
            Debug.LogError("Bloom effect not found in the volume profile.");
        }
    }
    public void EndGame()
    {
        ScoreUI.SetActive(false);
        GradeUI.SetActive(true);
        AudioManager.Instance.PlayAudio(EndGameClip);
    }

    private IEnumerator SetBloomIntensity()
    {

        bloom.intensity.value = bloomIntensityMax;


        float t = 0;
        while (t < bloomTime)
        {
            t += Time.deltaTime;
            float normalizedTime = t / bloomTime;

            float bloomIntensity = Mathf.Lerp(bloomIntensityMax, bloomIntensityMin, normalizedTime);
            bloom.intensity.value = bloomIntensity;


            yield return null;
        }
    }
    private void BloomTime()
    {
        StartCoroutine(SetBloomIntensity());
    }

    private void OnEnable()
    {
        Event.OnBeat.AddListener(BloomTime);
    }

    private void OnDisable()
    {
        Event.OnBeat.RemoveListener(BloomTime);
    }

}
