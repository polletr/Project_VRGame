using UnityEngine;

public class MenuTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPanel;

    private void OnEnable()
    {
        TutorialManager.Instance.OnShowTutorial += ShowTutorial;
    }
    private void OnDisable()
    {
        TutorialManager.Instance.OnShowTutorial -= ShowTutorial;
    }

    private void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
    }
}
