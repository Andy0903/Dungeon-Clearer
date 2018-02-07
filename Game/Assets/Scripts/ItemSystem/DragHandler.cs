using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Based on https://www.youtube.com/watch?v=c47QYgsJrWc (Access date: 2018-02-07)

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    public void OnBeginDrag(PointerEventData data)
    {
        itemBeingDragged = gameObject;
        startPosition = gameObject.transform.position;
        startParent = gameObject.transform.parent;
    }

    public void OnDrag(PointerEventData data)    //TODO fixa till mobil.
    {
        gameObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (gameObject.transform.parent == startParent)
        {
            gameObject.transform.position = startPosition;
        }

        itemBeingDragged = null;
    }
}
