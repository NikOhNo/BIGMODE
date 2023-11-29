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
    private float jumpForce = 10f;
    //[SerializeField]
    //private float smoothInputSpeed = .2f;
    [SerializeField]
    private float atkTimeStart = 0f;
    [SerializeField]
    private float switchWindow = 1f; //tbd may change
    [SerializeField]
    private float atkDelay = 0.5f; //tbd may change
    [SerializeField]
    private float jumpTimer = 0f;
    [SerializeField]
    private float maxJumpTime = 0.25f;
    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private bool isAttacking = false;
    [SerializeField]
    private bool isMage = true;
    [SerializeField]
    private bool isSwitchAtk = false;
    public int jumpCounter = 0;


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
        //Debug.Log(jumpCounter++);

        if (context.performed && feetCollider.IsTouchingLayers()) //if touching any layers
        {
            jumpTimer = Time.time;
            // Apply upward force to make the character jump
            //myRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //now done in update() to allow for hold jump to be higher than tap
        }
        else if (context.canceled)
        {
            jumpTimer = 0;
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


}
