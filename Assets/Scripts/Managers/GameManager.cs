using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject TutorialText;


    [Header("Events")]
    public GameEvent Event;


    private Bloom bloom;

    public bool tutorialOn;

    private void OnEnable()
    {
        Event.OnBreak.AddListener(DisableTutorial);
        Event.OnBeat.AddListener(BloomTime);

    }

    private void OnDisable()
    {
        Event.OnBreak.RemoveListener(DisableTutorial);
        Event.OnBeat.RemoveListener(BloomTime);

    }

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
        tutorialOn = true;
        Coroutine tutorialFadeCoroutine = StartCoroutine(TutorialFade());
    }
    public void EndGame()
    {
        ScoreUI.SetActive(false);
        GradeUI.SetActive(true);
        AudioManager.Instance.PlayAudio(EndGameClip);
    }

    private void DisableTutorial(int score)
    {
        if (score > 20)
        {
            tutorialOn = false;
        }
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

    public void OnPlayAgain() => StartCoroutine(PlayDelay());
    public void OnQuit() => StartCoroutine(QuitDelay());


    public IEnumerator PlayDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator TutorialFade()
    {
        CanvasGroup canvasGroup = TutorialText.GetComponent<CanvasGroup>();

        while (tutorialOn)
        {
            // Fade in
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1.0f)
            {
                canvasGroup.alpha = t;
                yield return null;
            }
            canvasGroup.alpha = 1.0f;

            yield return new WaitForSeconds(1f);

            // Fade out
            for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime / 1.0f)
            {
                canvasGroup.alpha = t;
                yield return null;
            }
            canvasGroup.alpha = 0.0f;
        }
    }
    public IEnumerator QuitDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Application.Quit();
            SceneManager.LoadScene(0);
        }
    }

}
