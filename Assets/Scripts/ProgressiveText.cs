using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressiveText : MonoBehaviour
{
    [SerializeField] float timeBetweenTextAppearances;
    [SerializeField] [TextArea] string existingText;
    [SerializeField] [TextArea] string progressiveText;
    [SerializeField] GameObject[] activeOnEnd;
    [SerializeField] GameObject[] activateOnStart;

    TextMeshProUGUI _text;
    int _index = 0;

    AudioSource source;

    void OnDisable()
    {
        for (int i = 0; i < activateOnStart.Length; i++)
            activateOnStart[i].SetActive(false);
    }

    public void OnEnable()
    {
        timeBetweenTextAppearances = 0.025f;

        _text = GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < activateOnStart.Length; i++)
            activateOnStart[i].SetActive(true);

        for (int i=0;i<activeOnEnd.Length;i++)
            activeOnEnd[i].SetActive(false);

        _text.text = existingText;

        _index = 0;

        source = gameObject.AddComponent<AudioSource>();
        source.clip = Resources.Load<AudioClip>("Sounds/type");

        StartCoroutine(Progress());
    }

    IEnumerator Progress()
    {
        while(_index != progressiveText.Length)
        {
            yield return new WaitForSecondsRealtime(timeBetweenTextAppearances);

            try
            {
                while (progressiveText[_index] == '<' && progressiveText[_index+1] != '=' && _index != progressiveText.Length)
                {
                    while (progressiveText[_index] != '>' && _index != progressiveText.Length)
                        _text.text += progressiveText[_index++].ToString();

                    _text.text += progressiveText[_index++].ToString();
                }
            }
            catch
            {

            }

            if (_index != progressiveText.Length)
            {
                _text.text += progressiveText[_index++].ToString();

                source.Play();
            }
        }

        for (int i = 0; i < activeOnEnd.Length; i++)
            activeOnEnd[i].SetActive(true);
    }
}
