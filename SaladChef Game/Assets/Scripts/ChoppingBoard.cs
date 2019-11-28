using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class ChoppingBoard : MonoBehaviour, IDroppable
    {
        public bool _IsBusy = false;

        public void OnDropItem(object droppedItem)
        {
            if(!_IsBusy)
            {
                VegetableData veg = droppedItem as VegetableData;
                if (veg != null)
                {
                    _IsBusy = true;
                    StartCoroutine("ChopVegetable", veg);
                }
            }
        }

        IEnumerator ChopVegetable(VegetableData vegData)
        {
            yield return new WaitForSeconds(vegData._CutDuration);

            Debug.Log("Chopping done " + vegData._Name);
            _IsBusy = false;
        }
    }
}

