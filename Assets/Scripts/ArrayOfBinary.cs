using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayOfBinary : MonoBehaviour
{
    private const float TimeBetweenEnables = 0.1f;

    private void Start()
    {
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        List<Transform> children = new List<Transform>();

        foreach(Transform child in transform)
        {
            children.Add(child);
            child.gameObject.SetActive(false);
        }

        children.Sort((a, b) => b.position.y.CompareTo(a.position.y));

        for (int i=0;i<children.Count;i++)
        {
            yield return new WaitForSeconds(TimeBetweenEnables);

            children[i].gameObject.SetActive(true);
        }
    }
}
