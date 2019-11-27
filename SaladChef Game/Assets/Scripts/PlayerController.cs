using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_Speed = default;
    [SerializeField] private string m_HorizontalAxis = default;
    [SerializeField] private string m_VerticalAxis = default;
    [SerializeField] private string m_ActionKey = default;

    private bool mCanMove;
    private Queue<GameObject> mItemsInHand = new Queue<GameObject>();

    private CharacterController mCharController;

    void Start()
    {
        mCharController = GetComponent<CharacterController>();
        mCanMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerInputs();
    }

    void HandlePlayerInputs()
    {
        if(mCanMove)
        {
            Vector3 movementVector = new Vector3(Input.GetAxis(m_HorizontalAxis), 0, Input.GetAxis(m_VerticalAxis)) * m_Speed * Time.deltaTime;
            mCharController.Move(movementVector);
        }

        if(Input.GetButtonDown(m_ActionKey))
            OnActionKey();
    }

    void OnActionKey()
    {
        Debug.Log(name+"Action Key Pressed");
    }
}
