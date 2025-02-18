using System;
using System.Collections;
using UnityEngine;

public class ExplosionTest : MonoBehaviour
{
    [SerializeField, Range(0.1f, 5f)] private float m_explosionFrequency = 2f;//allows the user to change the value between a given range
    [SerializeField, Range(0.5f, 4f)] private float m_explosionRadius = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        StartCoroutine(ExplosionSpawner());//starts the coroutine on game start
    }

    private IEnumerator ExplosionSpawner()
    {
        while (true) 
        {
            //Wait for a moment
            yield return new WaitForSeconds(m_explosionFrequency);//waits for the specified time period

            //Find the position to spawn an explosion
            Vector3 explosionPosition = transform.position;
            explosionPosition += (Vector3)UnityEngine.Random.insideUnitCircle.normalized * m_explosionRadius;
            //the position is affected by a random location within the small circle created by .insideUnitCircle
            //this circle is then magnified by the explosion radius so the location the explosion can spawn is in a larger random area around the character
            //this is then converted to a vector with (Vector3)

            //Spawn an explosion
            VFXManager.CreateExplosion(explosionPosition);
        }
    }
}
