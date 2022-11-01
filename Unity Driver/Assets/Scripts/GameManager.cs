using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Instance not specified");
            }
            return _instance;
        }
    }

    [SerializeField] private Text _currentLapText;
    [SerializeField] private Text _currentTimeLapText;
    [SerializeField] private Text _1stLapTimeText;
    [SerializeField] private Text _2ndLapTimeText;
    [SerializeField] private Text _3rdLapTimeText;
    [SerializeField] private Text _totalTimeText;
    [SerializeField] private Text _bestTimeText;
    [SerializeField] private GameObject _finishTriger;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _newBestText;

    private float _currentTimeLap;
    private float _currentLap;
    private float _1stLapTime;
    private float _2ndLapTime;
    private float _3rdLapTime;
    private float _totalTime;
    private float _bestTime;
    private bool _isPaused;

    public bool GameIsPaused => _isPaused;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _bestTime = PlayerPrefs.GetFloat("bestTime");
    }

    private void Update()
    {
        if (_isPaused)
        {
            return;
        }

        if (_currentLap >= 1 && _currentLap < 4)
        {
            _currentTimeLap += Time.deltaTime;
            EnebleFinishLine();
        }

        if (_currentLap > 3)
        {
            _currentLapText.text = "3/3";
            _currentTimeLapText.text = _3rdLapTimeText.text;

            ActiveMenu();
        }

        UpdateTimeText(_currentTimeLapText, _currentTimeLap);
    }

    private void ActiveMenu()
    {
        _menuPanel.SetActive(true);
        _isPaused = true;
    }

    private void RemeberBestTime()
    {
        if (_bestTime > _totalTime)
        {
            _newBestText.SetActive(true);

            _bestTime = _totalTime;
            _bestTimeText.text = _totalTimeText.text;

            PlayerPrefs.SetFloat("bestTime", _bestTime);
            PlayerPrefs.SetString("bestTimeOnMenu", _bestTimeText.text);
        }
        else
        {
            _bestTimeText.text = PlayerPrefs.GetString("bestTimeOnMenu");
        }
    }

    private void RememberTimeOfEachLap()
    {
        if (_currentLap == 2)
        {
            _1stLapTime = _currentTimeLap;
            UpdateTimeText(_1stLapTimeText, _currentTimeLap);

            _totalTime += _1stLapTime;
        }
        else if (_currentLap == 3)
        {
            _2ndLapTime = _currentTimeLap;
            UpdateTimeText(_2ndLapTimeText, _currentTimeLap);

            _totalTime += _2ndLapTime;
        }
        else if (_currentLap == 4)
        {
            _3rdLapTime = _currentTimeLap;
            UpdateTimeText(_3rdLapTimeText, _currentTimeLap);

            _totalTime += _3rdLapTime;
            UpdateTimeText(_totalTimeText, _totalTime);
            RemeberBestTime();
        }
    }

    private void EnebleFinishLine()
    {
        if (_currentTimeLap < 10)
        {
            _finishTriger.SetActive(false);
        }
        else
        {
            _finishTriger.SetActive(true);
        }
    }

    private void UpdateTimeText(Text text, float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = (time * 1000f) % 1000;

        text.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("000");
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void PressStartButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void AddCurrentLap()
    {
        _currentLap++;
        _currentLapText.text = _currentLap.ToString() + "/3";

        RememberTimeOfEachLap();

        if (_currentLap < 4)
        {
            _currentTimeLap = 0;
        }
    }
}
