using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //[SerializeField]
    //int _uniqueID;

    public GameObject ContainedItem
    {
        get
        {
            if (gameObject.transform.childCount > 0)
                return transform.GetChild(0).gameObject;

            return null;
        }
    }

    //public int UniqueID
    //{
    //    get { return _uniqueID; }
    //}

    public void OnPointerEnter(PointerEventData data)
    {
        if (ContainedItem == null)
            return;

        //Används för att visa tooltip, får tänka om här.
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (ContainedItem == null)
            return;

        //Används för att closea tooltip, tänk om.
    }

    public void AddItem(GameObject item)
    {
        if (ContainedItem != null)
            return;

        item.transform.SetParent(gameObject.transform, false);
    }

    public void OnDrop(PointerEventData data)
    {
        if (ContainedItem != null)
            return;
        
        Item.EType type = DragHandler.itemBeingDragged.GetComponent<Item>().Type;

        if (gameObject.CompareTag("Slot") || gameObject.CompareTag(type.ToString() + "Slot"))    //TODO fix so it works with our types
        {
            DragHandler.itemBeingDragged.transform.SetParent(gameObject.transform);
            //ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
}
