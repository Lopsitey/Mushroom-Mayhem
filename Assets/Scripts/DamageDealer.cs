using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] public int m_damageAmount = 10; // Adjust damage value as needed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObj = collision.gameObject;
        if (otherObj.CompareTag("Player"))
        {
            // Apply damage to the player
            PlayerHealth playerHealth = otherObj.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(m_damageAmount);
                //Output that damage was taken
                Debug.Log("Player took damage!");
                Debug.Log("Player health is now: " + playerHealth.GetCurrentHealth());
            }
        }
    }
}