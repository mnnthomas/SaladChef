namespace SaladChef
{
    /// <summary>
    /// Objects in scene that are expecting a item to be dropped on them implement the IDroppable.
    /// Eg: Choppingboard, Customer, TrashCan
    /// </summary>
    public interface IDroppable
    {
        void OnDropItem(object droppedItem, PlayerController droppedBy);
    }
}
