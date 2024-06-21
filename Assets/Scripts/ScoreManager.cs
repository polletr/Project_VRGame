using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    public GameEvent Event;

    private int score = 0;

    private void Awake()
    {
        SetText();  
    }

    public void AddScore(int amount)
    {
        score += amount;
        SetText();
    }

    public void ResetScore()
    {
        score = 0;
       SetText();
    }

    private void SetText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    private void OnEnable()
    {
        Event.OnBreak.AddListener(AddScore);
    }

    private void OnDisable()
    {
        Event.OnBreak.RemoveListener(AddScore);
    }
}

