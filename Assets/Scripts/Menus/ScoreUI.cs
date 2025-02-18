using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private ScoreSystem m_scoreSystem;
    private TMPro.TextMeshProUGUI m_UILabel;

    private ScoreSystem ScoreSystem;
    private GameObject ScoreLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreSystem = GetComponent<ScoreSystem>();//score system reference
        if (GetComponent<ScoreSystem>() != null)//validation
        {
            m_scoreSystem = GetComponent<ScoreSystem>();
        }

        ScoreLabel = GameObject.Find("ScoreLabel");
        if (ScoreLabel != null) 
        {
            m_UILabel = ScoreLabel.GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_UILabel.text = "Score: " + m_scoreSystem.m_score;
    }
}
