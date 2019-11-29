using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    public class VegetableSpawn : MonoBehaviour, IPickable
    {
        [SerializeField] private string m_VegetableName = default;
        [SerializeField] private Transform m_SpawnTransform = default;

        private VegetableData mVegetable;

        private void Start()
        {
            mVegetable = GameManager.pInstance._VegetableConfig.GetVegetableByName(m_VegetableName);
            if (mVegetable != null && mVegetable._Object)
            {
                GameObject obj = Instantiate(mVegetable._Object, transform);
                obj.transform.position = m_SpawnTransform.position;
            }
        }

        public object PickItem()
        {
            return mVegetable;
        }
    }
}


