using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    private InputAction m_rollAction;
    private InputAction m_lookAction;

    //The components that we need to edit to make the player move smoothly.
    private Animator m_animator;
    private Rigidbody2D m_rigidBody;
    
    //The direction that the player is moving in.
    private Vector2 m_playerDirection;

    [Header("Movement Parameters")]
    //The speed at which the player moves
    [SerializeField] private float m_playerSpeed = 200f;
    //The maximum speed the player can move
    [SerializeField] private float m_playerMaxSpeed = 1000f;
    //How fast you can roll - a method of indirectly accessing the rollTimeout variable
    [SerializeField] float m_rollRate = 1f;

    #endregion

    /// <summary>
    /// When the script first initializes this gets called.
    /// Use this for grabbing components and setting up input bindings.
    /// </summary>
    [Header("Projectile Parameters")]
    //Reference to the game obj
    [SerializeField] GameObject m_projectilePrefab;
    //Where the projectile will be spawned from
    [SerializeField] Transform m_firePoint;
    //How fast our projectile will travel
    [SerializeField] float m_projectileSpeed;
    //How fast the projectiles will fire
    [SerializeField] float m_fireRate;

    [Header("Lighting Parameters")]
    [SerializeField] private Transform m_torchLight;
    [SerializeField] private float m_rotationSpeed;

    private float m_fireTimeout = 0;
    //Default to 0.0f, -1.0f which is facing down so if the player tries to fire a bullet without moving it will always have a valid direction
    private Vector2 m_LastDirection = new Vector2(0.0f, -1.00f);
    private Quaternion m_targetRotation;
    private bool m_move;
    private bool m_isRolling;

    [Header("Mask Parameters")]
    //The mask applied when the player receives damage
    [SerializeField] private GameObject m_maskCore;
    [SerializeField] private GameObject m_maskOuter;
    //The period of time the mask would be visible for
    [SerializeField] private float m_maskDuration = 1f;
    //The minimum and maximum sizes of the mask when growing
    [SerializeField] private float m_maskMinSize = 0.5f;
    [SerializeField] private float m_maskMaxSize = 0.3f;
    [SerializeField] private int m_maskStabCount = 3;

    private void Awake()
    {
        //bind movement inputs to variables
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_attackAction = InputSystem.actions.FindAction("Attack");
        m_rollAction = InputSystem.actions.FindAction("Jump");
        m_lookAction = InputSystem.actions.FindAction("Interact");

        //get components from Character game object so that we can use them later.
        m_animator = GetComponent<Animator>();
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Called after Awake(), and is used to initialize variables e.g. set values on the player
    /// </summary>
    void Start()
    {
        m_move = false;
        m_maskCore.gameObject.SetActive(false);//Hides the masks on start
        m_maskOuter.gameObject.SetActive(false);
    }

    /// <summary>
    /// When a fixed update loop is called, it runs at a constant rate, regardless of pc performance.
    /// This ensures that physics are calculated properly.
    /// </summary>
    private void FixedUpdate()
    {
        //clamp the speed to the maximum speed for if the speed has been changed in code.
        float speed = m_playerSpeed > m_playerMaxSpeed ? m_playerMaxSpeed : m_playerSpeed;

        if (!m_move)//only move if your not holding right click
        {
            speed = 0.0f;
        }
        //apply the movement to the character using the clamped speed value.
        m_rigidBody.linearVelocity = m_playerDirection * (speed * Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// When the update loop is called, it runs every frame.
    /// Therefore, this will run more or less frequently depending on performance.
    /// Used to catch changes in variables or input.
    /// </summary>
    void Update()
    {
        //Store any movement inputs into m_playerDirection - this will be used in FixedUpdate to move the player.
        if (m_lookAction.IsPressed())
        {
            Vector3 m_mousePosition = Input.mousePosition;//The mouse position
            Vector3 mousePointOnScreen = Camera.main.ScreenToWorldPoint(m_mousePosition);
            m_playerDirection = (mousePointOnScreen - m_torchLight.position).normalized;
            m_move = false;
        }
        else
        {
            m_playerDirection = m_moveAction.ReadValue<Vector2>();
            m_move = true;
        }
            // ~~ handle animator ~~
            // Update the animator speed to ensure that we revert to idle if the player doesn't move.
            m_animator.SetFloat("Speed", m_playerDirection.magnitude);

        if (m_playerDirection != m_LastDirection)
        {
            // If there is movement, set the directional values to ensure the character is facing the way they are moving.
            if (m_playerDirection.magnitude > 0)
            {
                m_animator.SetFloat("Horizontal", m_playerDirection.x);
                m_animator.SetFloat("Vertical", m_playerDirection.y);

                m_LastDirection = m_playerDirection;//Sets the last facing direction of the player
            }
            //Converts the x and y of m_playerDirection to polar coordinates, i.e. an angle
            float angle = Mathf.Atan2(m_playerDirection.y, m_playerDirection.x) * Mathf.Rad2Deg;
            //Sets the rotation to this angle on Z and doesn't update the angle unless there is a new valid input
            m_targetRotation = m_playerDirection.magnitude < 0.01f ? m_targetRotation : Quaternion.Euler(0, 0, angle - 90);
        }
        //smoothly rotates the light using the player's orientation
        m_torchLight.rotation = Quaternion.Slerp(m_torchLight.rotation, m_targetRotation, m_rotationSpeed * Time.deltaTime);

        // check if an attack has been triggered.
        if (m_attackAction.IsPressed() && Time.time > m_fireTimeout)//gives the bullets a delay between firing
        {
            m_fireTimeout = Time.time + m_fireRate;
            //adding the fire rate increases fire timeout more and more this which means the if statement has to wait for the delay
            //increasing m_fireTimeout forever is okay because it is being compared to time which also always increases
            Fire();
        }
        if (m_rollAction.IsPressed() && !m_isRolling)//Ensures multiple rolls can't be instigated simultaneously
        {
            //Set rolling state
            m_isRolling = true;

            //Store the speed
            float oldSpeed = m_playerSpeed;
            float newSpeed = m_playerSpeed * 2;

            //Set the speed and animation
            m_playerSpeed = newSpeed;
            m_animator.SetTrigger("Rolling");
            
            //Reset after roll duration
            StartCoroutine(EndRoll(oldSpeed));
        }
    }

    void Fire()//spawns a bullet from the location of the fire point at the rotation of identity (null)
    {
        GameObject projectileToSpawn = Instantiate(m_projectilePrefab, m_firePoint.position, m_targetRotation);//spawns a projectile using the prefab-object, fire point and target rotation variables
        if (projectileToSpawn.GetComponent<Rigidbody2D>() != null)
        {
            projectileToSpawn.GetComponent<Rigidbody2D>().AddForce(m_LastDirection.normalized * m_projectileSpeed, ForceMode2D.Impulse);//adds force to the object's y axis in relation to its speed
            //the forcemode2D, type impulse is instantaneous as oppose to type force which is continuous
            //a gun would apply force once to a bullet so impulse seems more relevant on this occasion
        }
    }

    public IEnumerator ShowDamageMask(int i)
    {
        m_maskCore.gameObject.SetActive(true);//Shows the mask
        m_maskOuter.gameObject.SetActive(true);

        LeanTween.scale(m_maskOuter, Vector3.one * (m_maskMinSize * 4), m_maskDuration / 4f)//Grow animation
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.scale(m_maskOuter, Vector3.one * (m_maskMaxSize * 2), m_maskDuration / 4f)//Shrink animation
                    .setEase(LeanTweenType.easeInQuad);
            });

        yield return new WaitForSeconds(m_maskDuration);
        
        m_maskCore.gameObject.SetActive(false);//Hides the mask after the duration
        m_maskOuter.gameObject.SetActive(false);

        if (++i < m_maskStabCount)
        {
            StartCoroutine(ShowDamageMask(i));
        }
    }

    private IEnumerator EndRoll(float oldSpeed) 
    {
        yield return new WaitForSeconds(m_rollRate);
        m_playerSpeed = oldSpeed;
        m_isRolling = false;
    }

    public bool GetRolling()//Gets the state of roll for the enemy to decide if the attack will deal damage to the player
    {
        return m_isRolling;
    }
}
