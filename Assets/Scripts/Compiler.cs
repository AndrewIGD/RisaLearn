using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hec;
using TMPro;

public class Compiler : MonoBehaviour
{
    public void Compile()
    {
        try
        {
            HecType type = Hec.Compiler.Compile(sourceField.text);

            switch (type)
            {
                case HecType.Unknown:
                    console.text = "<color=#D32F2F>Error</color>: Compilation not successful.";
                    break;

                case HecType.Type1:
                    console.text = "<color=#D32F2F>Hack detected!</color> Executing <color=#F4511E>cout</color> hack...";
                    break;

                case HecType.Type2:
                    console.text = "<color=#D32F2F>Hack detected!</color> Executing <color=#F4511E>strtok</color> hack...";
                    break;

                case HecType.Type3:
                    console.text = "<color=#D32F2F>Hack detected!</color> Executing <color=#F4511E>while</color> hack...";
                    break;
            }
        }
        catch
        {
            console.text = "<color=#D32F2F>System overload</color>: Aborting compilation.";
        }
    }

    [SerializeField] InputField sourceField;
    [SerializeField] TextMeshProUGUI console;
}
