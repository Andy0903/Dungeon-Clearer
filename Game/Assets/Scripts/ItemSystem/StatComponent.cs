using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStats
{
    Strength,
    Vitality,
}

public class StatComponent : IItemComponent
{
    int value;

    public StatComponent(EStats type, int value) : base()
    {
        this.value = value;
    }

    public void BindToItem(Item item)
    {
        item.OnEquip += Item_OnEquip;
    }

    void Item_OnEquip(Equipment inventory)
    {
        //Do some stuff when equipped
    }

    void Item_OnUnequip(Equipment invetory)
    {
        //Do some stuff when unequipped
    }
}
