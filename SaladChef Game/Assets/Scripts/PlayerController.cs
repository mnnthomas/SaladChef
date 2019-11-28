using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace SaladChef
{
    public class PlayerController : MonoBehaviour
    {
        [Header("-- Stats --")]
        [SerializeField] private float m_Speed = default;
        [SerializeField] private float m_Duration = default;

        [Header("-- Player movement and input variables --")]
        [SerializeField] private string m_HorizontalAxis = default;
        [SerializeField] private string m_VerticalAxis = default;
        [SerializeField] private string m_ActionKey = default;

        [Header("-- Player UI variables --")]
        [SerializeField] private Text m_Timer = default;
        [SerializeField] private Text m_Score = default;

        private MovementController mMovementController;
        private Queue<GameObject> mVegetablesInHand = new Queue<GameObject>();
        private GameObject mSaladInHand;
        private string mTriggerTag;
        private bool mEndTimer;

        void Start()
        {
            mMovementController = GetComponent<MovementController>();
            if (mMovementController)
                mMovementController.InitMovement(m_Speed, m_HorizontalAxis, m_VerticalAxis);
        }

        private void Update()
        {
            HandleInputAction();
            UpdateTimer();
        }

        private void HandleInputAction()
        {
            if (Input.GetButtonDown(m_ActionKey))
                OnActionButtonClicked();
        }

        private void UpdateTimer()
        {
            if (m_Duration > 0)
            {
                m_Duration -= Time.deltaTime;
                m_Timer.text = (int)m_Duration / 60 + " m " + (int)m_Duration % 60 + " s";
            }
            else
            {
                if(!mEndTimer)
                {
                    mEndTimer = true;
                    OnTimerEnd();
                }
            }
        }

        private void OnActionButtonClicked()
        {
            Debug.Log(name + " Action click");
        }

        private void OnTimerEnd()
        {
            Debug.Log(name + " Times up");
        }

        private void OnTriggerEnter(Collider other)
        {
            mTriggerTag = other.tag;
        }

        private void OnTriggerExit(Collider other)
        {
            mTriggerTag = string.Empty;
        }
    }
}
