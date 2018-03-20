using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    PathFinder pf;

    Transform target;

    Vector3 oldTargetPos;

    List<Vector3> path;
    Vector3 pathTarget;

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            try
            {
                path = pf.FindPath(transform.position, target.position);
                pathTarget = path[0];
            }
            catch (NullReferenceException)
            {
                target = null;
            }
            return;
        }

        const float distanceTreashold = 0.000001f;


        ///Något fel i movement logiken.

        if (Vector3.Distance(oldTargetPos, target.position) > distanceTreashold)
        {
            path = pf.FindPath(transform.position, target.position);
            oldTargetPos = target.position;
        }
        const float speed = 3;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pathTarget, step);

        if (Vector3.Distance(transform.position, pathTarget) < distanceTreashold)
        {
            int index = path.IndexOf(pathTarget);
            if (index < path.Count - 1)
            {
                index++;
            }

            pathTarget = path[index];
        }
    }
}
