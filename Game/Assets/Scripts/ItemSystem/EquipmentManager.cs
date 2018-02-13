﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    Dictionary<string, EquipmentSlot> slots = new Dictionary<string, EquipmentSlot>();

    public Player Player { get; private set; }
    Text descriptionText;

    private void Awake()
    {
        descriptionText = GameObject.FindGameObjectWithTag("PlayerStatDescriptionText").GetComponent<Text>();
        descriptionText.gameObject.SetActive(false);
    }

    public void Start()
    {
        EquipmentSlot[] equipmentSlots = FindObjectOfType<MenuManager>().transform.GetComponentsInChildren<EquipmentSlot>(true);

        foreach (EquipmentSlot eq in equipmentSlots)
        {
            slots.Add(eq.tag, eq);
        }

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void UpdateText()
    {
        string description = "<color=orange>Player stats</color>\n";

        foreach (Stats.EType stat in Enum.GetValues(typeof(Stats.EType)))
        {
            description += stat.ToString() + ": " + Player.Stats[stat] + "\n";
        }

        descriptionText.text = description;
    }

    public bool TwoHandEquipped
    {
        get
        {
            return (slots[Item.EType.TwoHand.ToSlotType()].ContainedItem != null &&
                slots[Item.EType.TwoHand.ToSlotType()].ContainedItem.Type == Item.EType.TwoHand);
        }
    }

    public bool OffHandEquipped
    {
        get
        {
            return (slots[Item.EType.OffHand.ToSlotType()].ContainedItem != null);
        }
    }
}
