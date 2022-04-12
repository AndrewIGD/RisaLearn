using Risa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisaTankWaypoints : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BasicRisa>().OnDebugInit += AddWaypoints;
    }

    void AddWaypoints(VM vm)
    {
        ValueArray arr = vm.CreateArray();
        
        for(int i=0;i<waypoints.Length;i++)
        {
            ValueObject vector = vm.CreateObject();
            vector.Set("x", vm.CreateFloat(waypoints[i].position.x));
            vector.Set("y", vm.CreateFloat(waypoints[i].position.y));

            arr.Add(vector.ToValue());
        }

        vm.LoadGlobal("waypoints", arr.ToValue());
    }

    [SerializeField] Transform[] waypoints;
}
