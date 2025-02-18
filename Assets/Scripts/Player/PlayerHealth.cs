using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] private int m_currentHealth = 100;
    [SerializeField] private int m_maxHealth = 100;
    [SerializeField] private TopDownCharacterController m_topDownCharacterController;

    [Header("Miscellaneuos Parameters")]
    [SerializeField] private Light2D m_torch;

    private ParticleSystem m_smokePuff;

    void Start()
    {
        m_smokePuff = m_topDownCharacterController.gameObject.GetComponent<ParticleSystem>();
        m_smokePuff.Stop();
        m_currentHealth = m_maxHealth;
    }

    public int GetCurrentHealth()//It needs a getter because serialize field makes it private to everything except the inspector
    {
        return m_currentHealth;
    }

    public void TakeDamage(int amount, bool showMask)//Essentially a setter
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth - amount, 0, m_maxHealth);

        if (m_currentHealth <= 70 && m_currentHealth >= 50)
        {
            m_torch.gameObject.SetActive(true);
        }
        if (m_currentHealth <= 50 && m_currentHealth > 20)
        {
            m_torch.intensity = 4f;
            m_torch.pointLightInnerAngle = 42;//Sets the inner cone angle
            m_torch.pointLightOuterAngle = 70;//Sets the outer cone angle
        }
        else if (m_currentHealth <= 20 && m_currentHealth > 0)
        {
            m_torch.intensity = 6f;
            m_torch.pointLightInnerAngle = 24;
            m_torch.pointLightOuterAngle = 40;
        }
        else if (m_currentHealth == 0)
        {
            //Handles player death if health reaches zero
            //Player died!
            SceneManager.LoadScene("RespawnMenu");
        }

        if (showMask)
        {
            //Damage animation
            int i = 0;
            StartCoroutine(m_topDownCharacterController.ShowDamageMask(i));
        }
        else 
        {
            //Fire damage animation
            m_smokePuff.Play();
        }
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