using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Static Methods
    public static void Start(float seconds)
    {
        instance._start(seconds);
    }

    public static void Stop()
    {
        instance._stop();
    }
    #endregion

    #region Instance Methods
    public void _start(float seconds)
    {
        _time = seconds;
        _counting = true;
    }

    public void _stop()
    {
        _counting = false;
    }

    void Start()
    {
        instance = this;
        _countdownText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_counting)
        {
            _time -= Time.deltaTime;

            if (_time < 0)
            {
                _time = 0;
                _counting = false;

                TankReset.instance.ResetGame();
            }

            _countdownText.text = $"{_time:00.000}";
        }
    }
    #endregion

    #region Variables and Components
    public static Timer instance;

    bool _counting = false;
    float _time = 0;
    TextMeshProUGUI _countdownText;
    #endregion
}
