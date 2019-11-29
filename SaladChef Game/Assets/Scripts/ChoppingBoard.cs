using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class ChoppingBoard : MonoBehaviour, IDroppable
    {
        public bool _IsBusy = false;
        [SerializeField] private Plate m_Plate = default;
        private GameObject mVegObject;

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
            mVegObject = Instantiate(vegData._Object, transform, true);
            mVegObject.transform.position = transform.position + Vector3.up * 0.75f;

            yield return new WaitForSeconds(vegData._CutDuration);
            Destroy(mVegObject);
            m_Plate.AddSaladIngredient(vegData);
            Debug.Log("Chopping done " + vegData._Name);
            _IsBusy = false;
        }
    }
}

