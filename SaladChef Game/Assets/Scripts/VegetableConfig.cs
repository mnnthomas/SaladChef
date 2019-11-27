using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vegetables
{
    public string _Name;
    public GameObject _Object;
}

[CreateAssetMenu(menuName = "Vegetable configuration")]
public class VegetableConfig : ScriptableObject
{
    [SerializeField] private List<Vegetables> m_Vegetables = new List<Vegetables>();

    public GameObject GetObjectByName(string name)
    {
        return m_Vegetables.Find(x => x._Name == name)._Object;
    }

    public List<Vegetables> GetRandomCombination(int count)
    {
        List<Vegetables> randomList = new List<Vegetables>(count);
        List<Vegetables> mCurVegetables = m_Vegetables;

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
