using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerDownHandler//, IPointerExitHandler
{
    static GameObject markedSlot;
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

    public virtual void OnDrop(PointerEventData data) //Called when putting something in the inventory slots.
    {
        if (ContainedItem != null)
            return;
        
        if (CompareTag("Slot"))
        {
            DragHandler.itemBeingDragged.transform.SetParent(transform);

            if (DragHandler.itemBeingDragged.GetComponent<Item>().IsEquipped)
            {
                DragHandler.itemBeingDragged.GetComponent<Item>().Unequip(this);
            }
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (markedSlot != null)
        {
            markedSlot.GetComponent<Outline>().enabled = false;
        }

        markedSlot = gameObject;
        GetComponent<Outline>().enabled = true;
        if (ContainedItem != null)
        {
            descriptionText.text = ContainedItem.GetComponent<Item>().GetComponentDescriptions();
        }
        else
        {
            descriptionText.text = string.Empty;
        }
    }

    public void Deselect()
    {
        GetComponent<Outline>().enabled = false;
        descriptionText.text = string.Empty;
        markedSlot = null;
    }
}

