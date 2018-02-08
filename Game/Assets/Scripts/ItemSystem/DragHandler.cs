﻿using UnityEngine;
using UnityEngine.EventSystems;

// Based on https://www.youtube.com/watch?v=c47QYgsJrWc (Access date: 2018-02-07)

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged; //TODO property
    Vector3 startPosition;
    Transform startParent;

    public void OnBeginDrag(PointerEventData data)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData data)    //TODO fixa till mobil.
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData data)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }
}
