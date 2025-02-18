using UnityEngine;

public class DestroyOnLeavingCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible() //presumably if the object is out of the field private void this automatically gets called?
    {
        Destroy(gameObject);
    }
}
