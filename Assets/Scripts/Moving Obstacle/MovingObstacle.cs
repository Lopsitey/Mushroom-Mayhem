using System.Runtime.CompilerServices;
using UnityEngine;
public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Transform m_startWaypoint;
    [SerializeField] private Transform m_endWaypoint;
    [SerializeField] private float m_speed = 5;
    [SerializeField] private Sprite[] m_sprites; //array of sprites to cycle through

    private Transform m_target;
    private int currentSpriteIndex = 0;
    private SpriteRenderer spriteRenderer;
    //private Rigidbody2D m_rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (m_sprites.Length > 0)
        {
            spriteRenderer.sprite = m_sprites[0];
            InvokeRepeating("NextSprite", 0.5f, 0.5f); // Change sprite every 0.5 seconds
        }
        m_target = m_startWaypoint;
    }
    
    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 StartPosition;
        Vector3 EndPosition;
        float TimeElapsed;
        Vector3 SomewhereBetween = Vector3.Lerp(StartPosition, EndPosition, TimeElapsed);
        transform.position = SomewhereBetween;
        */
        transform.position = Vector2.MoveTowards(transform.position, m_target.position, m_speed * Time.deltaTime);
        /*
        get vector to target
        Vector2 direction = m_target.position - transform.position;
        apply the movement in that direction
        transform.Translate(direction * m_speed * Time.deltaTime);
        */
        /*
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_rigidbody.linearVelocity = direction.normalized * m_speed;
        could also be m_rigidbody.AddForce(direction.normalized * m_speed);
        */
        /*
        Vector2 position = transform.position;
        position.y = transform.position.y + 0.1f;// * Time.deltaTime;
        transform.position = position;
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingObstacleWaypoint")) 
        {
            ChangeTarget();
        }
    }
    void ChangeTarget() 
    {
        if (m_target == m_startWaypoint)//checks what direction the ball is heading in and sets to the reverse direction
        {
            m_target = m_endWaypoint;
        }
        else 
        {
            m_target = m_startWaypoint;
        }
    }
    void NextSprite()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % m_sprites.Length;
        spriteRenderer.sprite = m_sprites[currentSpriteIndex];
    }
}