using Risa;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Evaluator : MonoBehaviour
{
    bool _evaluating = false;
    bool Evaluating
    {
        get
        {
            return _evaluating;
        }
        set
        {
            risaVM.Evaluating = _evaluating = value;

            if (_evaluating == false)
                progressBar.localScale = new Vector2(0, 1);
        }
    }
    int _dataInIndex = 0;
    int _evaluatorDataIndex = 0;
    List<bool> _evaluatorResults;
    public void Evaluate()
    {
        if (risaVM.Running)
            return;

        if (Evaluating)
        {
            Evaluating = false;

            return;
        }

        _data = "";
        _dataInIndex = 0;
        _error = false;
        _evaluatorResults = new List<bool>();
        _evaluatorDataIndex = 0;
        evaluationResult.text = "";
        Evaluating = true;

        risaVM.SetupForEvaluation(SendData, ObtainData, ObtainError);

        risaVM.ExecuteScript();
    }

    string SendData(IO.InputMode mode)
    {
        string dataIn = evaluationData[_evaluatorDataIndex].DataIn;

        try
        {
            switch (mode)
            {
                case IO.InputMode.CHAR:
                    return dataIn[_dataInIndex++].ToString();

                case IO.InputMode.WORD:
                    string word = "";

                    while (_dataInIndex < dataIn.Length)
                        if (dataIn[++_dataInIndex-1] != ' ' && dataIn[_dataInIndex-1] != '\n')
                            word += dataIn[_dataInIndex-1];
                        else break;

                    return word;

                case IO.InputMode.LINE:
                    string line = "";

                    while (_dataInIndex < dataIn.Length)
                        if (dataIn[_dataInIndex++] != '\n')
                            line += dataIn[_dataInIndex - 1];
                        else break;
                    return line;

                default:
                    return "";
            }
        }
        catch
        {
            return "";
        }
    }

    string _data = "";
    bool _error = false;
    void ObtainData(string data)
    {
        _data += data;
    }

    void ObtainError(string data)
    {
        risaVM.LogError(data);
        _error = true;
    }

    private void LateUpdate()
    {
        if(Evaluating)
        {
            if(_error)
            {
                Evaluating = false;
            }

            if(risaVM.Evaluating == false)
            {
                _evaluatorResults.Add(_data.Trim() == evaluationData[_evaluatorDataIndex++].ExpectedOut.Trim());
                _data = "";
                _dataInIndex = 0;

                progressBar.localScale = new Vector2((float)_evaluatorDataIndex / evaluationData.Length, 1);

                if(_evaluatorDataIndex >= evaluationData.Length)
                {
                    Evaluating = false;

                    int count = 0;
                    for (int i = 0; i < _evaluatorResults.Count; i++)
                    {
                        if (_evaluatorResults[i])
                            count++;
                    }

                    if (count == evaluationData.Length)
                    {
                        GameObject.Find("WinCanvas").GetComponent<WinCanvas>().Win();
                    }
                    else evaluationResult.text = count + "/" + evaluationData.Length + " tests successful.";
                }
                else risaVM.ExecuteScript(true);
            }    
        }
    }

    [SerializeField] Animator levelAnimator;
    [SerializeField] BasicRisa risaVM;
    [SerializeField] RectTransform progressBar;
    [SerializeField] EvaluationData[] evaluationData;
    [SerializeField] TextMeshProUGUI evaluationResult;
}

[Serializable]
public class EvaluationData
{
    [TextArea] public string DataIn;
    [TextArea] public string ExpectedOut;
}
