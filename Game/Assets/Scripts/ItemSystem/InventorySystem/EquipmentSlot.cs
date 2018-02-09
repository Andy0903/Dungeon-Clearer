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

    public override void OnDrop(PointerEventData data)
    {
        if (ContainedItem != null)
            return;

        Item.EType Draggedtype = DragHandler.itemBeingDragged.GetComponent<Item>().Type;

        bool willEquip = true;
        if (CompareTag(Draggedtype.ToSlotType()))
        {
            if (Draggedtype == Item.EType.OffHand && TwoHandEquipped)
            {
                willEquip = false;
            }
            else if (Draggedtype == Item.EType.TwoHand && OffHandEquipped)
            {
                willEquip = false;
            }

            if (willEquip)
            {
                DragHandler.itemBeingDragged.transform.SetParent(transform);
            }
        }
    }
}
