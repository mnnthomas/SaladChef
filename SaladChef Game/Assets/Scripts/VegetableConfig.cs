using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    [System.Serializable]
    public class VegetableData
    {
        public string _Name;
        public GameObject _Object;
        public GameObject _CutObject;
        public float _CutDuration;
    }

    [CreateAssetMenu(menuName = "Vegetable configuration")]
    public class VegetableConfig : ScriptableObject
    {
        [SerializeField] private List<VegetableData> m_Vegetables = new List<VegetableData>();

        public VegetableData GetVegetableByName(string name)
        {
            return m_Vegetables.Find(x => x._Name == name);
        }

        public List<VegetableData> GetRandomCombination(int count)
        {
            List<VegetableData> randomList = new List<VegetableData>(count);
            List<VegetableData> mCurVegetables = m_Vegetables;

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
