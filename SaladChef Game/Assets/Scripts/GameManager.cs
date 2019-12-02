using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SaladChef
{
    public class GameManager : MonoBehaviour
    {
        [Header("-- UI references --")]
        [SerializeField] private Text m_GameEndText = default;
        [SerializeField] private GameObject m_GameEndScreen = default;
        [SerializeField] private GameObject m_HighScoreSaveScreen = default;
        [SerializeField] private GameObject m_HighScoreTable = default;
        [SerializeField] private InputField m_HighScoreInputField = default;
        [Header("-- Object references --")]
        [SerializeField] private HighScores m_HighScoreObject = default;
        [SerializeField] private PlayerController m_Player1 = default;
        [SerializeField] private PlayerController m_Player2 = default;

        public VegetableConfig _VegetableConfig;
        public System.Action OnGameEnd;
        public static GameManager pInstance { get; private set; }

        private int mPlayersCompleted;
        private PlayerController mWonPlayer;

        private void Awake()
        {
            if (pInstance == null)
                pInstance = this;
        }

        public void OnPlayerTimerEnd()
        {
            mPlayersCompleted++;
            if(mPlayersCompleted == 2)
            {
                ShowGameEnd();
            }
        }

        public void ShowGameEnd()
        {
            OnGameEnd?.Invoke(); 
            m_GameEndScreen.SetActive(true);

            if (m_Player1.pScore < m_Player2.pScore)
            {
                m_GameEndText.text = m_Player2.pPlayerName+" has won !";
                mWonPlayer = m_Player2;
                if (m_HighScoreObject.IsHighScore(m_Player2.pScore))
                    m_HighScoreSaveScreen.SetActive(true);
            }
            else if (m_Player1.pScore > m_Player2.pScore)
            {
                m_GameEndText.text = m_Player1.pPlayerName+" has won !";
                mWonPlayer = m_Player1;
                if (m_HighScoreObject.IsHighScore(m_Player1.pScore))
                    m_HighScoreSaveScreen.SetActive(true);
            }
            else
                m_GameEndText.text = "Game tied !";
        }

        public void OnReset()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnHighScoreSaved()
        {
            if(!string.IsNullOrEmpty(m_HighScoreInputField.text))
            {
                m_HighScoreObject.AddHighScores(m_HighScoreInputField.text, mWonPlayer.pScore);
                m_HighScoreSaveScreen.SetActive(false);
            }
        }

        public void OnShowHighScoreTable(bool value)
        {
            m_HighScoreTable.SetActive(value);
        }

        public void OnExit()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            pInstance = null;
        }
    }
}


