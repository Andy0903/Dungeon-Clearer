using UnityEngine;

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
                slot.AddItem(ItemFactory.Instance.Create(Item.EType.Head));
            }
        }
    }
}
