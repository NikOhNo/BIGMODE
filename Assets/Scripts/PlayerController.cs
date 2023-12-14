using Assets.Scripts;
using Assets.Scripts.PlayerStates;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    //-- SERIALIZED FIELDS
    [SerializeField]
    private PlayerSettingsSO playerSettings;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private Canvas playerUI;
    [SerializeField]
    private Collider2D feetCollider;

    //-- PROPERTIES
    public UnityEvent OnJump = new();
    public PlayerHealthSystem HealthSystem => healthSystem;
    public SpellShooter SpellShooter => spellShooter;
    public PlayerSettingsSO PlayerSettings => playerSettings;
    public UIController UIController => uiController;
    public Vector3 AttackPoint => CalculateFacingAttackPoint();
    public Vector2 MoveInput => moveInput;
    public Collider2D FeetCollider => feetCollider;
    public SpriteRenderer Sprite { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public ParticleSystem Pfx { get; private set; }

    //-- STATE PROPERTIES
    public MageState MageState { get; private set; } = new();
    public MeleeState MeleeState { get; private set; } = new();
    public HurtState HurtState { get; private set; } = new();

    //-- FIELDS
    private Vector2 moveInput;
    private UIController uiController;
    private IState currState;
    private PlayerHealthSystem healthSystem;
    private SpellShooter spellShooter;

    //Happens when gathering values before Start
    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        Pfx = GetComponent<ParticleSystem>();
        uiController = playerUI.GetComponent<UIController>();
        spellShooter = GetComponent<SpellShooter>();
        healthSystem = new(this);

        EnableInput();

        SwitchState(MageState);
    }

    private void EnableInput()
    {
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

    // Update is called once per frame
    void Update()
    {
        currState.PerformState();
    }

    public void SwitchState(IState newState)
    {
        currState?.ExitState();
        currState = newState;
        currState.EnterState(this);
    }

    public void Move(CallbackContext context) 
    { //.perform, .press, .release
        //Debug.Log(context);
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(CallbackContext context)
    {
        if (context.performed)
        {
            if (currState is PlayableState playableState)
            {
                playableState.PerformJump();
            }

            OnJump.Invoke();
        }
        else if (context.canceled)
        {
            if (currState is PlayableState playableState)
            {
                playableState.CancelJump();
            }
        }
    }

    public void Attack(CallbackContext context)
    {
        if (context.performed)
        {
            if (currState is PlayableState playState)
            {
                playState.PerformAttack();
            }
        }
    }

    public void Switch(CallbackContext context)
    {
        //Debug.Log(context); //removed so seeing whether switch attacks or basic attacks are performed in debug log is easier
        if (context.performed)
        {
            if (currState is PlayableState playState)
            {
                playState.SwitchState();
            }
        }
    }

    private Vector3 CalculateFacingAttackPoint()
    {
        float xSpawnPos = attackPoint.position.x;
        if (Sprite.flipX)
        {
            xSpawnPos += 2 * (attackPoint.parent.position.x - attackPoint.position.x);
        }

        return new Vector3(xSpawnPos, attackPoint.position.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(playerSettings.EnemyLayers.value);
        //Debug.Log(collision.gameObject.layer);
        //if (playerSettings.EnemyLayers.value == collision.gameObject.layer) //playerSettings.EnemyLayers.value = 64, collision.gameObject.layer = layerNum in Inspector
        if (collision.gameObject.layer == 6) //6 == Enemy1 (Layer)
        {
            HealthSystem.Hurt(1);
            //SwitchState(HurtState); //would ideally like to put this in hurt function of health system, but I can't use PlayerController.HurtState there 
        }
    }
}

//you can uncomment this script to see the hitbox for the Melee attacks
/*
private void OnDrawGizmosSelected()
{
    if (attackPoint == null)
        return;

    Gizmos.DrawWireSphere(attackPoint.position, basicMeleeRange);
}
*/