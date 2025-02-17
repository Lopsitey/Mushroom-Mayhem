using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CollectibleUI : MonoBehaviour
{
    [Header("Score Parameters")]
    [SerializeField] private const int m_maxScore = 20;

    [Header("Enemies Spawned")]
    [SerializeField] private GameObject[] m_enemyArray;

    private TMPro.TextMeshProUGUI m_UILabel;
    private TMPro.TextMeshProUGUI m_respawnUILabel;
    private TMPro.TextMeshProUGUI m_winUILabel;

    private GameObject m_scoreLabel;
    private GameObject m_respawnScoreLabel;
    private GameObject m_winScoreLabel;

    private static int m_currentScore = 0;//Static so it persists across scenes

    void Start()
    {
        m_scoreLabel = GameObject.Find("ScoreLabel");
        m_respawnScoreLabel = GameObject.Find("RespawnScore");
        m_winScoreLabel = GameObject.Find("WinScore");
        if (m_scoreLabel != null)
        {
            m_UILabel = m_scoreLabel.GetComponent<TMPro.TextMeshProUGUI>();
        }
        if (m_respawnScoreLabel != null)
        {
            m_respawnUILabel = m_respawnScoreLabel.GetComponent<TMPro.TextMeshProUGUI>();
            m_respawnUILabel.text = "Your final score: " + m_currentScore;
            m_currentScore = 0;//Resets on death
        }
        if (m_winScoreLabel != null)
        {
            double mins = Time.time > 60 ? Math.Truncate(Time.time / 60) : 0;
            double secs = Math.Truncate(Time.time - (mins * 60));//May be off by one on occasion
            m_winUILabel = m_winScoreLabel.GetComponent<TMPro.TextMeshProUGUI>();
            m_winUILabel.text = $"Your final time: {mins}:{secs}";
        }
    }

    public void SetScore(int score)//Sets the score securely
    {
        m_currentScore += score;
        m_UILabel.text = "Score: " + m_currentScore;
        if (m_currentScore == m_maxScore) 
        {
            SceneManager.LoadScene("WinScreen");//Finishes the game once the last object is collected
        }
        if (m_enemyArray != null) 
        {
            switch (m_currentScore)
            {
                case 5:
                    m_enemyArray[0].SetActive(true);//Activates the first enemy
                    break;
                case 10:
                    m_enemyArray[1].SetActive(true);
                    break;
                case 15:
                    m_enemyArray[2].SetActive(true);
                    break;
                case 17:
                    m_enemyArray[3].SetActive(true);
                    break;
                case 19:
                    m_enemyArray[4].SetActive(true);
                    break;
            }
        }
    }
    public int GetScore() 
    {
        return m_currentScore;
    }
}
