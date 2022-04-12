using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaretPositionCopy : MonoBehaviour
{
    [SerializeField] TMP_CaretInputField inputField;
    [SerializeField] RectTransform viewport;
    [SerializeField] float offset;

    RectTransform localToWorldConverter;
    RectTransform rect;
    private void Start()
    {
        localToWorldConverter = new GameObject().AddComponent<RectTransform>();
        rect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
            try
            {
                if (inputField.isFocused)
                {
                    Vector2 caretPosition = CaretPosition, bottomRight = BottomRight, topLeft = TopLeft;

                    if (caretPosition.y < bottomRight.y)
                    {
                        transform.position += new Vector3(0, bottomRight.y - caretPosition.y + offset);
                    }

                    if (caretPosition.y > topLeft.y)
                    {
                        transform.position -= new Vector3(0, caretPosition.y - topLeft.y + offset);
                    }

                    if (caretPosition.x < topLeft.x)
                    {
                        transform.position += new Vector3(topLeft.x - caretPosition.x + offset, 0);
                    }

                    if (caretPosition.x > bottomRight.x)
                    {
                        transform.position -= new Vector3(caretPosition.x - bottomRight.x + offset, 0);
                    }
                }
            }
            catch { }
        
    }

    Vector2 CaretPosition
    {
        get
        {
            return inputField.transform.TransformPoint(inputField.GetLocalCaretPosition());
        }
    }

    Vector2 BottomRight
    {
        get
        {
            localToWorldConverter.parent = viewport.transform;
            localToWorldConverter.anchorMax = localToWorldConverter.anchorMin = new Vector2(1f, 0f);
            localToWorldConverter.anchoredPosition = new Vector2(0, 0);

            return localToWorldConverter.position;
        }
    }

    Vector2 TopLeft
    {
        get
        {
            localToWorldConverter.parent = viewport.transform;
            localToWorldConverter.anchorMax = localToWorldConverter.anchorMin = new Vector2(0f, 1f);
            localToWorldConverter.anchoredPosition = new Vector2(0, 0);

            return localToWorldConverter.position;
        }
    }
}
