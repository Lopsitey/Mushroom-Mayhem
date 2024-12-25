using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private int m_currentHealth;

    [SerializeField]
    private int m_maxHealth = 100;

    public int GetCurrentHealth()//needs a getter because serialise field makes it private to everything except the inspector
    {
        return m_currentHealth;
    }

    void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)//essentialy a setter
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth - amount, 0, m_maxHealth);

        // Handle player death if health reaches zero
        if (m_currentHealth == 0)
        {
            // Handle player death (e.g., game over)
            Debug.Log("Player died!");
        }
    }
    public void Heal(int value)//anoher setter
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth + value, 0, m_maxHealth);
    }
}