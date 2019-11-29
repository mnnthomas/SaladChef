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
        public Sprite _Sprite;
    }

    [System.Serializable]
    public class Salad
    {
        public List<VegetableData> _Ingredients = new List<VegetableData>();

        public Salad()
        {

        }

        public void AddIngredients(VegetableData vegData)
        {
            _Ingredients.Add(vegData);
        }

        public bool CompareSalad(List<VegetableData> saladList)
        {
            if (this.Equals(saladList))
                return true;
            return false;
        }
    }

    [CreateAssetMenu(menuName = "Vegetable configuration")]
    public class VegetableConfig : ScriptableObject
    {
        [SerializeField] private List<VegetableData> m_Vegetables = new List<VegetableData>();

        public VegetableData GetVegetableByName(string name)
        {
            return m_Vegetables.Find(x => x._Name == name);
        }

        public Salad GetRandomSalad(int count)
        {
            Salad randomSalad = new Salad();
            List<int> randoms = GetNonRepetableRandom(count, 0, m_Vegetables.Count-1);

            for (int i = 0; i < count; i++)
                randomSalad.AddIngredients(m_Vegetables[randoms[i]]);

            return randomSalad;
        }

        public List<int> GetNonRepetableRandom(int count, int minValue, int maxValue)
        {
            if (count > maxValue - minValue)
                return null;

            List<int> nonRepetableRandom = new List<int>(count);
            List<int> curValues = new List<int>();

            for(int i = 0; i <= maxValue - minValue; i++)
                curValues.Add(minValue + i);

            for(int i = 0; i < count; i ++)
            {
                int random = Random.Range(0, curValues.Count);
                nonRepetableRandom.Add(curValues[random]);
                curValues.RemoveAt(random);
            }

            return nonRepetableRandom;
        }

    }
}
