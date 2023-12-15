using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private Vector2 attackPoint;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float maxVisionDistance = 10f;
    [SerializeField]
    private float atkRange = 2f; //when we try to stop to attack player
    [SerializeField]
    private float hitRange = 1f; //radius of hit circle
    [SerializeField]
    private float atkPositionX = 1f; //position of hit circle
    [SerializeField]
    private float atkDowntime = .5f;
    [SerializeField]
    private float queueTime = 1f;
    [SerializeField]
    private float attackTime = .714f; //1-(2/7)
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private bool isAttacking = false;
    [SerializeField]
    private int EnemyID = 1; //Currently 1 is for Lily, 2 is for FMN

    private float direction = 0f;
    private SpriteRenderer sprite;
    private Animator animator;
    private Rigidbody2D myRb;
    public Transform player;
    private Dictionary<bool, int> boolToInt = new Dictionary<bool, int> { { false, -1 }, { true, 1 } };
    private ParticleSystem part;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        attackPoint.x = myRb.position.x + atkPositionX;
        attackPoint.y = myRb.position.y;
        part = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        direction = mySign(player.transform.position.x - myRb.position.x);
    }
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(myRb.position, player.transform.position);
        float realDirection = mySign(player.transform.position.x - myRb.position.x);
        if ((distance < atkRange && direction == realDirection)|| isAttacking) //attack when facing player and close, don't move while attacking
        {
            myRb.velocity = new Vector2(0f, myRb.velocity.y);
            if (!isAttacking)
            {
                isAttacking = true;
                QueueAttack();
                Invoke("Attack", queueTime);
                if (EnemyID == 1) { //only want Play Attack for Lily
                    Invoke("PlayAttack", attackTime);
                }
            }
        }
        else if (distance < maxVisionDistance)
        {
            FollowPlayer();
        }
        /*
        else
        {
            //maybe patrol??
        }
        */
    }

    float mySign(float number) //returns -1, 0, or 1
    {
        if (number > 0.01f || number < -0.01f) //if number is not zero within floating point error
            return Mathf.Sign(number); 
        return 0;
    }

    void FollowPlayer()
    {
        direction = mySign(player.transform.position.x - myRb.position.x); 
        Vector2 newVelocity = new Vector2(moveSpeed * direction, myRb.velocity.y);

        // Update myRb.velocity
        myRb.velocity = newVelocity;
        float absVelocityX = Mathf.Abs(newVelocity.x);
        animator.SetFloat("speed", absVelocityX);
        //if the sprite is moving make the sprite face that direction
        //otherwise keep the sprite facing in the direction we moved last
        if (absVelocityX > 0.01)
        {
            sprite.flipX = newVelocity.x > 0;
            direction = boolToInt[newVelocity.x > 0];
            attackPoint.x = myRb.position.x + direction * atkPositionX;
            attackPoint.y = myRb.position.y;
        }
    }

    void QueueAttack()
    {
        part.Play();
        if (EnemyID == 2) { //If FMN queueAttack
            animator.SetTrigger("queueAttack");
        }
        Debug.Log("Enemy Preparing Attack!");
    }

    void PlayAttack()
    {
        animator.SetTrigger("attack");
    }


    void Attack()
    {
        Debug.Log("Enemy Attacking Player!");
        if (EnemyID == 2) {
            animator.SetTrigger("attack");
        }
        int damageValue = 1;
        // Currently set up to deal damage for a single frame
        Collider2D[] playerHit = Physics2D.OverlapCircleAll(attackPoint, hitRange, playerLayer);
        foreach(Collider2D player in playerHit)
        {
            if (player.GetComponent<PlayerController>() != null)
            {
                PlayerController controller = player.GetComponent<PlayerController>();

                controller.HealthSystem.Hurt(damageValue);
            }
        }
        // If we end up creating a hitbox that lingers we may want to start with the code below instead
        // GameObject hitbox = Instantiate(basicEnemyHitbox, new Vector3(attackPoint.position.x, myRb.position.y, 0), Quaternion.identity);
        // hitbox.GetComponent<> //get name of script on hitbox and call the function that will assign damage to it
        Invoke("ResetAttack", atkDowntime); //way of keeping enemy from attacking repeatedly too fast
    }
    void ResetAttack()
    {
        isAttacking = false;
    }

    public void Hurt(int damage)
    {
        //play hurt sfx, vfx, maybe blood splatter or flash depending on game feel we want to go for
        health -= damage;

        if (health < 1) 
        {
            //maybe award exp/currency here
            //death animation/sfx
            Debug.Log("Enemy Slain!");
            Destroy(gameObject); //destroy self
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(new Vector3(attackPoint.x, attackPoint.y, 0), hitRange);
    }
    
}
