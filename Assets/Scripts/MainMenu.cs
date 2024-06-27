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


    private void Awake()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        RespawnCreditsButton();
    }

    public void PlayGame()
    {
       StartCoroutine(LoadGame()); 
    }

    public IEnumerator LoadGame()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(1);
        }
    }


    public void QuitGame()
    {
        Application.Quit();

/*        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
        }
*/    }

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
