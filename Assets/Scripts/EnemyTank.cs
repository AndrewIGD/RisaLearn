using Risa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Tank
{
    [SerializeField] Range range;
    [SerializeField] Transform[] waypoints;

    int _waypointIndex = 0;

    protected override void Update()
    {
        base.Update();
        
        if(range.target != null)
        {
            if (Physics2D.Linecast(transform.position, range.target.transform.position, LayerMask.GetMask("Wall")).collider == null)
            {
                Vector2 diff = range.target.transform.position - transform.position;
                diff.Normalize();

                float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

                if (Mathf.Abs(Mathf.DeltaAngle(angle, gunPivot.eulerAngles.z)) < 5)
                    Shoot();
                else RotateBarrelTowards(angle, 1);
            }
        }

        if (waypoints.Length == 0)
            return;

        var waypointDiff = waypoints[_waypointIndex].position - transform.position;
        waypointDiff.Normalize();

        float waypointRot = Mathf.Atan2(waypointDiff.y, waypointDiff.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(Mathf.DeltaAngle(waypointRot, transform.eulerAngles.z)) > 3)
            RotateTowards(waypointRot, 1f);
        else
        {
            transform.eulerAngles = new Vector3(0, 0, waypointRot);

            Move(0.5f);

            if(Vector2.Distance(transform.position, waypoints[_waypointIndex].position) < 0.5f)
            {
                _waypointIndex++;

                if (_waypointIndex == waypoints.Length)
                    _waypointIndex = 0;
            }
        }
    }
}
