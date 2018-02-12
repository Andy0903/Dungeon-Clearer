using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    Dictionary<string, EquipmentSlot> slots = new Dictionary<string, EquipmentSlot>();

    public Player Player { get; private set; }

    public void Start()
    {
        EquipmentSlot[] equipmentSlots = FindObjectOfType<MenuManager>().transform.GetComponentsInChildren<EquipmentSlot>(true);

        foreach (EquipmentSlot eq in equipmentSlots)
        {
            slots.Add(eq.tag, eq);
        }

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
