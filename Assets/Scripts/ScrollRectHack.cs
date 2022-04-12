using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRectHack : MonoBehaviour
{
    [SerializeField] RectTransform rect;

    public bool ignoreHack = false;

    Vector3 updatePos;
    private void Update()
    {
        updatePos = rect.position;
    }

    private void LateUpdate()
    {
        if (updatePos != rect.position && ignoreHack == false)
            rect.position = updatePos;

        ignoreHack = false;
    }
}
