using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_Speed = default;
    [SerializeField] private string m_HorizontalAxis = default;
    [SerializeField] private string m_VerticalAxis = default;
    [SerializeField] private string m_ActionKey = default;

    private bool mCanMove;
    private MovementController mMovementController;

    private Queue<GameObject> mVegetablesInHand = new Queue<GameObject>();
    private GameObject mSaladInHand;

    void Start()
    {
        mMovementController = GetComponent<MovementController>();
        if(mMovementController)
            mMovementController.InitMovement(m_Speed, m_HorizontalAxis, m_VerticalAxis);
    }

    private void Update()
    {
        HandleInputAction();
    }

    private void HandleInputAction()
    {
        if (Input.GetButtonDown(m_ActionKey))
            OnActionButtonClicked();
    }

    private void OnActionButtonClicked()
    {
        Debug.Log(name + " Action click");
    }

}
