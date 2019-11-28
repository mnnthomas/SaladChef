using System.Collections.Generic;
using System.Collections;
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

        [Header("-- Items carry variable --")]
        [SerializeField] private int m_MaxVegetableInHand = default;
        [SerializeField] private int m_MaxSaladInHand = default;

        private bool mCanInteract = true;
        private MovementController mMovementController;
        private Queue<VegetableData> mVegetablesInHand = new Queue<VegetableData>();
        private List<GameObject> mVegetableObjectsInHand = new List<GameObject>();
        private GameObject mSaladInHand;
        private bool mEndTimer;
        private Collider mCurCollider;
        private bool mCanPickVegetable
        {
            get { return mVegetablesInHand.Count < 2 && mSaladInHand == null; }
        }


        void Start()
        {
            mMovementController = GetComponent<MovementController>();
            if (mMovementController)
                mMovementController.InitMovement(m_Speed, m_HorizontalAxis, m_VerticalAxis);
        }

        private void Update()
        {
            if(mCanInteract)
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
            if(mCurCollider)
            {
                if (mCurCollider.GetComponent<VegetableSpawn>() && mCanPickVegetable)
                {
                    VegetableData pickedVeg = mCurCollider.GetComponent<IPickable>().PickItem() as VegetableData;
                    if(pickedVeg != null)
                    {
                        mVegetablesInHand.Enqueue(pickedVeg);
                        OnVegetablePicked(pickedVeg);
                    }
                }
                else if(mCurCollider.GetComponent<ChoppingBoard>() && !mCurCollider.GetComponent<ChoppingBoard>()._IsBusy && mSaladInHand == null && mVegetablesInHand.Count > 0)
                {
                    Debug.Log("Dropping item >>> " + mVegetablesInHand.Peek()._Name);
                    OnDroppedItemInChoppingBoard(mVegetablesInHand.Peek());
                    mCurCollider.GetComponent<IDroppable>().OnDropItem(mVegetablesInHand.Dequeue());
                }
                else if(mCurCollider.GetComponent<TrashCan>())
                {
                    if (mVegetablesInHand.Count > 0)
                    {
                        DropVegetable(mVegetablesInHand.Peek());
                        mCurCollider.GetComponent<IDroppable>().OnDropItem(mVegetablesInHand.Dequeue());
                    }
                }

            }
        }

        private void OnVegetablePicked(VegetableData veg)
        {
            if (veg._Object)
            {
                mVegetableObjectsInHand.Add(Instantiate(veg._Object, transform));
                if (mVegetablesInHand.Count == 1)
                    mVegetableObjectsInHand[0].transform.localPosition = Vector3.up * 1.5f;
                else
                    mVegetableObjectsInHand[1].transform.localPosition = Vector3.up * 2.5f;
            }
        }

        private void OnDroppedItemInChoppingBoard(VegetableData veg)
        {
            DropVegetable(veg);
            StartCoroutine("PauseForSeconds", veg._CutDuration);
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
            Debug.Log(name + " Times up");
        }

        private void OnTriggerEnter(Collider other)
        {
            mCurCollider = other;
        }

        private void OnTriggerExit(Collider other)
        {
            mCurCollider = null;
        }
    }
}
