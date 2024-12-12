using System.Collections;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TileBoard board;
    [SerializeField] private CanvasGroup gameOver;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;

    [SerializeField] private AudioSource backgroundMusic; // Reference to the AudioSource for background music
    [SerializeField] private AudioSource gameOverSound;   // Reference to the AudioSource for the game-over sound
    [SerializeField] private AudioSource swipeSound;       // Reference to the AudioSource for the swipe sound
    public int score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        // Reset score
        SetScore(0);
        hiscoreText.text = LoadHiscore().ToString();

        // Hide game-over screen
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        // Update board state
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;

        // Play background music
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true; // Ensure it loops
            backgroundMusic.Play();
        }
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;

        StartCoroutine(Fade(gameOver, 1f, 1f));

        // Stop background music and play the game-over sound
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
        if (gameOverSound != null)
        {
            gameOverSound.Play();
        }
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();

        SaveHiscore();
    }

    private void SaveHiscore()
    {
        int hiscore = LoadHiscore();

        if (score > hiscore)
        {
            PlayerPrefs.SetInt("hiscore", score);
        }
    }

    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("hiscore");
        hiscoreText.text = "0";
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PlaySwipeSound()
    {
        if (swipeSound != null)
        {
            swipeSound.Play();
        }
    }
}
