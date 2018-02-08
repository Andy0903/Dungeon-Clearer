using System;

public class StatComponent : IItemComponent
{
    int value;
    EStat type;

    public StatComponent(EStat type, int value) : base()
    {
        this.type = type;
        this.value = value;
    }

    public string Description
    {
        get { return "Grants " + value + " " + type; }
    }

    public void BindToItem(Item item)
    {
        item.OnEquip += Item_OnEquip;
    }

    void Item_OnEquip(Equipment inventory)
    {
        //Do some stuff when equipped
    }

    void Item_OnUnequip(Equipment invetory)
    {
        //Do some stuff when unequipped
    }
}
