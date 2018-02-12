using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    Dictionary<string, EquipmentSlot> slots = new Dictionary<string, EquipmentSlot>();

    public void Start()
    {
        EquipmentSlot[] equipmentSlots = FindObjectOfType<MenuManager>().transform.GetComponentsInChildren<EquipmentSlot>(true);

        foreach (EquipmentSlot eq in equipmentSlots)
        {
            slots.Add(eq.tag, eq);
        }
    }

    public bool TwoHandEquipped
    {
        get
        {
            if (slots[Item.EType.TwoHand.ToSlotType()].ContainedItem != null)
            {
                if (slots[Item.EType.TwoHand.ToSlotType()].ContainedItem.Type != Item.EType.TwoHand)
                {
                    return false;
                }
                return true;
            }
            return false;

            //    return (slots[Item.EType.TwoHand.ToSlotType()].ContainedItem != null &&
            //        slots[Item.EType.TwoHand.ToSlotType()].ContainedItem.Type == Item.EType.TwoHand);
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
