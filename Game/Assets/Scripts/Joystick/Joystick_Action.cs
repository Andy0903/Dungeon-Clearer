using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Joystick_Action : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    Image actionImage;

    public bool isAttackPressed
    {
        get;
        private set;
    }

    void Start()
    {
        actionImage = GetComponent<Image>();
        isAttackPressed = false;
    }

    public void OnPointerUp(PointerEventData eData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(actionImage.rectTransform, eData.position, eData.pressEventCamera))
        {
            Debug.Log("Attack pressed");
            isAttackPressed = true;
        }
    }

    public void OnPointerDown(PointerEventData eData)
    {
        isAttackPressed = false;
    }
}
