using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = obj.position + (Vector3)offset;
    }

    [SerializeField] Transform obj;
    [SerializeField] Vector2 offset;
}
