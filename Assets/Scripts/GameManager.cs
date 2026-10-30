using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _player1ScoreText;
    [SerializeField] private TextMeshProUGUI _player2ScoreText;
    
    [Header("UI Game Over")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _winnerText;
    [SerializeField] private TMP_Text _finalPlayer1ScoreText;
    [SerializeField] private TMP_Text _finalPlayer2ScoreText;
    
    [Header("UI Pause")]
    [SerializeField] private GameObject _pausePanel;
    
    [Header("Audio Effects")]
    [SerializeField] private AudioLowPassFilter _lowPassFilter;
    [SerializeField] private AudioClip _gameOverStinger;
    [SerializeField] private AudioSource _gameOverAudioSource;
    [SerializeField] private AudioSource _musicSource;
    
    [Header("Timer")]
    [SerializeField] private float _gameDuration = 180f;
    private float _remainingTime;
    
    // Singleton
    public static GameManager Instance { get; private set; }

    // Game States
    public enum GameState
    {
        _start,
        _playing,
        _paused,
        _gameOver
    }

    public GameState CurrentState { get; private set; }

    // Players
    [SerializeField] private GameObject[] _players;

    // Scores
    private int _player1Score;
    private int _player2Score;

    public int Player1Score
    {
        get { return _player1Score; }
        set { _player1Score = value; }
    }

    public int Player2Score
    {
        get { return _player2Score; }
        set { _player2Score = value; }
    }
    
    public void AddScore(PlayerID playerID, int points)
    {
        if (playerID == PlayerID.Player1)
        {
            Player1Score += points;
        }
        else
        {
            Player2Score += points;
        }
        
        UpdateScoreUI();
        
        Debug.Log("Player 1 Score: " + Player1Score);
        Debug.Log("Player 2 Score: " + Player2Score);
        
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CurrentState = GameState._playing;
        _remainingTime = _gameDuration;
        
        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void Update()
    {
        // ESC: salir del juego en cualquier momento
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
            return;
        }

        // P: pausar o reanudar
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (CurrentState == GameState._playing)
                PauseGame();
            else if (CurrentState == GameState._paused)
                ResumeGame();
        }
        
        
        if (CurrentState != GameState._playing)
        {
            return;
        }

        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0f)
        {
            _remainingTime = 0f;
            EndGame();
        }

        UpdateTimerUI();
    }
    
    public void PauseGame()
    {
        CurrentState = GameState._paused;
        Time.timeScale = 0f;
        _pausePanel.SetActive(true);

        _lowPassFilter.cutoffFrequency = 800f;
    }

    public void ResumeGame()
    {
        CurrentState = GameState._playing;
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
        
        _lowPassFilter.cutoffFrequency = 22000f;
    }
    
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60f);
        int seconds = Mathf.FloorToInt(_remainingTime % 60f);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void PlayAgain()
    {
        NewRound();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    private void UpdateScoreUI()
    {
        _player1ScoreText.text = Player1Score.ToString();
        _player2ScoreText.text = Player2Score.ToString();
    }
    
    private void EndGame()
    {
        CurrentState = GameState._gameOver;

        if (Player1Score > Player2Score)
        {
            ShowGameOver("PLAYER 1 WINS!");
        }
        else if (Player2Score > Player1Score)
        {
            ShowGameOver("PLAYER 2 WINS!");
        }
        else
        {
            ShowGameOver("DRAW!");
        }
    }
    
    private void EndGameByDeath()
    {
        CurrentState = GameState._gameOver;

        if (!_players[0].activeSelf)
        {
            ShowGameOver("PLAYER 2 WINS!");
        }
        else if (!_players[1].activeSelf)
        {
            ShowGameOver("PLAYER 1 WINS!");
        }
        else
        {
            ShowGameOver("DRAW!");
        }
        
    }
    
    
    public void CheckWin()
    {
        int aliveCount = 0;

        foreach (GameObject player in _players)
        {
            if (player.activeSelf)
            {
                aliveCount++;
            }
        }

        if (aliveCount <= 1)
        {
            EndGameByDeath();
            ;
        }
    }
    
    private void ShowGameOver(string result)
    {
        CurrentState = GameState._gameOver;

        _winnerText.text = result;
        _finalPlayer1ScoreText.text = "PLAYER 1: " + Player1Score;
        _finalPlayer2ScoreText.text = "PLAYER 2: " + Player2Score;
        
        if (_musicSource != null)
        {
            _musicSource.Stop();
        }
        
        if (_gameOverAudioSource != null && _gameOverStinger != null)
        {
            _gameOverAudioSource.PlayOneShot(_gameOverStinger);
        }

        _gameOverPanel.SetActive(true);

        _timerText.gameObject.SetActive(false);
        _player1ScoreText.gameObject.SetActive(false);
        _player2ScoreText.gameObject.SetActive(false);
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitSafeTheJungle()
    {
        Time.timeScale = 1f;
        Application.Quit();

        Debug.Log("Game Closed");
    }
}