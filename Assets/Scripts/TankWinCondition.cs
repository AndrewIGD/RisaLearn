using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWinCondition : MonoBehaviour
{
    void Update()
    {
        if(FindObjectsOfType<EnemyTank>().Length == 0)
            GameObject.Find("WinCanvas").GetComponent<WinCanvas>().Win();
    }
}
