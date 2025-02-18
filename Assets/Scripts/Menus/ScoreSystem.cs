using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int m_score;
    /// <summary>
    /// Method to add score to the player
    /// </summary>
    /// <param name="scoreToAdd">The amount of score to be added.</param>
    public void AddScore(int scoreToAdd)
    {
        //Increment the score with the paramater
        m_score += scoreToAdd;
    }
    
}
