using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{

    //-- SERIALIZED FIELDS
    //Large Structures
    [SerializeField]
    private AudioClip jumpSFX;
    [SerializeField]
    private Canvas playerUI;
    [SerializeField]
    private BoxCollider2D feetCollider;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private LayerMask enemyLayers;
    [SerializeField]
    private GameObject projectilePrefab;
    //floats
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float jumpForce = 7.5f;
    [SerializeField]
    private float atkTimeStart = 0f;
    [SerializeField]
    private float switchWindow = 1f; //tbd may change
    [SerializeField]
    private float atkDelay = 0.5f; //tbd may change
    [SerializeField]
    private float jumpTimer = -5f; //initialized as negative to prevent jumping at frame 0
    [SerializeField]
    private float maxJumpTime = 0.2f;
    [SerializeField]
    private float basicMeleeRange = 1f;
    [SerializeField]
    private float attackPositionX = 1f;
    [SerializeField]
    private float switchMeleeRange = 1f;
    //integers
    [SerializeField]
    private int health = 5;
    [SerializeField]
    private int maxHealth = 5;
    [SerializeField]
    private int recoveryCurrency = 0; //think soul from hollow knight, we can decide on a name later
    [SerializeField]
    private int maxRecoveryCurrency = 100;
    //bools
    [SerializeField]
    private bool isAttacking = false;
    [SerializeField]
    private bool isMage = true;
    [SerializeField]
    private bool isSwitchAtk = false;


    //-- PROPERTIES
    private SpriteRenderer sprite;
    private Animator animator;
    private Rigidbody2D myRb;
    private Vector2 moveInput;
    private Dictionary<bool, int> boolToInt = new Dictionary<bool, int> { { false, -1 }, {true, 1} };
    private float direction = -1f;
    private AudioSource audioSource;


    //Happens when gathering values before Start
    private void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GetComponent<PlayerInput>().actions.Enable();

        if (!Mouse.current.enabled)
        {
            InputSystem.EnableDevice(Mouse.current);
        }
        if (!Keyboard.current.enabled)
        {
            InputSystem.EnableDevice(Keyboard.current);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //I am considering moving all of the code in between the ----- to the move function since we'd call less things in update 
        //and we no longer use Time.delta
        //-----------------------------------------------------------------------------------
        Vector2 newVelocity = new Vector2(moveInput.x * speed, myRb.velocity.y);
        // if running backwards flip the animation 
        // Update myRb.velocity
        myRb.velocity = newVelocity;
        float absVelocityX = Mathf.Abs(newVelocity.x);
        animator.SetFloat("Speed", absVelocityX);
        //if the sprite is moving make the sprite face that direction
        //otherwise keep the sprite facing in the direction we moved last
        if (absVelocityX > 0.01) 
        {
            sprite.flipX = newVelocity.x > 0;
            direction = boolToInt[newVelocity.x > 0];
            attackPoint.position = new Vector2(myRb.position.x + direction * attackPositionX, attackPoint.position.y);
        }
        //------------------------------------------------------------------------------------

        if (Time.time - atkTimeStart > atkDelay) //only allows us to start attacking again after atkDelay
        {
            isAttacking = false;
        }
        
        if (Time.time - jumpTimer < maxJumpTime) //jump is held
        {
            //needed to get rid of AddForce() call to fix bug

            myRb.velocity = new Vector2(myRb.velocity.x, jumpForce);
        }
    }
    public void Attack(CallbackContext context)
    {
        if (!isAttacking) 
        { 
            atkTimeStart = Time.time;
            isAttacking = true;
            int damageValue = 0;
            //if the attack hits an enemy we want to gain some recoveryCurrency
            //we can also decide how much we should get for each attack
            if (isMage) //projectile attacks
            {
                //TODO: make projectiles
                if (isSwitchAtk) //whether we attacked (switched) within switchWindow
                {
                    //play spell attack animation and sfx
                    Debug.Log("Spell switch attack");
                    GameObject spell = Instantiate(projectilePrefab, new Vector3(attackPoint.position.x, myRb.position.y, 0), Quaternion.identity);
                    damageValue = 1;
                    spell.GetComponent<PlayerProjectile>().Init(direction, damageValue);
                }
                else
                {
                    //play spell attack animation and sfx
                    Debug.Log("Spell basic attack");
                    GameObject spell = Instantiate(projectilePrefab, new Vector3(attackPoint.position.x, myRb.position.y, 0), projectilePrefab.transform.rotation);
                    damageValue = 1;
                    spell.GetComponent<PlayerProjectile>().Init(direction, damageValue);
                }
            }
            else
            {
                Collider2D[] enemiesHit = { };
                if (isSwitchAtk)
                {
                    //play spell attack animation and sfx
                    Debug.Log("Sword switch attack");
                    enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, switchMeleeRange, enemyLayers);
                    damageValue = 2;
                }
                else
                {
                    //play sword attack animation and sfx
                    Debug.Log("Sword basic attack");
                    enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, basicMeleeRange, enemyLayers);
                    damageValue = 1;
                }
                foreach (Collider2D enemy in enemiesHit)
                {
                    if (enemy.GetComponent<EnemyController>() != null) 
                    { 
                        enemy.GetComponent<EnemyController>().Hurt(damageValue);
                        recoveryCurrency += 10;
                    }
                }
            }
            isSwitchAtk = false;
        }
    }
    public void Move(CallbackContext context) 
    { //.perform, .press, .release
        //Debug.Log(context);

        moveInput = context.ReadValue<Vector2>();
        if (moveInput.x != 0)
        {
            //flip character along horizontal axis
            //In gd script this would be: $AnimatedSprite2D.flip_h = moveInput.x < 0
        }
    }
    public void Jump(CallbackContext context)
    {
        if (context.performed && feetCollider.IsTouchingLayers()) //if touching any layers
        {
            jumpTimer = Time.time;
            audioSource.PlayOneShot(jumpSFX);
            //AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
            // Apply upward force to make the character jump
            //myRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //now done in update() to allow for hold jump to be higher than tap
        }
        else if (context.canceled)
        {
            jumpTimer = -5;
        }
    }

    public void Switch(CallbackContext context)
    {
        //Debug.Log(context); //removed so seeing whether switch attacks or basic attacks are performed in debug log is easier
        if (context.performed && !isAttacking)
        {
            isMage ^= true; //true becomes false; false becomes true
            if (Time.time - atkTimeStart <= switchWindow) //if within switch window, do switch attack
            {
                isSwitchAtk = true;
                Attack(context);
            }
        }
    }

    public void Recover()
    {
        if (recoveryCurrency >= maxRecoveryCurrency)
        {
            //show recovery particle efx (and sfx?)
            health++;
            recoveryCurrency = 0;
        }
    }

    public void Hurt(int damage) 
    {
        //play hurt animation/sfx
        //consider dealing knockback to player
        health -= damage;
        playerUI.GetComponent<UIController>().ChangeHealth(health);
        if (health < 1)
        {
            Death();
        }
    }

    public void Death()
    {
        //play death animation/sfx
        //goto room with last checkpoint
        Debug.Log("You died!");
        health = maxHealth;
    }

    //you can uncomment this script to see the hitbox for the Melee attacks
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, basicMeleeRange);
    }

 

}
