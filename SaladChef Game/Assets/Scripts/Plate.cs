using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class Plate : MonoBehaviour, IPickable
    {
        public Salad mSalad = new Salad();
        private List<GameObject> mCutVeg = new List<GameObject>();

        /// <summary>
        /// Clears plate and returns a copy of salad
        /// </summary>
        /// <returns>Deep copy of Salad item</returns>
        public object PickItem()
        {
            Salad deepCopySalad = mSalad.DeepCopy();
            ClearPlate();
            return deepCopySalad;
        }

        public void AddSaladIngredient(VegetableData veg)
        {
            mSalad.AddIngredients(veg);

            mCutVeg.Add(Instantiate(veg._CutObject, transform, true));
            mCutVeg[mCutVeg.Count -1].transform.position = transform.position + new Vector3(Random.Range(0, 0.25f), 0.1f * mSalad._Ingredients.Count, Random.Range(0, 0.25f));
        }

        /// <summary>
        /// Checks if there are any ingredients in the salad
        /// </summary>
        /// <returns>true/false based on ingredients count</returns>
        public bool HasSalad()
        {
            if (mSalad._Ingredients.Count > 0)
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
            mSalad = null;
        }
    }
}

