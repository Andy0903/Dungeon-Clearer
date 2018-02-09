using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Outfit : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] bodyOutfit;

    enum OutfitPiece
    {
        Head = 0,
        Hair = 1,
        Body = 2,
        Hand = 3,
        Feet = 4
    }

    void ChangeOutfit(Sprite sprite, OutfitPiece piece)
    {
        bodyOutfit[(int)piece].sprite = sprite;
    }
}
