using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public void CaretToEnd()
    {
        scrollRect.verticalNormalizedPosition = 0;
    }

    TMP_InputField inputField;
    ScrollRect scrollRect;

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        scrollRect = GetComponentInChildren<ScrollRect>();
    }
}
