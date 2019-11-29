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

        [Header("-- Customer Timer values --")]
        [SerializeField] private float m_WaitDurationPerIngredient = default;
        [SerializeField] private float m_PowerUpSpawnPercentage = default;
        [Header("-- Customer Angry values --")]
        [SerializeField] private float m_AngryTimeMultiplier = default;
        [SerializeField] private float m_AngryScoreMultiplier = default;
        [Header("-- Customer Score values --")]
        [SerializeField] private float m_CorrectScore = default;
        [SerializeField] private float m_WrongScore = default;
        [Header("-- Customer Ingredient values --")]
        [SerializeField] private int m_minIngredient = default;
        [SerializeField] private int m_maxIngredient = default;

        private Salad mCurRequestedSalad;
        private bool mIsAngry;
        private float mCurRequestStartTime;
        private float mCurRequestDuration;
        private Coroutine mCurRequestCoroutine;

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
            m_Slider.value = 0;
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);

            if (mCurRequestCoroutine != null)
                StopCoroutine(mCurRequestCoroutine);
            mCurRequestCoroutine = StartCoroutine("StartNewRequestTimer");
        }

        IEnumerator StartNewRequestTimer()
        {
            while(m_Slider.value < m_Slider.maxValue)
            {
                if (!mIsAngry)
                    m_Slider.value += Time.deltaTime;
                else
                    m_Slider.value += Time.deltaTime * m_AngryTimeMultiplier;

                yield return new WaitForEndOfFrame();
            }

            OnRequestTimerEnd();
        }

        private void OnRequestTimerEnd()
        {
            RequestNewSalad();
        }

        public void OnDropItem(object droppedItem)
        {
            //Check if salad is the requested one
            Salad droppedSalad = droppedItem as Salad;
            if(droppedSalad != null)
            {
                if (mCurRequestedSalad.CompareSalad(droppedSalad))
                {
                    Debug.Log("Correct !!!");
                    RequestNewSalad();
                }
                else
                {
                    GetComponent<Renderer>().material.SetColor("_Color",Color.gray);
                    Debug.Log("Wrong !!!");
                    mIsAngry = true;
                }
            }
        }
    }
}