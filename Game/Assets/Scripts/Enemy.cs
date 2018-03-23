﻿using System;
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

    Vector3 spawnPos;

    float attackRange = 2.0f;
    float attackIntervall = 0.4f;
    float timeSinceLastAttack = 0;
    int attackDamage = 5;
    

    private void InitializeNewPathTarget()
    {
        oldTargetPos = target.position;
        path = pf.FindPath(transform.position, target.position);
        pathTarget = path[0];
    }

    private void Start()
    {
        spawnPos = transform.position;
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            try
            {
                InitializeNewPathTarget();
            }
            catch (NullReferenceException)
            {

                target = null;
            }
            return;
        }

        if(Vector3.Distance(transform.position, target.position) < attackRange)
        {
            TryDealDamage();
        }

        const float distanceTreashold = 0.000001f;
        if (Vector3.Distance(oldTargetPos, target.position) > distanceTreashold)
        {
            try
            {
                InitializeNewPathTarget();
            }
            catch (NullReferenceException)
            {
                if (transform.parent.tag !=  "BackgroundLayer")
                {
                    transform.position = spawnPos;
                }
            }
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

    private bool TryDealDamage()
    {
        Health eHP = target.GetComponent<Health>();

        if (timeSinceLastAttack > attackIntervall && !eHP.isInvincible)
        {
            eHP.DealDamage(attackDamage);
            timeSinceLastAttack = 0;
            return true;
        }
        return false;
    }
}
