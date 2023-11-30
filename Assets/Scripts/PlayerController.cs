using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{



    //-- SERIALIZED FIELDS
    //[SerializeField]
    //private PlayerSettings playerSettings;
    [SerializeField]
    private BoxCollider2D feetCollider;
    [SerializeField] 
    private float speed = 5f;
    [SerializeField]
    private float jumpForce = 7.5f;
    //[SerializeField]
    //private float smoothInputSpeed = .2f;
    [SerializeField]
    private float atkTimeStart = 0f;
    [SerializeField]
    private float switchWindow = 1f; //tbd may change
    [SerializeField]
    private float atkDelay = 0.5f; //tbd may change
    [SerializeField]
    private float jumpTimer = -5f; 
    [SerializeField]
    private float maxJumpTime = 0.1f;
    [SerializeField]
    private bool isAttacking = false;
    [SerializeField]
    private bool isMage = true;
    [SerializeField]
    private bool isSwitchAtk = false;
    [SerializeField]
    private int health = 5;
    [SerializeField]
    private int maxHealth = 5;
    [SerializeField]
    private int recoveryCurrency = 0; //think soul from hollow knight, we can decide on a name later
    [SerializeField]
    private int maxRecoveryCurrency = 100;


    //-- PROPERTIES
    //public PlayerSettings PlayerSettings => playerSettings;
    public Vector2 MoveInput = new();
    public Vector2 SmoothedInputVector = new();

    private Vector2 smoothInputVelocity;

    private PlayerController controller;
    private Rigidbody2D myRb;
    private Vector2 moveInput;

    //Happens when gathering values before Start
    private void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newVelocity = new Vector2(moveInput.x * speed, myRb.velocity.y);
        // if running backwards flip the animation 
        // Update myRb.velocity
        myRb.velocity = newVelocity;


        if (Time.time - atkTimeStart > atkDelay) //only allows us to start attacking again after atkDelay
        {
            isAttacking = false;
        }
        
        if (Time.time - jumpTimer < maxJumpTime) //jump is held
        {
            myRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    public void Attack(CallbackContext context)
    {
        //Debug.Log(context); //removed so seeing whether switch attacks or basic attacks are performed in debug log is easier
        if (!isAttacking) 
        { 
            atkTimeStart = Time.time;
            isAttacking = true;
            //if the attack hits an enemy we want to gain some recoveryCurrency
            //we can also decide how much we should get for each attack
            if (isMage)
            {
                if (isSwitchAtk) //whether we attacked (switched) within switchWindow
                {
                    //play spell attack animation and sfx
                    Debug.Log("Spell switch attack");
                }
                else
                {
                    //play spell attack animation and sfx
                    Debug.Log("Spell basic attack");
                }
            }
            else
            {
                if (isSwitchAtk)
                {
                    //play spell attack animation and sfx
                    Debug.Log("Sword switch attack");
                }
                else
                {
                    //play sword attack animation and sfx
                    Debug.Log("Sword basic attack");
                }
            }
            isSwitchAtk = false;
        }
    }
    public void Move(CallbackContext context) 
    { //.perform, .press, .release
        Debug.Log(context);
        moveInput = context.ReadValue<Vector2>();
    }
    public void Jump(CallbackContext context)
    {
        if (context.performed && feetCollider.IsTouchingLayers()) //if touching any layers
        {
            jumpTimer = Time.time;
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


}
