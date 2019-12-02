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

        /// <summary>
        /// Inits movement update
        /// </summary>
        /// <param name="speed">movement speed</param>
        /// <param name="horizontalAxis">name of horizontal axis</param>
        /// <param name="verticalAxis">name of vertical axis</param>
        public void InitMovement(float speed, string horizontalAxis, string verticalAxis)
        {
            mIsInitialized = true;
            mHorizontal = horizontalAxis;
            mVertical = verticalAxis;
            mSpeed = speed;
            Pause(false);
        }


        /// <summary>
        /// Pauses movement controller's update
        /// </summary>
        /// <param name="value">mAllowInput</param>
        public void Pause(bool value)
        {
            mAllowInput = !value;
        }

        void Update()
        {
            HandlePlayerInputs();
        }

        public void UpdateSpeed(float speed)
        {
            mSpeed = speed;
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
