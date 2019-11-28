using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        private CharacterController mCharController;
        private string mHorizontal;
        private string mVertical;
        private float mSpeed;
        private bool mAllowInput;
        private bool mIsInitialized;

        void Start()
        {
            mCharController = GetComponent<CharacterController>();
        }

        public void InitMovement(float speed, string horizontalAxis, string verticalAxis)
        {
            mIsInitialized = true;
            mHorizontal = horizontalAxis;
            mVertical = verticalAxis;
            mSpeed = speed;
            Pause(false);
        }

        public void Pause(bool value)
        {
            mAllowInput = !value;
        }

        void Update()
        {
            HandlePlayerInputs();
        }

        void HandlePlayerInputs()
        {
            if (mAllowInput && mIsInitialized)
            {
                Vector3 movementVector = new Vector3(Input.GetAxis(mHorizontal), 0, Input.GetAxis(mVertical)) * mSpeed * Time.deltaTime;
                mCharController.Move(movementVector);
            }
        }
    }
}
