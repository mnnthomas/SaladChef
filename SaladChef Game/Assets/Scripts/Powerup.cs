using UnityEngine;

namespace SaladChef
{
    [System.Serializable]
    public class PowerupData
    {
        public float _PowerupDuration;
        public float _PowerupMultiplier;
    }

    /// <summary>
    /// An abstract powerup class to handle common functionalities of powerup
    /// </summary>
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] protected PowerupData m_PowerupData;
        [SerializeField] protected string m_MethodToCall;
        private PlayerController mPlayer;
        private Renderer mRenderer;

        private void OnTriggerEnter(Collider other)
        {
            if(mPlayer != null && other.GetComponent<PlayerController>() == mPlayer)
            {
                mPlayer.SendMessage(m_MethodToCall, m_PowerupData);
                Destroy(gameObject);
            }
        }

        public virtual void SetPlayer(PlayerController player)
        {
            mPlayer = player;
            Renderer playerRend = player.GetComponent<Renderer>();
            mRenderer = GetComponent<Renderer>();

            if (mRenderer && playerRend)
                mRenderer.material.SetColor("_Color", playerRend.material.color);
        }
    }
}
