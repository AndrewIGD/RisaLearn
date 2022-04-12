using Risa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisaTankEnemies : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BasicRisa>().OnDebugInit += AddEnemies;
    }

    void AddEnemies(VM vm)
    {
        ValueArray arr = vm.CreateArray();

        for (int i = 0; i < enemies.Length; i++)
        {
            ValueObject tank = vm.CreateObject();

            Value positionFn = vm.CreateNative(enemies[i].GetPosition);
            tank.Set("Position", positionFn);

            Value rotationFn = vm.CreateNative(enemies[i].GetRotation);
            tank.Set("Rotation", rotationFn);

            Value barrelRotationFn = vm.CreateNative(enemies[i].GetBarrelRotation);
            tank.Set("BarrelRotation", barrelRotationFn);

            Value isAliveFn = vm.CreateNative(enemies[i].IsAlive);
            tank.Set("IsAlive", isAliveFn);

            Value velocityFn = vm.CreateNative(enemies[i].GetVelocity);
            tank.Set("Velocity", velocityFn);

            arr.Add(tank.ToValue());
        }

        vm.LoadGlobal("enemies", arr.ToValue());
    }

    [SerializeField] EnemyTank[] enemies;
}
