using UnityEngine;

public abstract class ItemComponent
{
    public ItemComponent()
    {
    }

    public abstract void BindToItem(Item item);
}
