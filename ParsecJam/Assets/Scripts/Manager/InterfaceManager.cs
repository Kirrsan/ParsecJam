using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{

    public static InterfaceManager instance;

    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _winPanel;

    [Header("WinPanel")]
    [SerializeField] private Text _winnerText;
    [SerializeField] private Button _firstSelectedWinButton;

    [Header("PausePanel")]
    [SerializeField] private Button _firstSelectedPauseButton;

    [Header("GamePanel")]
    [SerializeField] private Text _timerText;
    [SerializeField] private Image[] _dashCoolDown;
    [SerializeField] private Image[] _healthBar;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.onStateChange += () => {
            if (GameManager.instance.state == State.PAUSE)
            {
                GoInpause();
            }
            else if (GameManager.instance.state == State.INGAME)
            {
                GoToGame();
            }
            else if (GameManager.instance.state == State.WIN)
            {
                SetUpWinPanel();
                GoToWin();
            }
        };


        GoToGame();
    }

    // Update is called once per frame
    private void Update()
    {
        _timerText.text = FormatTime(LevelManager.instance.roundTimer);
    }

    private string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }

    public void FillCoolDown(int player, float fillAmount)
    {
        _dashCoolDown[player].fillAmount = fillAmount;
    }

    public void AdjustLifeBar(int index, float fillAmount)
    {
        _healthBar[index].fillAmount = fillAmount;
    }

    private void SetUpWinPanel()
    {
        string winner = ScoreManager.instance.GetWinner();
        if(winner == "tie")
        {
            _winnerText.text = "It's a tie !";
        }
        else{
            string[] row = winner.Split(new char[] { '-' });
            string point = " point";
            if(int.Parse(row[1]) > 1)
            {
                point = " points";
            }
            _winnerText.text = "The winner is P" + row[0] + " : " + row[1] + point;
        }
    }

    #region Show/HidePanels
    public void GoInpause()
    {
        Time.timeScale = 0;
        _firstSelectedWinButton.Select();
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(true);
        _firstSelectedPauseButton.Select();
    }

    private void GoToGame()
    {
        _pausePanel.SetActive(false);
        _winPanel.SetActive(false);
        _gamePanel.SetActive(true);
        Time.timeScale = 1;
    }

    private void GoToWin()
    {
        _firstSelectedPauseButton.Select();
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _winPanel.SetActive(true);
        _firstSelectedWinButton.Select();
    }
    #endregion

    #region Button Functions

    public void Resume()
    {
        GameManager.instance.ChangeState(State.INGAME);
        Time.timeScale = 1;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    #endregion
}
