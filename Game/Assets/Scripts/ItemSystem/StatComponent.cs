﻿using System;
using UnityEngine;

public class StatComponent : IItemComponent
{
    int value;
    Stats.EType type;

    public StatComponent(Stats.EType type, int value) : base()
    {
        this.type = type;
        this.value = value;
    }

    public string Description
    {
        get { return "Grants " + value + " " + type; }
    }

    public IItemComponent BindToItem(Item item)
    {
        item.OnEquip += Item_OnEquip;
        item.OnUnequip += Item_OnUnequip;
        return this;
    }

    void Item_OnEquip(InventorySlot inventory)
    {
        Debug.Log("Equipped " + Description);
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        p.Stats[type] += value;
    }

    void Item_OnUnequip(InventorySlot invetory)
    {
        Debug.Log("Unequipped " + Description);
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        p.Stats[type] -= value;
    }
}
