public interface IItemComponent
{
    string Description { get; }
    void BindToItem(Item item);
}
