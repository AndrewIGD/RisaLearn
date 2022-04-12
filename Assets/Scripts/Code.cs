using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Code : MonoBehaviour
{
    [SerializeField] string[] keywords;
    [SerializeField] string[] methods;
    [SerializeField] string[] classes;
    [SerializeField] char[] ignoredSymbols;
    [SerializeField] Color32 keywordColor;
    [SerializeField] Color32 methodColor;
    [SerializeField] Color32 classColor;
    [SerializeField] Color32 doubleQuoteStringColor;
    [SerializeField] Color32 numberColor;

    private void Start()
    {
        ResetLines();

        List<char> symbolList = ignoredSymbols.ToList();
        symbolList.Add('\n');
        symbolList.Add('\t');
        ignoredSymbols = symbolList.ToArray();

        _highlightSyntax = PlayerPrefs.GetInt("Syntax Highlighting", 1) == 1;
    }

    void ResetLines()
    {
        codeLineCount.text = "1\n";

        prevLineCount = 0;
    }

    public void Highlight()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.V))
            FullHighlight();
        else
            HighlightLine();
    }

    public void HighlightLine()
    {
        int i = inputField.caretPosition - 1;

        if (inputField.text.Length == 0)
            return;

        while (inputField.text[i] != '\n' && i > 0)
            i--;

        int j = i;

        string text = inputField.text[j++].ToString();

        if (j < inputField.text.Length)
            while (j < inputField.text.Length)
            {
                if (inputField.text[j] != '\n')
                    text += inputField.text[j++].ToString();
                else break;
            }

        text = HighlightString(text);

        string codeText = inputField.text;
        codeText = codeText.Remove(i, j - i);
        codeText = codeText.Insert(i, text);

        //codeTextBox.text = codeText;
    }

    bool _initializationCalled = false;
    bool _incrementCaretPosition = false;
    int _incrementTabCount = 0;
    public void InvokeThreadInitialization()
    {
        if (_initializationCalled == false)
            _initializationCalled = true;
        else return;

        try
        {
            if ((Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace)) == false)
                switch (inputField.text[inputField.caretPosition - 1])
                {
                    case '(':
                        inputField.text = inputField.text.Insert(inputField.caretPosition, ")");
                        break;
                    case '[':
                        inputField.text = inputField.text.Insert(inputField.caretPosition, "]");
                        break;
                    case '{':
                        inputField.text = inputField.text.Insert(inputField.caretPosition, "\n\t\n}");
                        _incrementCaretPosition = true;
                        break;
                    case '\n':
                        string text = inputField.text;

                        int tabCount = 0;
                        for (int i = 0; i < inputField.caretPosition - 1; i++)
                        {
                            if (text[i] == '\n')
                                tabCount = 0;
                            else if (text[i] == '\t')
                                tabCount++;
                        }

                        string tabs = "";

                        for (int i = 0; i < tabCount; i++)
                            tabs += "\t";

                        inputField.text = inputField.text.Insert(inputField.caretPosition, tabs);

                        _incrementTabCount = tabCount;

                        break;
                }
        }

        catch { }

        highlightTextOutput.text = "";

        CancelInvoke("InitializeThread");
        Invoke("InitializeThread", highlightDelay);
    }

    bool _threadRunning;
    string _threadReturnedString;
    string _threadStringParameter;
    Thread _thread;

    bool _threadFinished;
    bool _highlightSyntax;
    void InitializeThread()
    {
        if (_highlightSyntax == false)
        {
            //highlightTextOutput.text = inputField.text;

            return;
        }

        if (_thread != null)
            _thread.Join();

        _threadStringParameter = inputField.text;

        _thread = new Thread(ThreadedHighlight);
        _thread.Start();
    }

    void ThreadedHighlight()
    {
        _threadRunning = true;

        _threadReturnedString = HighlightString(_threadStringParameter);
        _threadFinished = true;

        _threadRunning = false;
    }

    private void Update()
    {
        _initializationCalled = false;

        if (_incrementTabCount != 0)
        {
            inputField.caretPosition += _incrementTabCount;
            _incrementTabCount = 0;
        }

        if(_incrementCaretPosition)
        {
            inputField.caretPosition += 2;

            _incrementCaretPosition = false;
        }

        if(_threadFinished)
        {
            highlightTextOutput.text = _threadReturnedString;

            _threadFinished = false;
        }
    }

    private void OnDisable()
    {
        if(_threadRunning)
        {
            _threadRunning = false;

            _thread.Join();
        }
    }

    public void FullHighlight()
    {
        string text = inputField.text;

        text = HighlightString(text);

        //codeTextBox.text = text;
    }

    private string HighlightString(string text)
    {
        bool doubleQuote = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\"')
            {
                if (doubleQuote == false)
                {
                    string color = "<color=#" + ColorUtility.ToHtmlStringRGBA(doubleQuoteStringColor) + ">";

                    text = text.Insert(i, color);

                    i += color.Length + 1;

                    doubleQuote = true;
                }
                else
                {
                    string color = "</color>";

                    text = text.Insert(i + 1, color);

                    i += color.Length + 1;

                    doubleQuote = false;
                }
            }

            if (i == text.Length)
                break;

            if (doubleQuote == false)
            {
                int numLength = 0;

                char[] digitList = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', 'f' };

                while (digitList.Contains(text[i + numLength]))
                {
                    numLength++;

                    if (i + numLength == text.Length)
                        break;
                }

                bool leftCheck = false, rightCheck = false;

                if (i == 0)
                    leftCheck = true;
                else if (ignoredSymbols.Contains(text[i - 1]))
                    leftCheck = true;

                if (i + numLength == text.Length)
                    rightCheck = true;
                else if (ignoredSymbols.Contains(text[i + numLength]))
                    rightCheck = true;

                if (leftCheck && rightCheck)
                {
                    string colorEnd = "</color>";
                    string colorStart = "<color=#" + ColorUtility.ToHtmlStringRGBA(numberColor) + ">";

                    text = text.Insert(i + numLength, colorEnd);
                    text = text.Insert(i, colorStart);

                    i += colorStart.Length + colorEnd.Length + numLength;

                }

                Highlight(ref text, ref i, keywords, keywordColor);
                Highlight(ref text, ref i, methods, methodColor);
                Highlight(ref text, ref i, classes, classColor);
            }
        }

        if(doubleQuote)
            text+="</color>";

        return text;
    }

    private void Highlight(ref string text, ref int i, string[] keywords, Color32 keywordColor)
    {
        for (int j = 0; j < keywords.Length; j++)
        {
            string word = "";

            for (int k = i; k < i + keywords[j].Length && k < text.Length; k++)
                word += text[k];

            if (word.Equals(keywords[j]))
            {
                bool leftCheck = false, rightCheck = false;

                if (i == 0)
                    leftCheck = true;
                else if (ignoredSymbols.Contains(text[i - 1]))
                    leftCheck = true;

                if (i + keywords[j].Length == text.Length)
                    rightCheck = true;
                else if (ignoredSymbols.Contains(text[i + keywords[j].Length]))
                    rightCheck = true;

                if (leftCheck && rightCheck)
                {
                    string colorEnd = "</color>";
                    string colorStart = "<color=#" + ColorUtility.ToHtmlStringRGBA(keywordColor) + ">";

                    text = text.Insert(i + keywords[j].Length, colorEnd);
                    text = text.Insert(i, colorStart);

                    i += colorStart.Length + colorEnd.Length + keywords[j].Length;

                    continue;
                }
            }
        }
    }

    int length;
    void FixedUpdate()
    {
        int lineCount = inputField.text.Split('\n').Length;

        if (lineCount == 0)
            lineCount = 1;

        if (lineCount > prevLineCount && lineCount > 1)
        {
            for (int i = prevLineCount + 1; i <= lineCount; i++)
            {
                codeLineCount.text += i + "\n";
            }
        }
        else if(lineCount < prevLineCount)
        {
            if (lineCount == 1)
                ResetLines();
            else
                for (int i = prevLineCount; i > lineCount; i--)
                {
                    codeLineCount.text = codeLineCount.text.Remove(codeLineCount.text.TrimEnd().LastIndexOf('\n')) + "\n";
                }
        }

        prevLineCount = lineCount;
    }


    #region Alternate syntax highlight
    private void LateUpdate()
    {
        return;

        length = inputField.text.Length;

        if (length != prevLength)
        {
            if (length > prevLength)
                AddText();
            else RemoveText();

            //LayoutRebuilder.ForceRebuildLayoutImmediate(lineParent);
            //inputRect.sizeDelta = new Vector2(lineParent.sizeDelta.x > minWidth ? lineParent.sizeDelta.x : minWidth + 25, lineParent.sizeDelta.y > minHeight ? lineParent.sizeDelta.y : minHeight);
        }

        prevLength = length;
    }

    void AddText()
    {
        int diff = length - prevLength;

        TextMeshProUGUI currentLine = null;

        string text = code = inputField.text;

        int line = 0, i;
        string lineText = "";
        int tabCount = 0;
        bool encounteredCharacter = false;

        for (i = 0; i < inputField.caretPosition - diff; i++)
        {
            if (text[i] == '\n')
            {
                line++;
                lineText = "";
                tabCount = 0;
                encounteredCharacter = false;
            }
            else
            {
                if (text[i] == '\t' && encounteredCharacter == false)
                    tabCount++;
                else encounteredCharacter = true;

                lineText += text[i];
            }
        }

        if (codeLines.Count == 0)
            codeLines.Add(Instantiate(linePrefab, lineParent).GetComponent<TextMeshProUGUI>());

        currentLine = codeLines[line];
        int startLine = line;

        List<TextMeshProUGUI> newLines = new List<TextMeshProUGUI>();

        for (i = inputField.caretPosition - diff; i < inputField.caretPosition; i++)
        {
            if (text[i] == '\n')
            {
                currentLine.text = HighlightString(lineText);

                currentLine = Instantiate(linePrefab, lineParent).GetComponent<TextMeshProUGUI>();
                currentLine.transform.SetSiblingIndex(++line);
                newLines.Add(currentLine);

                lineText = "";

                string tabs = "";

                for (int j = 0; j < tabCount; j++)
                    tabs += "\t";

                lineText += tabs;
                code = text = inputField.text = text.Insert(i + 1, tabs);
                i += tabs.Length;
                inputField.caretPosition += tabs.Length;
            }
            else
                lineText += text[i];
        }

        codeLines.InsertRange(startLine + 1, newLines);

        while(i < text.Length)
        {
            if (text[i] != '\n')
                lineText += text[i++];
            else break;
        }

        currentLine.text = HighlightString(lineText);
    }

    void RemoveText()
    {
        int diff = prevLength - length;

        int line = 0, i;
        string lineText = "";

        for (i = 0; i < inputField.caretPosition; i++)
        {
            if (code[i] == '\n')
            {
                line++;
                lineText = "";
            }
            else lineText += code[i];
        }

        int lastLineIndex = line;
        for (i = inputField.caretPosition; i < inputField.caretPosition + diff; i++)
            if (code[i] == '\n')
                lastLineIndex++;

        while (i < code.Length)
        {
            if (code[i] != '\n')
                lineText += code[i++];
            else break;
        }

        if (lastLineIndex != line)
        {
            for (int j = line + 1; j <= lastLineIndex; j++)
                Destroy(codeLines[j].gameObject);

            codeLines.RemoveRange(line + 1, lastLineIndex - line);
        }

        codeLines[line].text = HighlightString(lineText);

        code = inputField.text;
    }

    int prevLineCount = 0;
    int prevLength = 0;
    string code = "";

    #endregion

    List<TextMeshProUGUI> codeLines = new List<TextMeshProUGUI>();
    [SerializeField] TextMeshProUGUI codeLineCount;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] RectTransform inputRect;
    [SerializeField] GameObject linePrefab;
    [SerializeField] RectTransform lineParent;
    [SerializeField] float minWidth;
    [SerializeField] float minHeight;
    [SerializeField] float highlightDelay = 2f;
    [SerializeField] TextMeshProUGUI highlightTextOutput;
}
