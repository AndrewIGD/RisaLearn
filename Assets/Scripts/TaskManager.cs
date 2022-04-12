using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] [TextArea] string title, description;

    [Header("Objects")]
    [SerializeField] TextMeshProUGUI taskTitle, taskDescription;

    public void ShowTask()
    {
        gameObject.SetActive(true);
    }

    public void HideTask()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        taskTitle.text = title;
        taskDescription.text = description;
    }
}
