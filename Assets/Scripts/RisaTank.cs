using Risa;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisaTank : Tank
{

    public Value RisaShoot(VM vm, Args args)
    {
        Shoot();

        return Value.NULL;
    }

    public Value RisaRotate(VM vm, Args args)
    {
        float dir = 0;

        Value v1 = args.Get(0);
        if (v1.IsFloat())
            dir = (float)v1.AsFloat();
        else if (v1.IsInt())
            dir = (int)v1.AsInt();
        else dir = 0;

        Rotate(dir);

        return Value.NULL;
    }

    public Value RisaMove(VM vm, Args args)
    {
        float dir = 0;

        Value v1 = args.Get(0);
        if (v1.IsFloat())
            dir = (float)v1.AsFloat();
        else if (v1.IsInt())
            dir = (int)v1.AsInt();
        else dir = 0;

        Move(dir);

        return Value.NULL;
    }

    public Value RisaRotateTowards(VM vm, Args args)
    {
        float angle = 0;

        Value v1 = args.Get(0);
        if (v1.IsFloat())
            angle = (float)v1.AsFloat();
        else if (v1.IsInt())
            angle = (int)v1.AsInt();
        else angle = 0;

        float amplifier = 0;

        Value v2 = args.Get(1);
        if (v2.IsFloat())
            amplifier = (float)v2.AsFloat();
        else if (v2.IsInt())
            amplifier = (int)v2.AsInt();
        else amplifier = 0;

        RotateTowards(angle, amplifier);

        return Value.NULL;
    }

    public Value RisaRotateBarrelTowards(VM vm, Args args)
    {
        float angle = 0;

        Value v1 = args.Get(0);
        if (v1.IsFloat())
            angle = (float)v1.AsFloat();
        else if (v1.IsInt())
            angle = (int)v1.AsInt();
        else angle = 0;

        float amplifier = 0;

        Value v2 = args.Get(1);
        if (v2.IsFloat())
            amplifier = (float)v2.AsFloat();
        else if (v2.IsInt())
            amplifier = (int)v2.AsInt();
        else amplifier = 0;

        RotateBarrelTowards(angle, amplifier);

        return Value.NULL;
    }

    public Value RisaRotateBarrel(VM vm, Args args)
    {
        float dir = 0;

        Value v1 = args.Get(0);
        if (v1.IsFloat())
            dir = (float)v1.AsFloat();
        else if (v1.IsInt())
            dir = (int)v1.AsInt();
        else dir = 0;

        RotateBarrel(dir);

        return Value.NULL;
    }

}
