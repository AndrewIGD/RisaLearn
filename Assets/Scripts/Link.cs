using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField] string pageName;

    public void LoadPage()
    {
        Documentation documentation = FindObjectOfType<Documentation>();
        documentation.LoadPage(pageName);
    }
}
