using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory // : Monobehaviour
{
    struct Slot
    {
        public Item.Type type;
        public Item item;
    }

    Slot[] slots = new Slot[] {
        new Slot { type = Item.Type.Head     },
        new Slot { type = Item.Type.Shoulder },
        new Slot { type = Item.Type.Chest    },
        new Slot { type = Item.Type.Back     },
        new Slot { type = Item.Type.Wrist    },
        new Slot { type = Item.Type.Hands    },
        new Slot { type = Item.Type.Waist    },
        new Slot { type = Item.Type.Legs     },
        new Slot { type = Item.Type.Feet     },
        new Slot { type = Item.Type.Neck     },
        new Slot { type = Item.Type.Trinket  },
        new Slot { type = Item.Type.Finger   },
        new Slot { type = Item.Type.OneHand  },
        new Slot { type = Item.Type.TwoHand  },
        new Slot { type = Item.Type.OffHand  },
    };

    public Inventory()
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
