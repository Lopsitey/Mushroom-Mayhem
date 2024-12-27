using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class to control the top down character.
/// Implements the player controls for moving and shooting.
/// Updates the player animator so the character animates based on input.
/// </summary>
public class TopDownCharacterController : MonoBehaviour
{
    #region Framework Variables

    //The inputs that we need to retrieve from the input system.
    private InputAction m_moveAction;
    private InputAction m_attackAction;

    //The components that we need to edit to make the player move smoothly.
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    
    //The direction that the player is moving in.
    private Vector2 m_playerDirection;
   

    [Header("Movement parameters")]
    //The speed at which the player moves
    [SerializeField] private float m_playerSpeed = 200f;
    //The maximum speed the player can move
    [SerializeField] private float m_playerMaxSpeed = 1000f;

    #endregion

    /// <summary>
    /// When the script first initialises this gets called.
    /// Use this for grabbing components and setting up input bindings.
    /// </summary>
    [Header("Projectile parameters")]
    //Reference to the game obj
    [SerializeField] GameObject m_projectilePrefab;
    //Wgere the projectile will be spawned from
    [SerializeField] Transform m_firePoint;
    //How fast our projectile will travel
    [SerializeField] float m_projectileSpeed;
    //How fast the projectiles will fire
    [SerializeField] float m_fireRate;

    private float m_fireTimeout = 0;
    private Vector2 m_LastDirection;
    private void Awake()
    {
        //bind movement inputs to variables
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_attackAction = InputSystem.actions.FindAction("Attack");
        
        //get components from Character game object so that we can use them later.
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Called after Awake(), and is used to initialize variables e.g. set values on the player
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// When a fixed update loop is called, it runs at a constant rate, regardless of pc performance.
    /// This ensures that physics are calculated properly.
    /// </summary>
    private void FixedUpdate()
    {
        //clamp the speed to the maximum speed for if the speed has been changed in code.
        float speed = m_playerSpeed > m_playerMaxSpeed ? m_playerMaxSpeed : m_playerSpeed;
        
        //apply the movement to the character using the clamped speed value.
        m_rigidbody.linearVelocity = m_playerDirection * (speed * Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// When the update loop is called, it runs every frame.
    /// Therefore, this will run more or less frequently depending on performance.
    /// Used to catch changes in variables or input.
    /// </summary>
    void Update()
    {
        // store any movement inputs into m_playerDirection - this will be used in FixedUpdate to move the player.
        m_playerDirection = m_moveAction.ReadValue<Vector2>();
        
        // ~~ handle animator ~~
        // Update the animator speed to ensure that we revert to idle if the player doesn't move.
        m_animator.SetFloat("Speed", m_playerDirection.magnitude);
        
        // If there is movement, set the directional values to ensure the character is facing the way they are moving.
        if (m_playerDirection.magnitude > 0)
        {
            m_animator.SetFloat("Horizontal", m_playerDirection.x);
            m_animator.SetFloat("Vertical", m_playerDirection.y);

            m_LastDirection = m_playerDirection;//Sets the last facing direction of the player
        }

        // check if an attack has been triggered.
        if (m_attackAction.IsPressed() && Time.time > m_fireTimeout)//gives the bullets a delay between firing
        {
            // just log that an attack has been registered for now
            // we will look at how to do this in future sessions.
            Debug.Log("Attack!");

            m_fireTimeout = Time.time + m_fireRate;//adding the fire rate increases fire timeout more and more this which means the if statement has to wait for the delay
            //Debug.Log(Time.time);//increasing it forever is okay because it is being compared to time wich also always increases
            Fire();
        }
    }
    void Fire()//spawns a bullet from the location of the firepoint at the rotation of identity (null)
    {
        // Vector3 mousePointOnScreen = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 fireDirection = m_LastDirection;
        Quaternion bulletRotation = Quaternion.identity;

        //left right and idle directions
        if (fireDirection == Vector2.zero)
        {
            fireDirection = Vector2.down;
        }
        else if (fireDirection == Vector2.left)
        {
            bulletRotation = Quaternion.Euler(0, 0, 90f);//if the direction faced is left rotate the bullet 90 degrees left
        }
        else if (fireDirection == Vector2.right)
        {
            bulletRotation = Quaternion.Euler(0, 0, 270f);//the same for facing right but in reverse
        }

        //NE SE SW NW diagonal directions
        // Handle quadrant-based rotations
        if (fireDirection == new Vector2(-0.71f, 0.71f))//NW
        {
            // Up-Left: 135 degrees
            bulletRotation = Quaternion.Euler(0, 0, 135f);
            Debug.Log("UP Left");
        }
        else if (fireDirection == new Vector2(0.71f, 0.71f))//NE
        {
            // Up-Right: 45 degrees
            bulletRotation = Quaternion.Euler(0, 0, 45f);
        }
        else if (fireDirection == new Vector2(-0.71f, -0.71f))//SW
        {
            // Down-Left: 225 degrees
            bulletRotation = Quaternion.Euler(0, 0, 225f);
        }
        else if (fireDirection == new Vector2(0.71f, -0.71f))//SE
        {
            // Down-Right: 315 degrees (or 0 to 45 for covering the full circle)
            bulletRotation = Quaternion.Euler(0, 0, 315f);
        }
        Debug.Log(fireDirection);
        Debug.Log((Vector2.left / 2) - new Vector2(0.21f,0.21f));

        GameObject projectileToSpawn = Instantiate(m_projectilePrefab, m_firePoint.position, bulletRotation);//spawns a projectile using the prefab-object, firepoint and bullet rotation variables
        if (projectileToSpawn.GetComponent<Rigidbody2D>() != null)
        {
            projectileToSpawn.GetComponent<Rigidbody2D>().AddForce(fireDirection.normalized * m_projectileSpeed, ForceMode2D.Impulse); //adds force to the object's y axis in relation to its speed
            //the forcemode2d, type impulse is instantaneous as oppose to type force which is continuous
            //a gun would apply force once to a bullet so impulse seems more relevant on this occasion
        }
    }
}
