using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMP_CaretInputField : TMP_InputField
{
    public Vector2 GetLocalCaretPosition()
    {
        if (isFocused)
        {
            TMP_CharacterInfo charInfo = textComponent.textInfo.characterInfo[caretPosition];

            return charInfo.topRight;
        }
        else
            return new Vector2(0f, 0f);
    }
}