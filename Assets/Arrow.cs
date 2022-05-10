using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<Animator>().Play("in", 0, 0);
    }
}
