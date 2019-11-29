using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SaladChef
{
    public class Customer : MonoBehaviour, IDroppable
    {
        [Header("-- References --")]
        [SerializeField] private Slider m_Slider = default;
        [SerializeField] private GameObject m_ImagePanel = default;

        [Header("-- Customer values --")]
        [SerializeField] private float m_WaitDurationPerIngredient = default;
        [SerializeField] private float m_PowerUpSpawnPercentage = default;
        [SerializeField] private float m_AngryMultiplier = default;
        [SerializeField] private int m_minIngredient = default;
        [SerializeField] private int m_maxIngredient = default;

        private Salad mCurRequestedSalad;
        private bool mIsAngry;
        private float mCurRequestStartTime;
        private float mCurRequestDuration;

        private void Start()
        {
            RequestNewSalad();
        }

        private void RequestNewSalad()
        {
            int ingredientCount = Random.Range(m_minIngredient, m_maxIngredient + 1);
            mCurRequestedSalad = GameManager.pInstance._VegetableConfig.GetRandomSalad(ingredientCount);
            for(int i = 0; i < mCurRequestedSalad._Ingredients.Count; i++)
            {
                Transform child = m_ImagePanel.transform.GetChild(i);
                child.GetComponent<Image>().sprite = mCurRequestedSalad._Ingredients[i]._Sprite;
                child.gameObject.SetActive(true);
            }
            mCurRequestDuration = ingredientCount * m_WaitDurationPerIngredient;
            m_Slider.maxValue = mCurRequestDuration;
            StartCoroutine("StartRequestTimer");
        }

        IEnumerator StartRequestTimer()
        {
            float value;
            while(m_Slider.value <= 1)
            {
                value = Time.deltaTime / mCurRequestDuration;
                m_Slider.value += value;
                yield return null;
            }
        }


        public void OnDropItem(object droppedItem)
        {
            //Check if salad is the requested one
        
        }
    }
}