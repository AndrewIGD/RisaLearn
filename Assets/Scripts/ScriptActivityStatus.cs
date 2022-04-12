using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptActivityStatus : MonoBehaviour
{
    private void Start()
    {
        instance = this;
        image = GetComponent<Image>();
    }

    public void Activate()
    {
        image.color = activeColor;
    }

    public void Deactivate()
    {
        image.color = inactiveColor;
    }

    public static ScriptActivityStatus instance;

    Image image;

    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;
}
