using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject creditsMenu;
    public GameObject mainMenu;

    public GameObject creditsBack;
    public Transform creditsBackPos;

    public GameObject creditButton;
    public Transform creditButtonPos;

    public float delay = 5f;


    private void Awake()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        RespawnCreditsButton();
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

    public void OnToggleCredits()
    {
        creditsMenu.SetActive(!creditsMenu.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);
    }

    public void RespawnCreditsBack()
    {
        GameObject obj = Instantiate(creditsBack, creditsBackPos.position, Quaternion.identity);
        obj.SetActive(true);

    }
    public void RespawnCreditsButton()
    {
        GameObject obj = Instantiate(creditButton, creditButtonPos.position, Quaternion.identity);
        obj.SetActive(true);
    }

}
