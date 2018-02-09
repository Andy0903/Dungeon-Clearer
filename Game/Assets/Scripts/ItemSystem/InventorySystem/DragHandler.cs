﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Based on https://www.youtube.com/watch?v=c47QYgsJrWc (Access date: 2018-02-07)

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged; //TODO property
    Vector3 startPosition;
    Transform startParent;
    Transform parentToKeepAboveAllWhenDragged;

    public void Awake()
    {
        parentToKeepAboveAllWhenDragged = GameObject.FindGameObjectWithTag("InventoryPanel").transform;
    }

    public void OnBeginDrag(PointerEventData data) //Called when starting to drag.
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        itemBeingDragged.transform.SetParent(parentToKeepAboveAllWhenDragged);
    }

    public void OnDrag(PointerEventData data)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData data)
    {
        startParent.gameObject.GetComponent<InventorySlot>().Deselect();
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == parentToKeepAboveAllWhenDragged)
        {
            transform.position = startPosition;
            transform.SetParent(startParent, false);
        }
    }
}
