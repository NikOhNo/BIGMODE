using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private float jumpForce = 5f;
    //[SerializeField]
    //private float smoothInputSpeed = .2f;
    [SerializeField]
    private bool isMage = true;

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
    }
    public void Attack(CallbackContext context) 
    {
        //may also want to have a condition where the player needs to wait at least "x" seconds before doing another attack, say .5 or .4
        if (isMage) 
        {
            //play spell attack animation and sfx
            Debug.Log("Spell basic attack");
        }
        else 
        {
            //play sword attack animation and sfx
            Debug.Log("Sword basic attack");
        }
        //Note: still need to integrate the two switch attacks into this logic

    }
    public void Move(CallbackContext context) 
    { //.perform, .press, .release
        Debug.Log(context);
        moveInput = context.ReadValue<Vector2>();
    }
    public void Jump(CallbackContext context)
    {
        Debug.Log(context);
    
        if (context.performed && feetCollider.IsTouchingLayers()) //if touching any layers
        {
            // Apply upward force to make the character jump
            myRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Switch(CallbackContext context)
    {
        Debug.Log(context);
        if (context.performed)
        {
            isMage ^= true; //true becomes false; false becomes true
        }
    }


}
