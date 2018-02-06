using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats
{
    Strength,
    Vitality,
}

public class StatComponent : ItemComponent
{
    int value;

    public StatComponent(Stats type, int value) : base()
    {
        this.value = value;
    }

    public override void BindToItem(Item item)
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
