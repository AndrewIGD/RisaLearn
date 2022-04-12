using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Risa;
using TMPro;
using System;

public class BasicRisa : MonoBehaviour
{
    protected virtual void ResetEnvironment() { }

    public virtual void SetupForDebug()
    {
        vm = new VM();

        vm.GetIO().RedirectIn((mode) =>
        {
            try
            {
                switch (mode)
                {
                    case IO.InputMode.CHAR:
                        return input[_dataInIndex++].ToString();

                    case IO.InputMode.WORD:
                        string word = "";

                        while (_dataInIndex < input.Length)
                            if (input[++_dataInIndex - 1] != ' ' && input[_dataInIndex - 1] != '\n')
                                word += input[_dataInIndex - 1];
                            else break;

                        return word;

                    case IO.InputMode.LINE:
                        string line = "";

                        while (_dataInIndex < input.Length)
                            if (input[_dataInIndex++] != '\n')
                                line += input[_dataInIndex-1];
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
        });
        vm.GetIO().RedirectOut((data) => console.text += data);
        vm.GetIO().RedirectErr((data) => LogError(data));

        vm.LoadAllLibraries();

        vm.LoadGlobalNative("Time", GetGameTime);

        if(OnDebugInit != null)
            OnDebugInit.Invoke(vm);
    }

    private Value GetGameTime(VM vm, Args args)
    {
        return vm.CreateFloat(Time.realtimeSinceStartupAsDouble);
    }

    public delegate void RisaFunction(VM vm);

    public event RisaFunction OnDebugInit;

    public void LogError(string data)
    {
        Debug.Log(data);

        console.text += "<color=red>" + data + "</color>";
    }

    public virtual void SetupForEvaluation(Risa.IO.InHandler InFn, Risa.IO.OutHandler OutFn, Risa.IO.OutHandler ErrFn)
    {
        vm = new VM();

        vm.GetIO().RedirectIn(InFn);
        vm.GetIO().RedirectOut(OutFn);
        vm.GetIO().RedirectErr(ErrFn);

        vm.LoadAllLibraries();

        vm.LoadGlobalNative("Time", GetGameTime);

    }

    public void Run()
    {
        if (Evaluating)
            return;

        if(Running)
        {
            Running = false;

            return;
        }    

        SetupForDebug();

        if (pageSelector.Script.Contains("read."))
            AskForInput();
        else
            ExecuteScript();
    }

    private void AskForInput()
    {
        inputForm.SetActive(true);
    }

    string input = "";
    int _dataInIndex = 0;
    public void GetInput()
    {
        input = dataInputField.text;

        CloseInput();

        _dataInIndex = 0;

        ExecuteScript();
    }

    public void CloseInput()
    {
        dataInputField.text = "";

        inputForm.SetActive(false);
    }

    public virtual void ExecuteScript(bool evaluation = false)
    {
        console.text = "";

        vm.Load(pageSelector.Script);

        if (evaluation)
            Evaluating = true;
        else Running = true;
    }

    float clock;
    bool running = false;
    public bool Running
    {
        get
        {
            return running;
        }
        set
        {
            running = value;

            if (running)
            {
                compileButtonText.text = "Halt";

                if (ScriptActivityStatus.instance != null)
                    ScriptActivityStatus.instance.Activate();
            }
            else
            {
                compileButtonText.text = "Run";

                ResetEnvironment();

                if (ScriptActivityStatus.instance != null)
                    ScriptActivityStatus.instance.Deactivate();
            }
        }
    }

    public bool Evaluating
    {
        get
        {
            return evaluating;
        }
        set
        {
            evaluating = value;

            if (evaluating)
            {
                evaluateButtonText.text = "Halt";
            }
            else
            {
                evaluateButtonText.text = "Evaluate";
            }
        }
    }
    bool evaluating = false;
    protected virtual void Update()
    {
        if (!Running && !Evaluating)
            return;

        try
        {
            clock += Time.deltaTime * maxInstructionsPerSecond;

            if (vm.Run((uint)Mathf.Ceil(clock)))
            {
                Running = false;
                Evaluating = false;
            }

            clock %= 1;
        }
        catch
        {
            Running = false;
            Evaluating = false;
        }
    }

    protected VM vm;
    [SerializeField] TMP_InputField console;
    [SerializeField] PageSelector pageSelector;
    [SerializeField] int maxInstructionsPerSecond;
    [SerializeField] Evaluator evaluator;
    [SerializeField] GameObject inputForm;
    [SerializeField] TMP_InputField dataInputField;
    [SerializeField] TextMeshProUGUI compileButtonText;
    [SerializeField] TextMeshProUGUI evaluateButtonText;
}
