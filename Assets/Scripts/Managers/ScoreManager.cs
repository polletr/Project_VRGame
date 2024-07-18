using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Score Grades")]
    public int maxPointsInSong = 0;
    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI gradeText;

    public GameEvent Event;

    private int score = 0;
    private readonly string[] gradeList = new string[] { "F", "D", "C", "B", "A" };

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
        gradeText.text = "Grade " + GetGrade(maxPointsInSong);
    }
    private void OnEnable()
    {
        Event.OnBreak.AddListener(AddScore);
    }

    private void OnDisable()
    {
        Event.OnBreak.RemoveListener(AddScore);
    }

    public string GetGrade(int maxPoints)
    {
        int pointsPerGrade = maxPoints / gradeList.Length;

        for (int i = 0; i < gradeList.Length; i++)
        {
            int minScore = i * pointsPerGrade;
            int maxScoreExclusive = (i + 1) * pointsPerGrade - 1;

            if (score >= minScore && score <= maxScoreExclusive)
            {
                return gradeList[i];
            }
        }

        if (score >= maxPoints)
        {
            return "A+"; 
        }

        return "F";
    }

    public void OnBeatEvent()
    {
        Event.OnBeat.Invoke();
    }
}
