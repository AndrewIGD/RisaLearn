using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] string settingName;
    [SerializeField] int defaultValue;

    TextMeshProUGUI _buttonText;
    private void Start()
    {
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();

        _buttonText.text = settingName + ": " + (PlayerPrefs.GetInt(settingName, defaultValue) == 1 ? "On" : "Off");
    }

    public void Toggle()
    {
        PlayerPrefs.SetInt(settingName, PlayerPrefs.GetInt(settingName, defaultValue) == 1 ? 0 : 1);

        _buttonText.text = settingName + ": " + (PlayerPrefs.GetInt(settingName, defaultValue) == 1 ? "On" : "Off");
    }
}
