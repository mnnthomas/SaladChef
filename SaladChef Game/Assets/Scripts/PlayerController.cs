using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace SaladChef
{
    public class PlayerController : MonoBehaviour
    {
        [Header("-- Stats --")]
        [SerializeField] private string m_PlayerName = default;
        [SerializeField] private float m_Speed = default;
        [SerializeField] private float m_Duration = default;
        [Header("-- Player movement and input variables --")]
        [SerializeField] private string m_HorizontalAxis = default;
        [SerializeField] private string m_VerticalAxis = default;
        [SerializeField] private string m_ActionKey = default;
        [Header("-- Player UI variables --")]
        [SerializeField] private Text m_TimerText = default;
        [SerializeField] private Text m_ScoreText = default;
        [Header("-- Items carry variable --")]
        [SerializeField] private int m_MaxVegetableInHand = default;

        private bool mInitialized;
        private bool mCanInteract = true;
        private MovementController mMovementController;
        private Queue<VegetableData> mVegetablesInHand = new Queue<VegetableData>();
        private List<GameObject> mVegetableObjectsInHand = new List<GameObject>();
        private Salad mSaladInHand;
        private GameObject mSaladObjectInHand;
        private bool mEndTimer;
        private Collider mCurCollider;
        private Coroutine mPauseCoroutine;

        public string pPlayerName { get { return m_PlayerName; } private set { }}
        public float pScore { get; private set; }
        private bool mCanPickVegetable
        {
            get { return mVegetablesInHand.Count < m_MaxVegetableInHand && mSaladInHand == null; }
        }


        void Start()
        {
            InitPlayer();
        }

        private void InitPlayer()
        {
            mMovementController = GetComponent<MovementController>();
            Customer.OnCorrectItemDelivered += OnCorrectSalad;
            Customer.OnItemNotDelivered += OnSaladNotDelivered;
            TrashCan.OnItemTrashed += OnItemTrashed;
            if (mMovementController)
                mMovementController.InitMovement(m_Speed, m_HorizontalAxis, m_VerticalAxis);
        }

        private void Update()
        {
            if (mCanInteract)
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
                m_TimerText.text = (int)m_Duration / 60 + " m " + (int)m_Duration % 60 + " s";
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
            if(mCurCollider)
            {
                //Condition to check and pick item from VegetableSpawn
                if (mCurCollider.GetComponent<VegetableSpawn>() && mCanPickVegetable)
                {
                    VegetableData pickedVeg = mCurCollider.GetComponent<IPickable>().PickItem() as VegetableData;
                    if(pickedVeg != null)
                    {
                        OnVegetablePicked(pickedVeg);
                    }
                }
                //Condition to drop a vegetable to chopping board
                else if (mCurCollider.GetComponent<ChoppingBoard>() && !mCurCollider.GetComponent<ChoppingBoard>()._IsBusy && mSaladInHand == null && mVegetablesInHand.Count > 0)
                {
                    OnDroppedItemInChoppingBoard(mVegetablesInHand.Peek());
                    mCurCollider.GetComponent<IDroppable>().OnDropItem(mVegetablesInHand.Dequeue(), this);
                }
                //Condition to drop an item to TrashCan
                else if(mCurCollider.GetComponent<TrashCan>())
                {
                    if (mVegetablesInHand.Count > 0)
                    {
                        DropVegetable(mVegetablesInHand.Peek());
                        mCurCollider.GetComponent<IDroppable>().OnDropItem(mVegetablesInHand.Dequeue(), this);
                    }
                    if(mSaladInHand != null)
                    {
                        mCurCollider.GetComponent<IDroppable>().OnDropItem(mSaladInHand, this);
                        DropSalad();
                    }
                }
                //Condition to pick salad plate
                else if(mCurCollider.GetComponent<Plate>() && mCurCollider.GetComponent<Plate>().HasSalad() && mVegetableObjectsInHand.Count == 0 && mSaladInHand == null)
                {
                    Salad pickedSalad = mCurCollider.GetComponent<IPickable>().PickItem() as Salad;
                    if(pickedSalad != null)
                    {
                        mSaladObjectInHand = Instantiate(mCurCollider.GetComponent<Plate>().gameObject, transform, true);
                        mSaladObjectInHand.transform.position = transform.position + Vector3.up;
                        OnSaladPicked(pickedSalad.DeepCopy());
                    }
                }
                //Condition to deliver to customer
                else if(mCurCollider.GetComponent<Customer>() && mSaladInHand != null)
                {
                    mCurCollider.GetComponent<IDroppable>().OnDropItem(mSaladInHand, this);
                    DropSalad();
                }
            }
        }

        private void OnVegetablePicked(VegetableData veg)
        {
            mVegetablesInHand.Enqueue(veg);
            if (veg._Object)
            {
                mVegetableObjectsInHand.Add(Instantiate(veg._Object, transform));
                if (mVegetablesInHand.Count == 1)
                    mVegetableObjectsInHand[0].transform.localPosition = Vector3.up * 1.5f;
                else
                    mVegetableObjectsInHand[1].transform.localPosition = Vector3.up * 2.5f;
            }
        }

        private void OnSaladPicked(Salad salad)
        {
            mSaladInHand = salad;
        }

        private void OnDroppedItemInChoppingBoard(VegetableData veg)
        {
            DropVegetable(veg);
            mPauseCoroutine = StartCoroutine("PauseForSeconds", veg._CutDuration);
        }

        private void DropSalad()
        {
            Destroy(mSaladObjectInHand);
            mSaladInHand = null;
        }

        private void DropVegetable(VegetableData veg)
        {
            Destroy(mVegetableObjectsInHand[0]);
            mVegetableObjectsInHand.RemoveAt(0);
            if (mVegetableObjectsInHand.Count > 0)
                mVegetableObjectsInHand[0].transform.localPosition = Vector3.up * 1.5f;
        }

        IEnumerator PauseForSeconds(float seconds)
        {
            AllowPlayerInputs(false);
            yield return new WaitForSeconds(seconds);
            AllowPlayerInputs(true);
        }

        private void AllowPlayerInputs(bool value)
        {
            mMovementController.Pause(!value);
            mCanInteract = value;
        }

        private void OnTimerEnd()
        {
            AllowPlayerInputs(false);
            if (mPauseCoroutine != null)
                StopCoroutine(mPauseCoroutine);
            GameManager.pInstance.OnPlayerTimerEnd();
        }

        private void OnTriggerEnter(Collider other)
        {
            mCurCollider = other;
        }

        private void OnTriggerExit(Collider other)
        {
            mCurCollider = null;
        }

        private void OnCorrectSalad(PlayerController player, float score)
        {
            if(player == this)
            {
                pScore += score;
                m_ScoreText.text = pScore.ToString();
            }
        }

        private void OnSaladNotDelivered(List<PlayerController> players, float angryScore, float score)
        {
            if (players != null && players.Count > 0 && players.Contains(this))
                pScore += angryScore;
            else
                pScore += score;

            m_ScoreText.text = pScore.ToString();
        }

        private void OnItemTrashed(PlayerController player, float score)
        {
            if (player == this)
            {
                pScore += score;
                m_ScoreText.text = pScore.ToString();
            }
        }

        private void OnTimePowerup(PowerupData powerupData)
        {
            Debug.Log("On Time powerup");
        }

        private void OnScorePowerup(PowerupData powerupData)
        {
            Debug.Log("On Score powerup");
        }

        private void OnSpeedPowerup(PowerupData powerupData)
        {
            Debug.Log("On Speed powerup");
        }

        private void OnDestroy()
        {
            Customer.OnCorrectItemDelivered -= OnCorrectSalad;
            Customer.OnItemNotDelivered -= OnSaladNotDelivered;
            TrashCan.OnItemTrashed += OnItemTrashed;
        }
    }
}
