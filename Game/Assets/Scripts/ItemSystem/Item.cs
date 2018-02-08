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

    public string GetComponentDescriptions()
    {
        string description = "<color=orange>" + Type.ToString() + "</color>" + "\n";

        foreach (IItemComponent component in components)
        {
            description += component.Description + "\n";
        }

        return description;
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
