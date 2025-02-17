using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage Parameters")]
    [SerializeField] public int m_damageAmount = 50;//Default is 50 damage, half health per hit
    [SerializeField] public float m_damageCooldownAmount = 2f;//Default is 2 seconds

    private bool m_damaging = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObj = collision.gameObject;
        if (otherObj.CompareTag("Player") && !m_damaging)
        {
            //Start damaging the player
            m_damaging = true;
            PlayerHealth playerHealth = otherObj.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(m_damageAmount, false);//Apply damage but son't show the mask
            }
            StartCoroutine(DamageCooldown());
        }
    }
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(m_damageCooldownAmount);
        m_damaging = false;
    }
}