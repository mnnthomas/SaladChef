﻿using System.Collections;
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

        public bool CompareSalad(Salad compareSalad)
        {
            if (_Ingredients.Count != compareSalad._Ingredients.Count)
                return false;

            for(int i = 0; i < _Ingredients.Count; i++)
            {
                if (!compareSalad._Ingredients.Contains(_Ingredients[i]))
                    return false;
            }
            return true;
           
        }

        public Salad DeepCopy()
        {
            Salad deepCopy = new Salad();
            deepCopy._Ingredients.AddRange(_Ingredients);
            return deepCopy;
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
            List<int> randoms = Utilities.GetNonRepetableRandom(count, 0, m_Vegetables.Count-1);

            for (int i = 0; i < count; i++)
                randomSalad.AddIngredients(m_Vegetables[randoms[i]]);

            return randomSalad;
        }
    }
}
