using System;
using System.Collections.Generic;
using UnityEngine;


/*
    Equip
    Unequp
    hit
    kill
    Die
    takedmg 
 */

public class Item // :monobehaviour
{
    public enum Type
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

    public delegate void EquipEvent(Inventory inventory);
    public event EquipEvent OnEquip;
    public event EquipEvent OnUnequip;

    List<ItemComponent> components;
    Type type;

    public Item(Type type)
    {
        this.type = type;
        components = new List<ItemComponent>();
    }

    public void AddComponent(ItemComponent component)
    {
        components.Add(component);
    }

    public void Equip(Inventory inventory)
    {
        OnEquip(inventory);
    }

    public void Unequip(Inventory inventory)
    {
        OnUnequip(inventory);
    }
}
