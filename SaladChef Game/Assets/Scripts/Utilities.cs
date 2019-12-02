using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    /// <summary>
    /// Utilities class - can have more static methods to handle common game functionalities
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Generates a integer list of non repetitive random numbers
        /// </summary>
        /// <param name="count">size of list</param>
        /// <param name="minValue">starting value</param>
        /// <param name="maxValue">ending value</param>
        /// <returns>returns null of requested number of randoms is more than the range, else returns a list of integer</returns>
        public static List<int> GetNonRepetitiveRandom(int count, int minValue, int maxValue)
        {
            if (count > maxValue - minValue)
                return null;

            List<int> nonRepetitiveRandom = new List<int>(count);
            List<int> curValues = new List<int>();

            for (int i = 0; i <= maxValue - minValue; i++)
                curValues.Add(minValue + i);

            for (int i = 0; i < count; i++)
            {
                int random = Random.Range(0, curValues.Count);
                nonRepetitiveRandom.Add(curValues[random]);
                curValues.RemoveAt(random);
            }

            return nonRepetitiveRandom;
        }
    }
}

