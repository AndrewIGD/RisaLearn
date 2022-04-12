using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    [SerializeField] string settingName;
    [SerializeField] float defaultValue;

    UnityEngine.UI.Slider _slider;
    private void Start()
    {
        _slider = GetComponentInChildren<UnityEngine.UI.Slider>();

        _slider.value = PlayerPrefs.GetFloat(settingName, defaultValue);

        AudioListener.volume = _slider.value;
    }

    public void ValueChanged()
    {
        PlayerPrefs.SetFloat(settingName, _slider.value);

        AudioListener.volume = _slider.value;
    }
}
