using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private const int _timeForRestartFinishTriger = 10;
    private const int _indexCorrection = 2;

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
    [SerializeField] private Text _totalTimeText;
    [SerializeField] private Text _bestTimeText;
    [SerializeField] private GameObject _finishTriger;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _newBestText;
    [SerializeField] private GameObject _startButton;
    [SerializeField] [Range(1, 6)] private int _numberOFLaps;
    [SerializeField] private List<Text> _lapsTimeTextList;
    [SerializeField] private List<GameObject> _linesOfLapsTimeTextList;

    private List<float> _lapsTimeList = new List<float>();
    private float _currentTimeLap;
    private float _currentLap;
    private float _totalTime;
    private float _bestTime;
    private bool _isPaused;

    static bool _gameIsStarted = false;

    public bool GameIsPaused => _isPaused;


    private void Awake()
    {
        _instance = this;

        for (int i = 0; i < _numberOFLaps; i++)
        {
            _linesOfLapsTimeTextList[i].SetActive(true);
        }

        _bestTime = PlayerPrefs.GetFloat("bestTime");
    }

    private void Start()
    {
        _currentLapText.text = "0/" + _numberOFLaps.ToString();

        if (!_gameIsStarted)
        {
            _isPaused = true;
            _startButton.SetActive(true);
        }
    }

    private void Update()
    {
        if (_isPaused)
        {
            return;
        }

        if (_currentLap >= 1 && _currentLap < _numberOFLaps + 1)
        {
            _currentTimeLap += Time.deltaTime;
            EnebleFinishLine();
        }

        if (_currentLap > _numberOFLaps)
        {
            _currentLapText.text = _numberOFLaps.ToString() + "/" + _numberOFLaps.ToString();
            _currentTimeLapText.text = _lapsTimeTextList[(int)_currentLap - _indexCorrection].text;

            ActiveMenu();
        }

        UpdateTimeText(_currentTimeLapText, _currentTimeLap);
    }

    private void ActiveMenu()
    {
        _menuPanel.SetActive(true);
        _isPaused = true;
    }

    private void RememberBestTime()
    {
        if (_bestTime > _totalTime || _bestTime == 0)
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

        if (_currentLap == _lapsTimeList.Count && _lapsTimeList.Count > 1)
        {
            var time = _lapsTimeList[_lapsTimeList.Count - 1];
            _totalTime += time;

            UpdateTimeText(_lapsTimeTextList[(int)_currentLap - _indexCorrection], _currentTimeLap);

            if (_currentLap > _numberOFLaps)
            {
                UpdateTimeText(_totalTimeText, _totalTime);
                RememberBestTime();
            }
        }
    }

    private void EnebleFinishLine()
    {
        if (_currentTimeLap < _timeForRestartFinishTriger)
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

    public void PressRestartButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void PressStartButton()
    {
        _startButton.SetActive(false);
        _isPaused = false;
        _gameIsStarted = true;
    }

    public void AddCurrentLap()
    {
        _currentLap++;
        _currentLapText.text = _currentLap.ToString() + "/" + _numberOFLaps.ToString();

        _lapsTimeList.Add(_currentTimeLap);

        RememberTimeOfEachLap();

        if (_currentLap < _numberOFLaps + 1)
        {
            _currentTimeLap = 0;
        }
    }
}
