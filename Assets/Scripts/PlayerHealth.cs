using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] private int m_currentHealth = 100;
    [SerializeField] private int m_maxHealth = 100;
    [SerializeField] private TopDownCharacterController m_topDownCharacterController;

    void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    public int GetCurrentHealth()//It needs a getter because serialize field makes it private to everything except the inspector
    {
        return m_currentHealth;
    }

    public void TakeDamage(int amount)//Essentially a setter
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth - amount, 0, m_maxHealth);

        //Handles player death if health reaches zero
        if (m_currentHealth == 0)
        {
            //Player died!
            SceneManager.LoadScene("RespawnMenu");
        }

        //Damage animation
        int i = 0;
        StartCoroutine(m_topDownCharacterController.ShowDamageMask(i));
    }

    public void Heal(int value)//Another setter
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth + value, 0, m_maxHealth);
    }

    public TopDownCharacterController GetTopDown()
    {
        return m_topDownCharacterController;
    }
}