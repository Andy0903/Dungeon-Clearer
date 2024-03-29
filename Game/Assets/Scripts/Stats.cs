﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public enum EType
    {
        Damage,
        Health,
        ElectricResistance,
        WaterResistance,
        FireResistance
    }

    Dictionary<EType, int> stats;

    public Stats()
    {
        stats = new Dictionary<EType, int>();
    }

    public int GetResistance(Health.EAttackType type)
    {
        //This will look really ugly with more resistances, should probably rethink the solution
        switch (type)
        {
            case Health.EAttackType.Electric:
                return this[EType.ElectricResistance];
            case Health.EAttackType.Water:
                return this[EType.WaterResistance];
            case Health.EAttackType.Fire:
                return this[EType.FireResistance];
            default:
                return 0;
        }
    }

    public int this[EType stat]
    {
        get
        {
            int val;
            stats.TryGetValue(stat, out val);
            return val;
        }
        set
        {
            stats[stat] = value;
        }
    }
}
