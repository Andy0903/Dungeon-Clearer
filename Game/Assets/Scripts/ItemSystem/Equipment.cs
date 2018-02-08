public class Equipment // : Monobehaviour
{
    struct Slot
    {
        public Item.EType type;
        public Item item;
    }

    Slot[] slots = new Slot[] {
        new Slot { type = Item.EType.Head     },
        new Slot { type = Item.EType.Shoulder },
        new Slot { type = Item.EType.Chest    },
        new Slot { type = Item.EType.Back     },
        new Slot { type = Item.EType.Wrist    },
        new Slot { type = Item.EType.Hands    },
        new Slot { type = Item.EType.Waist    },
        new Slot { type = Item.EType.Legs     },
        new Slot { type = Item.EType.Feet     },
        new Slot { type = Item.EType.Neck     },
        new Slot { type = Item.EType.Trinket  },
        new Slot { type = Item.EType.Finger   },
        new Slot { type = Item.EType.OneHand  },
        new Slot { type = Item.EType.TwoHand  },
        new Slot { type = Item.EType.OffHand  },
    };

    public Equipment()
    {

    }

    void EquipItem(Item item)
    {
        item.Equip(this);
    }

    void UnequipItem(Item item)
    {
        item.Unequip(this);
    }
}
