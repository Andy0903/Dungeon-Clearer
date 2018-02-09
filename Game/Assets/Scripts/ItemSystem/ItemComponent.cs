public interface IItemComponent
{
    string Description { get; }
    IItemComponent BindToItem(Item item);
}
