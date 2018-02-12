using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public enum EType
    {
        Strength,
        Vitality,
    }

    Dictionary<EType, int> stats;

    Stats()
    {
        stats = new Dictionary<EType, int>();
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
