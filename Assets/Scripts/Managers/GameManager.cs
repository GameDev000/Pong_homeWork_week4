using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int targetGoals = 5; //target
    [Header("Players")]
    [SerializeField] private PlayerMovment L_Player;
    [SerializeField] private PlayerMovment R_Player;

    [Header("Score")]
    public int leftScore = 0;
    public int rightScore = 0;
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;

    [SerializeField] private MoveDisk disk;

    [Header("Timer")]
    [SerializeField] private float gameDuration = 60f;
    [SerializeField] private TextMeshProUGUI timerText;
    private float timeLeft;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    private bool isGameOver = false;

    private void Start()
    {
        timeLeft = gameDuration;
        UpdateScoreUI();
        UpdateTimerUI();
        //KeyCode to players
        if (R_Player)
            R_Player.SetKeys(KeyCode.UpArrow, KeyCode.DownArrow);
        if (L_Player)
            L_Player.SetKeys(KeyCode.W, KeyCode.S);

        gameOverPanel.SetActive(false);
    }

    //update time
    private void Update()
    {
        if (isGameOver)
            return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateTimerUI();
            EndGameByTime();
            return;
        }
        UpdateTimerUI();
    }

    //++Score, called after Goal.  CheckWinCondition() after ++Score.
    public void ScoreLeftPlayer()
    {
        if (isGameOver)
            return;
        leftScore++;
        UpdateScoreUI();
        CheckWinCondition();
        disk.Reset();
    }
    public void ScoreRightPlayer()
    {
        if (isGameOver)
            return;
        rightScore++;
        UpdateScoreUI();
        CheckWinCondition();
        disk.Reset();
    }

    //--Score, called after OutOfLimits
    public void UpdateScoreLeft()
    {
        leftScore = Mathf.Max(0, leftScore - 1);
        UpdateScoreUI();
    }

    public void UpdateScoreRight()
    {
        rightScore = Mathf.Max(0, rightScore - 1);
        UpdateScoreUI();
    }

    //helpers fun
    private void UpdateScoreUI()
    {
        leftScoreText.text = leftScore.ToString();
        rightScoreText.text = rightScore.ToString();
    }

    private void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(timeLeft); //CeilToInt()  --> time up
        int minutes = seconds / 60;
        seconds %= 60;
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void EndGameWithWinner(PlayerMovment winner, string defaultName)
    {
        if (isGameOver)
            return;

        isGameOver = true;
        gameOverPanel.SetActive(true);

        string winnerName = defaultName;
        gameOverText.text = $"{winnerName} Wins!";
    }

    private void CheckWinCondition()
    {
        if (leftScore >= targetGoals)
            EndGameWithWinner(L_Player, "Left Player");
        else if (rightScore >= targetGoals)
            EndGameWithWinner(R_Player, "Right Player");
    }

    private void EndGameByTime()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        gameOverPanel.SetActive(true);
        gameOverText.text = "There is no winner\nTry again!";
    }
}

