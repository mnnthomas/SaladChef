using UnityEngine;

namespace SaladChef
{
    public class TrashCan : MonoBehaviour, IDroppable
    {
        [SerializeField] private float m_VegetableTrashedScore;
        [SerializeField] private float m_SaladTrashedScore;
        public static System.Action<PlayerController, float> OnItemTrashed;

        /// <summary>
        /// Decides which item was dropped and calls events
        /// </summary>
        /// <param name="droppedItem">Salad/Vegetable dropped</param>
        /// <param name="droppedBy">Player who dropped</param>
        public void OnDropItem(object droppedItem, PlayerController droppedBy)
        {
            VegetableData veg = droppedItem as VegetableData;
            if (veg != null)
                OnItemTrashed?.Invoke(droppedBy, m_VegetableTrashedScore);
            else
            {
                Salad salad = droppedItem as Salad;
                if (salad != null)
                    OnItemTrashed?.Invoke(droppedBy, m_SaladTrashedScore);
            }
        }
    }
}
