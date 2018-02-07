using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public GameObject ContainedItem
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }
    
    public void AddItem(GameObject item)
    {
        if (ContainedItem != null)
            return;

        item.transform.SetParent(transform, false);
    }

    public void OnDrop(PointerEventData data)
    {
        if (ContainedItem != null)
        {
            Debug.Log(ContainedItem.name);
            Debug.Log(ContainedItem.transform.parent.name);
            return;
        }
        
        Item.EType type = DragHandler.itemBeingDragged.GetComponent<Item>().Type;

        if (CompareTag("Slot") || CompareTag(type.ToString() + "Slot"))    //TODO fix so it works with our types
        {
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            //ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
}
