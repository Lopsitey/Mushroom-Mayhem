using UnityEngine;

public class ProjectleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObj = collision.gameObject;
        if (!gameObj.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}
