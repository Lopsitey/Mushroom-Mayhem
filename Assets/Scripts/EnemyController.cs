using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Transform m_player;
    [SerializeField] public float m_speed;
    [SerializeField] public float m_stoppingDistance;

    private bool m_playerInSight;
    private SpriteRenderer m_spriteRenderer;
    NavMeshAgent m_agent;

    enum EnemyStates
    {
        Idle,
        MovingToPlayer,
        Attack
    }
    EnemyStates m_enemyStates;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_player = FindFirstObjectByType<TopDownCharacterController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //while the current distance between the player and this object's transform is greater than the stopping distance
        if (m_playerInSight && Vector2.Distance(transform.position, m_player.position) > m_stoppingDistance)
        {
            m_enemyStates = EnemyStates.MovingToPlayer;//switches to moving state if moving has begun
            m_agent.SetDestination(m_player.position);

            if (m_agent.velocity.x > 0)//flips the sprite in the left or right direction so it's always facing the player
            {
                m_spriteRenderer.flipX = true; // Facing right
            }
            else if (m_agent.velocity.x < 0)
            {
                m_spriteRenderer.flipX = false; // Facing left
            }
        }
        else if (Vector2.Distance(transform.position, m_player.position) == m_stoppingDistance)//if the stopping distance has been met then an attack can occur
        {
            m_enemyStates = EnemyStates.Attack;
        }
        else//if an attack is not occurring and the enemy isn't moving then it must be idle
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
