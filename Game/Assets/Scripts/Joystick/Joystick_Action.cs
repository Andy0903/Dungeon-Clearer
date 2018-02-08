using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Joystick_Action : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    Image actionImage;
    private bool attackPressed;

    //Only allows you to press once per button mash
    public bool isAttackPressed
    {
        get
        {
            bool temp = attackPressed;
            attackPressed = false;
            return temp;
        }
    }

    void Start()
    {
        actionImage = GetComponent<Image>();
        attackPressed = false;
    }

    public void OnPointerUp(PointerEventData eData)
    {
        attackPressed = false;
    }

    public void OnPointerDown(PointerEventData eData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(actionImage.rectTransform, eData.position, eData.pressEventCamera))
        {
            attackPressed = true;
        }
    }
}
