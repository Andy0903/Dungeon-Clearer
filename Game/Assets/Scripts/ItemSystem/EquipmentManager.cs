using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    Dictionary<string, EquipmentSlot> slots = new Dictionary<string, EquipmentSlot>();

    public void Awake()
    {
        EquipmentSlot[] equipmentSlots = FindObjectsOfType<EquipmentSlot>();

        foreach (EquipmentSlot eq in equipmentSlots)
        {
            slots.Add(eq.tag, eq);
        }
    }

    public bool TwoHandEquipped
    {
        get
        {

            EquipmentSlot slot = slots["MainHandSlot"];
            Item c = slot.ContainedItem;

            if (slots[Item.EType.TwoHand.ToSlotType()].ContainedItem != null)
            {
                if (slots[Item.EType.TwoHand.ToSlotType()].ContainedItem.Type != Item.EType.TwoHand)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }

    public bool OffHandEquipped
    {
        get
        {
            EquipmentSlot slot = slots["OffHandSlot"];
            Item c = slot.ContainedItem;

            return (slots[Item.EType.OffHand.ToSlotType()].ContainedItem != null);
        }
    }
}
