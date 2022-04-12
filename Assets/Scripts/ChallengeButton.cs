using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : MonoBehaviour
{
    [SerializeField] string challengeName;
    [SerializeField] Color unsolvedColor, solvedColor;
    [SerializeField] MainMenuButtons mainMenu;

    Image _image;
    private void Start()
    {
        _image = GetComponent<Image>();

        if (PlayerPrefs.GetInt(challengeName, 0) == 0)
            _image.color = unsolvedColor;
        else _image.color = solvedColor;
    }

    public void Load()
    {
        StartCoroutine(mainMenu.LoadScene(challengeName));
    }
}
