﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Joystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler 
{

    private Image backgroundImg;
    private Image joyStickImg;
    private Vector2 input; 

    public float Horizontal
    {
        get
        {
            return (input.x != 0) ? input.x : Input.GetAxis("Horizontal");
        }
    }

    public float Vertical
    {
        get
        {
            return (input.y != 0) ? input.y : Input.GetAxis("Vertical");
        }
    }

    // Use this for initialization
    void Start ()
    {
        backgroundImg = GetComponent<Image>();
        joyStickImg = transform.GetChild(0).GetComponentInChildren<Image>(); //Should always result in JoyStick Image  
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnDrag(PointerEventData eData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImg.rectTransform, eData.position, eData.pressEventCamera, out pos))
        {
            //Debug.Log("OnDrag test");

            pos.x = pos.x / backgroundImg.rectTransform.sizeDelta.x;
            pos.y = pos.y / backgroundImg.rectTransform.sizeDelta.y;

            input = new Vector2(pos.x * 2 + 1, pos.y * 2 - 1); //Offsetting the input vector
            input = (input.magnitude > 1.0f) ? input.normalized : input;

            joyStickImg.rectTransform.anchoredPosition = new Vector2(input.x * backgroundImg.rectTransform.sizeDelta.x / 2, 
                                                                    input.y * backgroundImg.rectTransform.sizeDelta.y / 2);


        }
    }

    public virtual void OnPointerDown(PointerEventData eData)
    {
        OnDrag(eData); //If touching the screen redirect to OnDrag
    }

    public void OnPointerUp(PointerEventData eData)
    {
        input = joyStickImg.rectTransform.anchoredPosition = Vector2.zero; //reset
    }

    
}