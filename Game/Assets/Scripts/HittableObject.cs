using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour
{
    public bool isActivated
    {
        get;
        private set;
    }

    private SpriteRenderer sr;

    void Start()
    {
        isActivated = false;
        sr = GetComponent<SpriteRenderer>();
    }

    public void RegisterHit()
    {
        isActivated = !isActivated;
        if (isActivated)
        {
            ChangeColor(Color.blue);
        }
        else
        {
            ChangeColor(Color.white);
        }
    }

    private void ChangeColor(Color color)
    {
        sr.color = color;
    }
}
