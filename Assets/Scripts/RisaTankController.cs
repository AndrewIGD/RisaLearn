using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Risa;
using System;

public class RisaTankController : BasicRisa
{
    private void Start()
    {
        codeButton = FindObjectOfType<CodeButton>();
    }

    public override void SetupForDebug()
    {
        base.SetupForDebug();

        SetupForTankUsage();
    }

    public override void ExecuteScript(bool evaluation = false)
    {
        base.ExecuteScript(evaluation);
    }

    private void SetupForTankUsage()
    {
        ValueObject risaTank = vm.CreateObject();

        Value shootFn = vm.CreateNative(tank.RisaShoot);
        risaTank.Set("Shoot", shootFn);

        Value rotateFn = vm.CreateNative(tank.RisaRotate);
        risaTank.Set("Rotate", rotateFn);

        Value rotateTowardsFn = vm.CreateNative(tank.RisaRotateTowards);
        risaTank.Set("RotateTowards", rotateTowardsFn);

        Value rotateBarrelTowardsFn = vm.CreateNative(tank.RisaRotateBarrelTowards);
        risaTank.Set("RotateBarrelTowards", rotateBarrelTowardsFn);

        Value rotateBarrelFn = vm.CreateNative(tank.RisaRotateBarrel);
        risaTank.Set("RotateBarrel", rotateBarrelFn);

        Value moveFn = vm.CreateNative(tank.RisaMove);
        risaTank.Set("Move", moveFn);

        Value positionFn = vm.CreateNative(tank.GetPosition);
        risaTank.Set("Position", positionFn);

        Value velocityFn = vm.CreateNative(tank.GetVelocity);
        risaTank.Set("Velocity", velocityFn);

        Value rotationFn = vm.CreateNative(tank.GetRotation);
        risaTank.Set("Rotation", rotationFn);

        Value barrelRotationFn = vm.CreateNative(tank.GetBarrelRotation);
        risaTank.Set("BarrelRotation", barrelRotationFn);

        Value isAliveFn = vm.CreateNative(tank.IsAlive);
        risaTank.Set("IsAlive", isAliveFn);

        vm.LoadGlobal("Tank", risaTank.ToValue());
    }

    protected override void Update()
    {
        if (CanvasActivity.instance.gameObject.activeInHierarchy == false)
        {
            base.Update();

            //Invoke Update
        }
    }

    public override void SetupForEvaluation(IO.InHandler InFn, IO.OutHandler OutFn, IO.OutHandler ErrFn)
    {
        base.SetupForEvaluation(InFn, OutFn, ErrFn);

        SetupForTankUsage();
    }

    protected override void ResetEnvironment()
    {
        TankReset.instance.ResetGame();
    }

    CodeButton codeButton;
    [SerializeField] RisaTank tank;
}
