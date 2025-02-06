using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform m_player;
    public float m_speed;
    public float m_stoppingDistance;
    private bool m_playerInSight;

    enum EnemyStates
    {
        Idle,
        MovingToPlayer,
        Attack
    }
    EnemyStates m_enemyStates;

    void Start()
    {
        m_player = FindFirstObjectByType<TopDownCharacterController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerInSight && Vector2.Distance(transform.position, m_player.position) >= m_stoppingDistance)
        {
            m_enemyStates = EnemyStates.MovingToPlayer; 
            transform.position = Vector2.MoveTowards(transform.position, m_player.position, m_speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, m_player.position) == m_stoppingDistance) 
        {
            m_enemyStates = EnemyStates.Attack;
        }
        else
        {
            m_enemyStates = EnemyStates.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.name == "Character") 
        {
            m_playerInSight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.name == "Character") 
        {
            m_playerInSight = false;
        }
    }
}