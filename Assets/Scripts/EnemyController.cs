using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] public Transform m_player;
    [SerializeField] public float m_speed;
    [SerializeField] public float m_stoppingDistance;

    [Header("Damage Parameters")]
    [SerializeField] private int m_attackDamage = 25;
    [SerializeField] private float m_attackSpeed = 2f;
    [SerializeField] private float m_stunTime = 1.5f;
    [SerializeField] private PlayerHealth m_playerHealth;

    private SpriteRenderer m_spriteRenderer;
    NavMeshAgent m_agent;
    private bool m_attacking = false;

    enum EnemyStates
    {
        Idle,
        MovingToPlayer,
        Attack,
        Stunned
    }
    EnemyStates m_enemyStates;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_player = FindFirstObjectByType<TopDownCharacterController>().transform;
        m_enemyStates = EnemyStates.Idle;//Idle by default
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is in sight, is outside of the stopping distance and the enemy is not stunned
        if (Vector2.Distance(transform.position, m_player.position) > m_stoppingDistance && m_enemyStates == EnemyStates.MovingToPlayer)
        {
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
        //If the stopping distance has been met then an attack can occur
        else if (Vector2.Distance(transform.position, m_player.position) <= m_stoppingDistance && m_enemyStates == EnemyStates.MovingToPlayer)
        {
            m_enemyStates = EnemyStates.Attack;
        }

        switch (m_enemyStates)
        {
            case EnemyStates.Idle:
                // Just standing around...
                break;

            case EnemyStates.MovingToPlayer:
                break;

            case EnemyStates.Attack:
                AttackPlayer();
                break;

            case EnemyStates.Stunned:
                // Do nothing while stunned (controlled by Coroutine)
                break;
        }
    }

    void AttackPlayer()
    {
        if (!m_attacking)//If the attack isn't on cooldown
        {
            if (!m_playerHealth.GetTopDown().GetRolling())//If the player isn't rolling
            {
                m_attacking = true;
                m_playerHealth.TakeDamage(m_attackDamage);
                StartCoroutine(AttackDelay());//Delay for next attack
            }
            else
            {
                StartCoroutine(AttackDelay());//Waits before checking again
            }
        }
    }

    public void Stun()
    {
        m_enemyStates = EnemyStates.Stunned;
        m_agent.isStopped = true;//Stops movement immediately
        m_agent.velocity = Vector3.zero;//Ensures no movement happens
        StartCoroutine(StunTimer());//Delay for stunning
    }

    IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(m_stunTime);//Stunned for one and a half seconds by default
        m_agent.isStopped = false;//Resumes movement
        m_enemyStates = EnemyStates.MovingToPlayer;//Resume chasing after stun
    }

    IEnumerator AttackDelay() 
    {
        yield return new WaitForSeconds(m_attackSpeed);//Attacks once every second by default
        m_attacking = false;
        m_enemyStates = EnemyStates.MovingToPlayer;//Resume chasing after attacking
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            m_enemyStates = EnemyStates.MovingToPlayer;//Switches to moving state if the player is detected
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Character")
        {
            m_enemyStates = EnemyStates.Idle;
        }
    }
}
