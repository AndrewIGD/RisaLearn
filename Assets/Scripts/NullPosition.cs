using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NullPosition : MonoBehaviour
{
    RectTransform rect;
    RectTransform caretRect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        rect.anchoredPosition = Vector2.zero;

        return;

        if (caretRect == null)
            caretRect = transform.parent.GetComponentInChildren<TMP_SelectionCaret>().GetComponent<RectTransform>();
        else caretRect.anchoredPosition = Vector2.zero;
    }
}
