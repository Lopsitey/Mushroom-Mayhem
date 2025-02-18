using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] private int m_currentHealth = 100;
    [SerializeField] private int m_maxHealth = 100;
    [SerializeField] private EnemyController m_enemyController;

    void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    public int GetCurrentHealth()//Just gets the health using the private variables
    {
        return m_currentHealth;//Indirectly exposes this variable - more secure
    }

    public void TakeDamage(int amount)//Essentially a setter
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth - amount, 0, m_maxHealth);

        // Handles enemy death
        if (m_currentHealth == 0)
        {
            Debug.Log("Enemy died!");
        }

        //Damage animation?
    }

    public void Heal(int value)//Another setter
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth + value, 0, m_maxHealth);
    }

    public EnemyController GetEnemyController() 
    {
       return m_enemyController;
    }
}