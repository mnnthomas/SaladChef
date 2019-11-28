using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class GameManager : MonoBehaviour
    {
        public VegetableConfig _VegetableConfig;

        public static GameManager pInstance { get; private set; }

        private void Awake()
        {
            if (pInstance == null)
                pInstance = this;
        }

        private void OnDestroy()
        {
            pInstance = null;
        }
    }
}


