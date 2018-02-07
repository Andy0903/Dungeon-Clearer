﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;


/*
    Equip
    Unequp
    hit
    kill
    Die
    takedmg 
 */

public class Item : MonoBehaviour
{
    public enum EType
    {
        Head,
        Shoulder,
        Chest,
        Back,
        Wrist,
        Hands,
        Waist,
        Legs,
        Feet,
        Neck,
        Trinket,
        Finger,
        OneHand,
        TwoHand,
        OffHand,
    }

    public delegate void EquipEvent(Equipment inventory);
    public event EquipEvent OnEquip;
    public event EquipEvent OnUnequip;

    List<IItemComponent> components;
    public EType Type { get; set; }

    public void Awake()
    {
        components = new List<IItemComponent>();
    }

    public void AddComponent(IItemComponent component)
    {
        components.Add(component);
    }

    public void Equip(Equipment inventory)
    {
        OnEquip(inventory);
    }

    public void Unequip(Equipment inventory)
    {
        OnUnequip(inventory);
    }
}
