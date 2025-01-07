using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public ScoreSystem m_scoreSystem;//score system reference

    public TMPro.TextMeshProUGUI m_UILabel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_UILabel.text = "Score: " + m_scoreSystem.m_score;
    }
}
