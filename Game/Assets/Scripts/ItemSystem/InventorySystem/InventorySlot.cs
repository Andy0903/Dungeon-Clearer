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

    public Item ContainedItem { get; private set; }

    public virtual void AddItem(GameObject item)
    {
        if (ContainedItem != null)
            return;

        item.transform.SetParent(transform, false);
        ContainedItem = item.GetComponent<Item>();
    }

    public virtual void OnDrop(PointerEventData data) //Called when putting something in the inventory slots.
    {
        if (CompareTag("Slot"))
        {
            AddItem(DragHandler.ItemBeingDragged);
        }
    }

    public virtual void RemoveItem()
    {
        Debug.Log("!!!");
        ContainedItem = null;
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

