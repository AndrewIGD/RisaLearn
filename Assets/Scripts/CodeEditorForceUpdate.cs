using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeEditorForceUpdate : MonoBehaviour
{
    RectTransform rect;
    ScrollRect scrollRect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        scrollRect = GetComponent<ScrollRect>();
    }

    private void LateUpdate()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }
}
