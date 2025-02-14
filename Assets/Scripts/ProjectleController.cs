using UnityEngine;

public class ProjectleController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObj = collision.gameObject;

        if (gameObj.CompareTag("Enemy"))
        {
            gameObj.GetComponent<EnemyHealth>().GetEnemyController().Stun();
            Destroy(gameObject);
        }
        else if (!gameObj.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}
