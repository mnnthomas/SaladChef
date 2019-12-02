namespace SaladChef
{
    /// <summary>
    /// Objects in scene that can be picked up implement the IPickable interface
    /// Eg: Vegetables, Plate
    /// </summary>
    public interface IPickable
    {
        object PickItem();
    }
}
