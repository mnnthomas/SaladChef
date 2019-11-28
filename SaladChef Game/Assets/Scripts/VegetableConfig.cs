using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    [System.Serializable]
    public class Vegetable
    {
        public string _Name;
        public GameObject _Object;
    }

    [CreateAssetMenu(menuName = "Vegetable configuration")]
    public class VegetableConfig : ScriptableObject
    {
        [SerializeField] private List<Vegetable> m_Vegetables = new List<Vegetable>();

        public Vegetable GetVegetableByName(string name)
        {
            return m_Vegetables.Find(x => x._Name == name);
        }

        public List<Vegetable> GetRandomCombination(int count)
        {
            List<Vegetable> randomList = new List<Vegetable>(count);
            List<Vegetable> mCurVegetables = m_Vegetables;

            if (count > mCurVegetables.Count)
                return null;

            for (int i = 0; i < count; i++)
            {
                int random = Random.Range(0, mCurVegetables.Count);
                randomList.Add(mCurVegetables[random]);
                mCurVegetables.RemoveAt(random);
            }
            return randomList;
        }
    }
}
