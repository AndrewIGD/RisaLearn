using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RisaTank>() != null)
            GameObject.Find("WinCanvas").GetComponent<WinCanvas>().Win();
    }
}
