using System.Collections;
using UnityEngine;

namespace SaladChef
{
    /// <summary>
    /// A chopping board class that functions vegetable chopping based on vegetable config and moves the chopped ingredient to respective plate
    /// </summary>
    public class ChoppingBoard : MonoBehaviour, IDroppable
    {
        [HideInInspector]
        public bool _IsBusy = false;
        [SerializeField] private Plate m_Plate = default;
        private GameObject mVegObject;
        private Coroutine mChopCoroutine;

        private void Start()
        {
            GameManager.pInstance.OnGameEnd += OnGameEnd;
        }

        public void OnDropItem(object droppedItem, PlayerController droppedBy)
        {
            if(!_IsBusy)
            {
                VegetableData veg = droppedItem as VegetableData;
                if (veg != null)
                {
                    _IsBusy = true;
                    mChopCoroutine = StartCoroutine("ChopVegetable", veg);
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

        private void OnGameEnd()
        {
            if (mChopCoroutine != null)
                StopCoroutine(mChopCoroutine);
        }
    }
}

