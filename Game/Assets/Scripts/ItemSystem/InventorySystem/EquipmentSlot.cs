using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    EquipmentManager manager;

    public void OnEnable()
    {
        manager = GameObject.FindObjectOfType<EquipmentManager>();
    }

    public override void OnDrop(PointerEventData data) //called when putting something in the equipment slots.
    {
        if (ContainedItem != null)
            return;

        Item draggedItem = DragHandler.ItemBeingDragged.GetComponent<Item>();

        bool willEquip = true;
        if (CompareTag(draggedItem.Type.ToSlotType()))
        {
            if (draggedItem.Type == Item.EType.OffHand && manager.TwoHandEquipped)
            {
                willEquip = false;
            }
            else if (draggedItem.Type == Item.EType.TwoHand && manager.OffHandEquipped)
            {
                willEquip = false;
            }

            if (willEquip)
            {
                AddItem(DragHandler.ItemBeingDragged);
            }
        }
    }

    public override void RemoveItem()
    {
        ContainedItem.Unequip(this);
        base.RemoveItem();
    }

    public override void AddItem(GameObject item)
    {
        base.AddItem(item);
        ContainedItem.Equip(this);
    }
}
