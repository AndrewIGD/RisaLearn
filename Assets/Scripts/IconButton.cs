using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] Sprite highlightSprite;
    [SerializeField] UnityEvent onClick;
    Sprite defaultSprite;
    Image icon;

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.sprite = highlightSprite;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        icon.sprite = defaultSprite;
    }

    private void Start()
    {
        icon = GetComponent<Image>();
        defaultSprite = icon.sprite;
    }


}
