using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] 
    private GameObject mainMenu;
    [SerializeField] 
    private GameObject creditsMenu;

    [Header("CreditsButtons")]
    [SerializeField] 
    private GameObject creditsBack;
    [SerializeField] 
    private GameObject creditButton;

    [Header("CreditsPositions")]
    [SerializeField]
    private Transform creditsBackPos;
    [SerializeField]
    private Transform creditButtonPos;

    public float delay = 5f;
    [SerializeField]
    private AudioClip BGMenuMusic;

    private void Awake()
    {
        AudioManager.Instance.PlayAudio(BGMenuMusic, true);
        creditsMenu.SetActive(false);
    }

    public void PlayGame() => StartCoroutine(PlayDelay());
    public void QuitGame() => StartCoroutine(QuitDelay());

    public IEnumerator PlayDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(1);
        }
    }
    public IEnumerator QuitDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif      
        }
    }

    private void DisableScreen()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void OnToggleCredits()
    {
        creditsMenu.SetActive(!creditsMenu.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);
    }

    public void RespawnCreditsBack()
    {
        GameObject obj = Instantiate(creditsBack, creditsBackPos.position, new Quaternion(0, 180, 0, 0));
        obj.SetActive(true);

    }
    public void RespawnCreditsButton()
    {
        GameObject obj = Instantiate(creditButton, creditButtonPos.position, new Quaternion(0, 180, 0, 0));
        obj.SetActive(true);
    }

}
