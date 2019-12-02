using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SaladChef
{

    /// <summary>
    /// Highscore data is a collection of Name(string) and score(float)
    /// </summary>
    public class HighScoreData
    {
        public string _Name;
        public float _Score;

        public HighScoreData(string name, float score)
        {
            _Name = name;
            _Score = score;
        }
    }

    public class HighScores : MonoBehaviour
    {
        [SerializeField] private string m_HighScoreFileName = default;
        public int _HighScoreCount;
        private List<HighScoreData> mHighScores = new List<HighScoreData>();
        private string mFilePath;

        private void Start()
        {
            mFilePath = Application.dataPath + "/" + m_HighScoreFileName + ".txt";
        }

       /// <summary>
       /// Reads and sorts highscore from the txt file path
       /// </summary>
       /// <returns> a list of sorted highscore data</returns>
        public List<HighScoreData> ReadHighScores()
        {
            if (File.Exists(mFilePath))
            {
                StreamReader sr = new StreamReader(mFilePath);
                string fileContents = sr.ReadToEnd();
                sr.Close();

                string[] linesInFile;
                linesInFile = fileContents.Split("\n"[0]);

                mHighScores.Clear();
                foreach (string line in linesInFile)
                {
                    string[] splitArray = line.Split(',');
                    if (!string.IsNullOrEmpty(splitArray[0]) && !string.IsNullOrEmpty(splitArray[1]))
                    {
                        HighScoreData highScore = new HighScoreData(splitArray[0], float.Parse(splitArray[1]));
                        mHighScores.Add(highScore);
                    }
                }

                //Sort Scores
                mHighScores = SortHighScores(mHighScores);
            }
            return mHighScores;
        }

        private List<HighScoreData> SortHighScores(List<HighScoreData> highscores)
        {
            if (highscores.Count > 1)
            {
                for (int i = 0; i < highscores.Count; i++)
                {
                    for (int j = 0; j < highscores.Count - 1; j++)
                    {
                        if (highscores[j]._Score < highscores[j + 1]._Score)
                        {
                            HighScoreData temp = highscores[j + 1];
                            highscores[j + 1] = highscores[j];
                            highscores[j] = temp;
                        }
                    }
                }
            }
            return highscores;
        }

        /// <summary>
        /// removes the least score on top highscore list and adds a new list of top highscores to the txt file
        /// </summary>
        public void AddHighScores(string name, float score)
        {
            if (!File.Exists(mFilePath))
            {
                FileStream newFile = File.Create(mFilePath);
                newFile.Close();
            }

            List<HighScoreData> highScoresData = ReadHighScores();
            if(highScoresData != null && highScoresData.Count >= _HighScoreCount)
            {
                highScoresData.RemoveAt(highScoresData.Count - 1);
                highScoresData.Add(new HighScoreData(name, score));

                ClearFile();
                StreamWriter sw = new StreamWriter(mFilePath, true);
                for(int i = 0; i < highScoresData.Count; i++)
                {
                    sw.WriteLine(highScoresData[i]._Name + "," + highScoresData[i]._Score);
                }
                sw.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(mFilePath, true);
                sw.WriteLine(name + "," + score.ToString());
                sw.Close();

            }
        }

        private void ClearFile()
        {
            FileInfo fInfo = new FileInfo(mFilePath);
            StreamWriter sw = new StreamWriter(fInfo.Open(FileMode.Truncate));
            sw.Write("");
            sw.Close();
        }

        /// <summary>
        /// checks if the given score can be in top highscores
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool IsHighScore(float score)
        {
            List<HighScoreData> highscoreData = ReadHighScores();
            if (highscoreData == null || highscoreData.Count < _HighScoreCount || score > highscoreData[highscoreData.Count - 1]._Score)
                return true;
            return false;
        }
    }
}
