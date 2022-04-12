using Risa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisaPhysics : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BasicRisa>().OnDebugInit += AddMethods;
    }

    void AddMethods(VM vm)
    {
        vm.LoadGlobalNative("Linecast", Linecast);

        vm.LoadGlobalNative("Raycast", Raycast);
    }

    public Value Linecast(VM vm, Args args)
    {
        ValueObject v1 = args.Get(0).AsObject();
        Vector2 startPos = new Vector2((float)v1.Get(1).value.AsFloat(), (float)v1.Get(0).value.AsFloat());

        ValueObject v2 = args.Get(1).AsObject();
        Vector2 endPos = new Vector2((float)v2.Get(1).value.AsFloat(), (float)v2.Get(0).value.AsFloat());

        return vm.CreateBool(Physics2D.Linecast(startPos, endPos, LayerMask.GetMask("Wall")).collider != null);
    }

    public Value Raycast(VM vm, Args args)
    {
        ValueObject v1 = args.Get(0).AsObject();
        Vector2 pos = new Vector2((float)v1.Get(1).value.AsFloat(), (float)v1.Get(0).value.AsFloat());

        ValueObject v2 = args.Get(1).AsObject();
        Vector2 dir = new Vector2((float)v2.Get(1).value.AsFloat(), (float)v2.Get(0).value.AsFloat());

        float dist = 0;

        Value v3 = args.Get(0);
        if (v3.IsFloat())
            dist = (float)v3.AsFloat();
        else if (v3.IsInt())
            dist = (int)v3.AsInt();
        else dist = 0;

        return vm.CreateBool(Physics2D.Raycast(pos, dir, dist, LayerMask.GetMask("Wall")).collider != null);
    }
}
