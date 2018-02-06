using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    PathFinder pf;
    [SerializeField]
    Transform target;

    List<Vector3> path;
    Vector3 pathTarget;

    private void Awake()
    {
        path = pf.FindPath(transform.position, target.position);
        pathTarget = path[0];
    }

    private void Update()
    {
        path = pf.FindPath(transform.position, target.position);
        const float speed = 3;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pathTarget, step);

        if (Vector3.Distance(transform.position, pathTarget) < 0.000001f)
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
