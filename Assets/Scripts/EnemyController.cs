using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyController : MonoBehaviour
{
    //[SerializeField]
    //private GameObject basicEnemyHitbox;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private Vector2 attackPoint;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float maxVisionDistance = 10f;
    [SerializeField]
    private float atkRange = 2f;
    [SerializeField]
    private float hitRange = 1f;
    [SerializeField]
    private float atkPositionX = 1f;
    [SerializeField]
    private float atkDowntime = .5f;
    [SerializeField]
    private float queueTime = 1f;
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private bool isAttacking = false;


    private Rigidbody2D myRb;
    public Transform player;
    private Dictionary<bool, int> boolToInt = new Dictionary<bool, int> { { false, -1 }, { true, 1 } };

    private void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
        attackPoint.x = myRb.position.x + hitRange;
        attackPoint.y = myRb.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(myRb.position, player.transform.position);
        if (distance < atkRange) 
        {
            myRb.velocity = new Vector2(0f, myRb.velocity.y);
            if (!isAttacking)
            {
                isAttacking = true;
                QueueAttack();
                Invoke("Attack", queueTime);
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
        float direction = mySign(player.transform.position.x - myRb.position.x); 
        Vector2 newVelocity = new Vector2(moveSpeed * direction, myRb.velocity.y);

        // Update myRb.velocity
        myRb.velocity = newVelocity;
        float absVelocityX = Mathf.Abs(newVelocity.x);
        //animator.SetFloat("Speed", absVelocityX);
        //if the sprite is moving make the sprite face that direction
        //otherwise keep the sprite facing in the direction we moved last
        if (absVelocityX > 0.01)
        {
            //sprite.flipX = newVelocity.x > 0;
            direction = boolToInt[newVelocity.x > 0];
            attackPoint.x = myRb.position.x + direction * atkPositionX;
            attackPoint.y = myRb.position.y;
        }
    }

    void QueueAttack()
    {
        ParticleSystem part = this.GetComponent<ParticleSystem>();
        part.Play();
        Debug.Log("Enemy Preparing Attack!");
    }


    void Attack()
    {
        Debug.Log("Enemy Attacking Player!");
        int damageValue = 1;
        // Currently set up to deal damage for a single frame
        Collider2D[] playerHit = Physics2D.OverlapCircleAll(attackPoint, hitRange, playerLayer);
        foreach(Collider2D player in playerHit)
        {
            if (player.GetComponent<PlayerController>() != null)
            {
                player.GetComponent<PlayerController>().Hurt(damageValue);
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
}
