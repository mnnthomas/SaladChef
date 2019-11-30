namespace SaladChef
{
    public interface IDroppable
    {
        void OnDropItem(object droppedItem, PlayerController droppedBy);
    }
}
