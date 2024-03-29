﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    PathFinder pf;

    [SerializeField]
    Health.EAttackType attackType;

    public Health.EAttackType AttackType
    {
        get;
        set;
    }

    public TimeEffectFactory.DayPhase mainTime;

    Transform target;

    Vector3 oldTargetPos;

    List<Vector3> path;
    Vector3 pathTarget;

    Vector3 spawnPos;

    const float movementSpeed = 3;
    float attackRange = 1.1f;
    float attackIntervall = 1.5f;
    float timeSinceLastAttack = 0;
    int attackDamage = 3;

    private const float AttackDamageIncreaseConstant = 0.45f;

    private void InitializeNewPathTarget()
    {
        oldTargetPos = target.position;
        path = pf.FindPath(transform.position, target.position);
        pathTarget = path[0];
    }

    private void Start()
    {
        GameData ld = GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>().LoadedData;

        attackDamage += (int)(ld.DungeonsCleared * AttackDamageIncreaseConstant);
        attackRange *= transform.localScale.x;

        spawnPos = transform.position;
    }

    private void Update()
    {
        timeSinceLastAttack += UnityEngine.Time.deltaTime;

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

        if (Vector3.Distance(transform.position, target.position) < attackRange)
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
                if (transform.parent.tag != "BackgroundLayer")
                {
                    transform.position = spawnPos;
                }
            }
        }

        float step = movementSpeed * UnityEngine.Time.deltaTime;
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

    private void TryDealDamage()
    {
        Health eHP = target.GetComponent<Health>();

        //Only attacks when intervall is reached and player isn't invincible
        if (timeSinceLastAttack > attackIntervall && !eHP.isInvincible)
        {
            eHP.DealDamage(attackDamage, attackType);
            timeSinceLastAttack = 0;
            eHP.ActivateInvincibility();
        }
    }
}
