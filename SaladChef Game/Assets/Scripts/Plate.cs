using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class Plate : MonoBehaviour, IPickable
    {
        [HideInInspector]
        public Salad _Salad = new Salad();
        private List<GameObject> mCutVeg = new List<GameObject>();

        /// <summary>
        /// Clears plate and returns a copy of salad
        /// </summary>
        /// <returns>Deep copy of Salad item</returns>
        public object PickItem()
        {
            Salad deepCopySalad = _Salad.DeepCopy();
            ClearPlate();
            return deepCopySalad;
        }

        public void AddSaladIngredient(VegetableData veg)
        {
            _Salad.AddIngredients(veg);

            mCutVeg.Add(Instantiate(veg._CutObject, transform, true));
            mCutVeg[mCutVeg.Count -1].transform.position = transform.position + new Vector3(Random.Range(0, 0.25f), 0.1f * _Salad._Ingredients.Count, Random.Range(0, 0.25f));
        }

        /// <summary>
        /// Checks if there are any ingredients in the salad
        /// </summary>
        /// <returns>true/false based on ingredients count</returns>
        public bool HasSalad()
        {
            if (_Salad != null && _Salad._Ingredients.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Clears the current salad in hand and destroys cut vegetable items in plate
        /// </summary>
        public void ClearPlate()
        {
            for (int i = 0; i < mCutVeg.Count; i++)
                Destroy(mCutVeg[i].gameObject);

            mCutVeg.Clear();
            _Salad = null;
        }
    }
}

