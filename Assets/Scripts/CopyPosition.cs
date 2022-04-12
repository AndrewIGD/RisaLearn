using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    float startPositionY;
    float elementStartPositionY;

    float startPositionX;
    float elementStartPositionX;

    private void OnEnable()
    {
        startPositionY = transform.position.y;
        elementStartPositionY = element.position.y;

        startPositionX = transform.position.x;
        elementStartPositionX = element.position.x;
    }

    void LateUpdate()
    {
            if (copyX)
                transform.position = new Vector2(element.position.x - elementStartPositionX + startPositionX, transform.position.y);

            if (copyY)
                transform.position = new Vector2(transform.position.x, element.position.y - elementStartPositionY + startPositionY);
    }

    [SerializeField] bool copyX = false;
    [SerializeField] bool copyY = false;
    [SerializeField] RectTransform element;
}
