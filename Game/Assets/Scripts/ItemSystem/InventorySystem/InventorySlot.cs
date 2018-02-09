using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerDownHandler//, IPointerExitHandler
{
    static GameObject marketSlot;
    Text descriptionText;

    private bool TwoHandEquipped
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

    private bool OffHandEquipped
    {
        get
        {
            return (GameObject.FindGameObjectWithTag("OffHandSlot").transform.childCount > 0);
        }
    }

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

        if (CompareTag("Slot") || tag == (type.ToSlotType() + "Slot"))
        {
            if (type.ToFriendlyString() == "Off Hand" && !TwoHandEquipped)
            {
                DragHandler.itemBeingDragged.transform.SetParent(transform);
            }
            else if (type.ToFriendlyString() == "Two Hands" && !OffHandEquipped)
            {
                DragHandler.itemBeingDragged.transform.SetParent(transform);
            }
            else if (type.ToFriendlyString() != "Off Hand" && type.ToFriendlyString() != "Two Hands")
            {
                DragHandler.itemBeingDragged.transform.SetParent(transform);
            }
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (marketSlot != null)
        {
            marketSlot.GetComponent<Outline>().enabled = false;
        }

        marketSlot = gameObject;
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
        marketSlot = null;
    }
}

