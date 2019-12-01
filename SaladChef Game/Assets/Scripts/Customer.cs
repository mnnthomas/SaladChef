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
        [SerializeField] private float m_UndeliveredScore = default;
        [Header("-- Customer Ingredient values --")]
        [SerializeField] private int m_minIngredient = default;
        [SerializeField] private int m_maxIngredient = default;

        public static System.Action<List<PlayerController>, float, float> OnItemNotDelivered;
        public static System.Action<PlayerController, float> OnCorrectItemDelivered;

        private Salad mCurRequestedSalad;
        private bool mIsAngry;
        private List<PlayerController> mPlayersWhoDeliveredWrong = new List<PlayerController>();
        private float mCurRequestStartTime;
        private float mCurRequestDuration;
        private Coroutine mCurRequestCoroutine;

        private void Start()
        {
            GameManager.pInstance.OnGameEnd += OnGameEnd;
            RequestNewSalad();
        }

        /// <summary>
        /// Resets previous salad request item and gets a new salad request
        /// </summary>
        private void RequestNewSalad()
        {
            //Getting a random salad and calculating timer duration
            int ingredientCount = Random.Range(m_minIngredient, m_maxIngredient + 1);
            mCurRequestedSalad = GameManager.pInstance._VegetableConfig.GetRandomSalad(ingredientCount);
            mCurRequestDuration = ingredientCount * m_WaitDurationPerIngredient;

            //Resetting and updating salad images on player
            ResetSaladImages();
            for (int i = 0; i < mCurRequestedSalad._Ingredients.Count; i++)
            {
                Transform child = m_ImagePanel.transform.GetChild(i);
                child.GetComponent<Image>().sprite = mCurRequestedSalad._Ingredients[i]._Sprite;
                child.gameObject.SetActive(true);
            }

            //resetting slider and angry values
            m_Slider.maxValue = mCurRequestDuration;
            m_Slider.value = 0;
            SetCustomerAngry(false);

            //Starting a new salad request timer
            if (mCurRequestCoroutine != null)
                StopCoroutine(mCurRequestCoroutine);
            mCurRequestCoroutine = StartCoroutine("StartNewRequestTimer");
        }

        private void ResetSaladImages()
        {
            foreach (Transform child in m_ImagePanel.transform)
            {
                child.GetComponent<Image>().sprite = null;
                child.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Salad request timer based on the number of ingredients in salad
        /// </summary>
        /// <returns></returns>
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
            OnItemNotDelivered?.Invoke(mPlayersWhoDeliveredWrong, m_UndeliveredScore * m_AngryScoreMultiplier, m_UndeliveredScore);
            RequestNewSalad();
        }

        /// <summary>
        /// Sets values for Customer angry status
        /// </summary>
        /// <param name="value">mIsAngry</param>
        /// <param name="player">Which player made the customer angry</param>
        private void SetCustomerAngry(bool value, PlayerController player = null)
        {
            mIsAngry = value;
 
            Material customerMat = GetComponent<Renderer>().material;
            customerMat.color = value ? Color.gray : Color.white;

            if (!value)
                mPlayersWhoDeliveredWrong.Clear();
            else if(value && player && !mPlayersWhoDeliveredWrong.Contains(player))
                    mPlayersWhoDeliveredWrong.Add(player);
        }
       
        /// <summary>
        /// On Salad delivered to customer
        /// </summary>
        /// <param name="droppedItem">Delivered salad</param>
        /// <param name="droppedBy">which player delivered</param>
        public void OnDropItem(object droppedItem, PlayerController droppedBy)
        {
            //Check if salad is the requested one
            Salad droppedSalad = droppedItem as Salad;
            if(droppedSalad != null)
            {
                if (mCurRequestedSalad.CompareSalad(droppedSalad))
                {
                    OnCorrectItemDelivered?.Invoke(droppedBy, m_CorrectScore);
                    if((m_Slider.value / m_Slider.maxValue * 100) < m_PowerUpSpawnPercentage)
                    {
                        Debug.Log("Delivered in " +(m_Slider.value / m_Slider.maxValue * 100)+ "% duration");
                        SpawnPowerUp(droppedBy);
                    }

                    RequestNewSalad();
                }
                else
                    SetCustomerAngry(true, droppedBy);
            }
        }

        private void SpawnPowerUp(PlayerController player)
        {
            Debug.Log("Powerup spawned for player " + player.gameObject.name);
        }

        private void OnGameEnd()
        {
            if (mCurRequestCoroutine != null)
                StopCoroutine(mCurRequestCoroutine);
        }
    }
}