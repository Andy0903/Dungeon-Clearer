using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Outfit : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] bodyOutfit;

    [SerializeField]
    private Sprite[] body;

    private int currentBodyPart;

    enum OutfitPiece
    {
        Head = 0,
        Hair = 1,
        Body = 2,
        Hand = 3,
        Feet = 4
    }

    void Start()
    {
#if DEBUG
        currentBodyPart = 0;
#endif
    }

    void Update()
    {
#if DEBUG
        if(Input.GetKeyDown(KeyCode.O))
        {
            bodyOutfit[(int)OutfitPiece.Body].sprite = body[currentBodyPart++ % body.Length];
        }

#endif
    }

    void ChangeOutfit(Sprite sprite, OutfitPiece piece)
    {
        bodyOutfit[(int)piece].sprite = sprite;
    }
}
