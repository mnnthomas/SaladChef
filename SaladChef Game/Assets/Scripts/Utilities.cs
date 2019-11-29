using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class Utilities
    {
        public static List<int> GetNonRepetableRandom(int count, int minValue, int maxValue)
        {
            if (count > maxValue - minValue)
                return null;

            List<int> nonRepetableRandom = new List<int>(count);
            List<int> curValues = new List<int>();

            for (int i = 0; i <= maxValue - minValue; i++)
                curValues.Add(minValue + i);

            for (int i = 0; i < count; i++)
            {
                int random = Random.Range(0, curValues.Count);
                nonRepetableRandom.Add(curValues[random]);
                curValues.RemoveAt(random);
            }

            return nonRepetableRandom;
        }
    }
}

