using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    [HideInInspector]
    public RisaTank target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RisaTank risaTank = collision.GetComponent<RisaTank>();

        if (risaTank != null)
            target = risaTank;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RisaTank risaTank = collision.GetComponent<RisaTank>();

        if (risaTank == target)
            target = null;
    }
}
