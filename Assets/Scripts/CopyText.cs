using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyText : MonoBehaviour
{
    public void Copy()
    {
        target.text = inputField.text;
    }

    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI target;
}
