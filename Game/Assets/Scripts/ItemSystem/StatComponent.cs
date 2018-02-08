public class StatComponent : IItemComponent
{
    int value;

    public StatComponent(EStat type, int value) : base()
    {
        this.value = value;
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
