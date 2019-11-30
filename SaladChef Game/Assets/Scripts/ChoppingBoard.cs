using System.Collections;
using UnityEngine;

namespace SaladChef
{
    public class ChoppingBoard : MonoBehaviour, IDroppable
    {
        public bool _IsBusy = false;
        [SerializeField] private Plate m_Plate = default;
        private GameObject mVegObject;

        public void OnDropItem(object droppedItem, PlayerController droppedBy)
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

        /// <summary>
        /// Creates a copy of dropped vegetable and Adds cut ingredient to plate after vegetable cut duration
        /// </summary>
        /// <param name="vegData">dropped vegetable data</param>
        /// <returns></returns>
        IEnumerator ChopVegetable(VegetableData vegData)
        {
            mVegObject = Instantiate(vegData._Object, transform, true);
            mVegObject.transform.position = transform.position + Vector3.up * 0.75f;

            yield return new WaitForSeconds(vegData._CutDuration);
            Destroy(mVegObject);
            m_Plate.AddSaladIngredient(vegData);
            _IsBusy = false;
        }
    }
}

