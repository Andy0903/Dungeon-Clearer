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
        
        if (type.ToString() == "TwoHand" && CompareTag("MainHandSlot"))
        {
            //Set item in MainHandSlot if no offhand equipped
            if (GameObject.FindGameObjectWithTag("OffHandSlot").transform.childCount <= 0)
            {
                DragHandler.itemBeingDragged.transform.SetParent(transform);
            }
        }
        else if (type.ToString() == "OneHand" && CompareTag("MainHandSlot"))
        {
            //Set item in MainHandSlot
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }
        else if (type.ToString() == "OffHand" && CompareTag("OffHandSlot"))
        {
            //Set item in offhandslot, if no two hand equipped
            if (GameObject.FindGameObjectWithTag("MainHandSlot").transform.childCount <= 0)
            {
                DragHandler.itemBeingDragged.transform.SetParent(transform);
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("MainHandSlot").transform.GetChild(0).GetComponent<Item>().Type != Item.EType.TwoHand)
                {
                    DragHandler.itemBeingDragged.transform.SetParent(transform);
                }
            }
        }
        else  if (CompareTag("Slot") || tag == (type.ToString() + "Slot"))
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
        else
        {
            descriptionText.text = string.Empty;
        }

    }
}

