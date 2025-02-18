using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class CollectibleBehavior : MonoBehaviour
{
    [Header("Score Parameters")]
    [SerializeField] private int m_scoreGiven = 1;
    [SerializeField] private CollectibleUI m_collectibleUI;

    [Header("Miscellaneous Parameters")]
    [SerializeField] private float m_sunFlashTime = 0.25f;

    private Light2D m_sun;
    private bool m_flashCooldown = false;
    private bool m_collected = false;

    private void Awake() 
    {
        m_sun = GameObject.Find("Sun").GetComponent<Light2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)//Ensures that it isn't triggered twice before its destroyed
    {
        if (collision.gameObject.name == "Character" && !m_collected)
        {
            m_collected = true;//Stops multiple overlap events before the object is destroyed
            GetComponent<SpriteRenderer>().enabled = false;//Disable the sprite renderer (makes object invisible)
            m_collectibleUI.SetScore(m_scoreGiven);//Iterates the current score by the score given
            //I didn't use a current score variable because I may want to dynamically change the score given at certain points in the game
            //It would also have a different value for every instance of this object anyway
            if (!m_flashCooldown)//If the flash isn't on cooldown
            {
                m_flashCooldown = true;
                m_sun.color = new Color(0.5f, 0.5f, 0.5f, 1f);//Toggles the sun grey ~ red 1f, 0.04f, 0.08f
                StartCoroutine(LightFlash());//0.25 second delay before going black
            }
        }
    }

    private IEnumerator LightFlash()
    {
        yield return new WaitForSeconds(m_sunFlashTime);
        m_sun.color = Color.black;
        m_flashCooldown = false;
        Destroy(gameObject);//Destroyed once collected
    }
}
