using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    Transform equipment;
    [SerializeField]
    Transform inventory;

    public void PutItemToFirstEmptySlot()
    {
        bool slotFound = false;

        for (int i = 0; i < inventory.transform.childCount && !slotFound; i++)
        {
            InventorySlot slot = inventory.GetChild(i).GetComponent<InventorySlot>();

            if (slot.ContainedItem == null)
            {
                slotFound = true;

                Item.EType type = Item.EType.Back;
                switch (Random.Range(0, 10))
                {
                    case 0: type = Item.EType.Head; break;
                    case 1: type = Item.EType.Chest; break;
                    case 2: type = Item.EType.Back; break;
                    case 3: type = Item.EType.Hands; break;
                    case 4: type = Item.EType.Waist; break;
                    case 5: type = Item.EType.Legs; break;
                    case 6: type = Item.EType.Feet; break;
                    case 7: type = Item.EType.OneHand; break;
                    case 8: type = Item.EType.TwoHand; break;
                    case 9: type = Item.EType.OffHand; break;
                }

                slot.AddItem(ItemFactory.Instance.Create(type));
            }
        }
    }

    public void DeleteItem()
    {
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            InventorySlot slot = inventory.GetChild(i).GetComponent<InventorySlot>();

            if (slot.ContainedItem != null && slot.GetComponent<Outline>().enabled)
            {
                Destroy(slot.ContainedItem.gameObject);
                slot.RemoveItem();
                slot.Deselect();
            }
        }
    }
}
