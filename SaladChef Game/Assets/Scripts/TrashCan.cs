using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class TrashCan : MonoBehaviour, IDroppable
    {
        public void OnDropItem(object droppedItem)
        {
            VegetableData veg = droppedItem as VegetableData;
            if (veg != null)
                Debug.Log("Vegetable trashed " + veg._Name);
            else
            {
                Salad salad = droppedItem as Salad;
                Debug.Log("Salad trashed with " + salad._Ingredients.Count +" ingredients");
            }
        }
    }
}
