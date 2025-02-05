using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform m_player;
    public float m_speed;

    void Start()
    {
        m_player = FindFirstObjectByType<TopDownCharacterController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_player.position, m_speed * Time.deltaTime);
    }
}
