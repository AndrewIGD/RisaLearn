using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CaretPositionInputField : InputField
{
    public Vector2 GetLocalCaretPosition()
    {
        if (isFocused)
        {
            TextGenerator gen = m_TextComponent.cachedTextGenerator;
            UICharInfo charInfo = gen.characters[caretPosition];
            float x = (charInfo.cursorPos.x + charInfo.charWidth) / m_TextComponent.pixelsPerUnit;
            float y = (charInfo.cursorPos.y) / m_TextComponent.pixelsPerUnit;

            return new Vector2(x, y);
        }
        else
            return new Vector2(0f, 0f);
    }

    public int CaretSelectPos
    {
        get
        {
            return caretSelectPositionInternal;
        }
        set
        {
            caretSelectPositionInternal = value;
        }
    }
}