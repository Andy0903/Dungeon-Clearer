using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerDownHandler//, IPointerExitHandler
{
    Text descriptionText;

    private void Awake()
    {
        descriptionText = GameObject.FindGameObjectWithTag("ItemDescriptionText").GetComponent<Text>();
    }

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
            return;
        }

        Item.EType type = DragHandler.itemBeingDragged.GetComponent<Item>().Type;

        if (CompareTag("Slot") || CompareTag(type.ToString() + "Slot"))    //TODO fix so it works with our types
        {
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (ContainedItem != null)
        {
            descriptionText.text = ContainedItem.GetComponent<Item>().GetComponentDescriptions();
        }
    }
}

