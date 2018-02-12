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
            return (slots[Item.EType.TwoHand.ToSlotType()].ContainedItem != null &&
                slots[Item.EType.TwoHand.ToSlotType()].ContainedItem.Type == Item.EType.TwoHand);
        }
    }

    public bool OffHandEquipped
    {
        get
        {
            return (slots[Item.EType.OffHand.ToSlotType()].ContainedItem != null);
        }
    }
}
