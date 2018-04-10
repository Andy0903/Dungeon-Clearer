using System;
using UnityEngine;

[Serializable]
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

    void Item_OnEquip(EquipmentManager equipmentManager)
    {
        equipmentManager.Player.Stats[type] += value;
    }

    void Item_OnUnequip(EquipmentManager equipmentManager)
    {
        equipmentManager.Player.Stats[type] -= value;
    }
}
