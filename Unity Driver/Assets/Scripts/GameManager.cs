using UnityEngine;
using UnityEngine.UI;

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

    private float _currentTimeLap;
    private float _currentLap;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {

    }

    private void Update()
    {
        Debug.Log(_currentLap);

        _currentTimeLap += Time.deltaTime;

        UpdateTimeText();
    }

    public void CurrentLap()
    {
        _currentLap++;
        _currentLapText.text = _currentLap.ToString() + "/3";
    }

    private void UpdateTimeText()
    {
        float minutes = Mathf.FloorToInt(_currentTimeLap / 60);
        float seconds = Mathf.FloorToInt(_currentTimeLap % 60);
        float milliseconds = (_currentTimeLap * 1000f) % 1000;

        _currentTimeLapText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");

    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
