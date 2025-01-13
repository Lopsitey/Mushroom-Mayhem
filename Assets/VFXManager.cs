using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject m_explosionPrefab;
    public static VFXManager s_instance;
    private void Awake()
    {
        if (s_instance == null) 
        {
            s_instance = this;
        }
    }
    ///<summary>
    ///Creates an explosion at a given point and destroys it after the specified time
    ///</summary>
    ///<param name="position">Where the explosion will be spawned</param>
    ///<param name="destroyAfter">How long it will take to destroy the explosion</param>
    ///<returns>A reference to the explosion GameObject that was spawned</returns>
    public static GameObject CreateExplosion(Vector3 position, float destroyAfter = 0.5f) 
    {
        if (s_instance == null) 
        {
            Debug.LogError("Tried to spawn an explosion but the instance hasn't been set.");//checks if an instance has been set
        }
        //Spawns ab explosion
        GameObject explosion = Instantiate(s_instance.m_explosionPrefab, position, Quaternion.identity);
        //Destroy the explosion after the specified time
        Destroy(explosion, destroyAfter);
        //returns a reference to the explosion
        return explosion;
    }
}
