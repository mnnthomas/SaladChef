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
            InitVegetable();
        }

        /// <summary>
        /// Creates a one time instance of the vegetable in spawn area.
        /// </summary>
        private void InitVegetable()
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


