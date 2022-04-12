using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasActivity : MonoBehaviour
{
    private CanvasActivity()
    {
        instance = this;
    }

    public static CanvasActivity instance;
}
