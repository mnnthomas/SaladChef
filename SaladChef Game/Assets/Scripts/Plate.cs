using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class Plate : MonoBehaviour, IPickable
    {
        public Salad mSalad = new Salad();
        private List<GameObject> mCutVeg = new List<GameObject>();

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
            mCutVeg[mCutVeg.Count -1].transform.position = transform.position + Vector3.up * 0.1f * mSalad._Ingredients.Count;
        }

        public bool HasSalad()
        {
            if (mSalad._Ingredients.Count > 0)
                return true;
            return false;
        }

        public void ClearPlate()
        {
            for (int i = 0; i < mCutVeg.Count; i++)
                Destroy(mCutVeg[i].gameObject);

            mCutVeg.Clear();
            mSalad = null;
        }
    }
}

