using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    bool TwoHandEquipped
    {
        get
        {
            if (GameObject.FindGameObjectWithTag("MainHandSlot").transform.childCount > 0)
            {
                return (GameObject.FindGameObjectWithTag("MainHandSlot").transform.GetChild(0).GetComponent<Item>().Type == Item.EType.TwoHand);
            }

            return false;
        }
    }

    bool OffHandEquipped
    {
        get
        {
            return (GameObject.FindGameObjectWithTag("OffHandSlot").transform.childCount > 0);
        }
    }

    public override void OnDrop(PointerEventData data) //called when putting something in the equipment slots.
    {
        if (ContainedItem != null)
            return;

        Item draggedItem = DragHandler.ItemBeingDragged.GetComponent<Item>();

        bool willEquip = true;
        if (CompareTag(draggedItem.Type.ToSlotType()))
        {
            if (draggedItem.Type == Item.EType.OffHand && TwoHandEquipped)
            {
                willEquip = false;
            }
            else if (draggedItem.Type == Item.EType.TwoHand && OffHandEquipped)
            {
                willEquip = false;
            }

            if (willEquip)
            {
                if (!draggedItem.IsEquipped)
                {
                    draggedItem.Equip(this);
                }

                DragHandler.ItemBeingDragged.transform.SetParent(transform);
            }
        }
    }

    void EquipItem(Item item)
    {
        item.Equip(this);
    }

    void UnequipItem(Item item)
    {
        item.Unequip(this);
    }
}
