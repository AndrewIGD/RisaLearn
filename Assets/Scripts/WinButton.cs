using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinButton : MonoBehaviour
{
    [SerializeField] string scene;
    [SerializeField] bool changeName = true;

    private void Start()
    {
        if(changeName)  
        GetComponentInChildren<TextMeshProUGUI>().text = scene;
    }

    public void Load()
    {
        StartCoroutine(FindObjectOfType<WinCanvas>().LoadScene(scene));
    }
}
