using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    public int Health = 3;
    [SerializeField]
    private float maxVisionDistance = 10f;
    [SerializeField]
    private float atkRange = 2f;


    private Rigidbody2D myRb;
    public Transform player;


    
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
        if (Health < 1)
        {
            //maybe award exp/currency here
            //death animation/sfx
            Debug.Log("Enemy Slain!");
            Destroy(gameObject); //destroy self
        }
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < atkRange) 
        {
            myRb.velocity = new Vector2(0f, myRb.velocity.y);
            Attack();
        }
        else if (distance < maxVisionDistance)
        {
            FollowPlayer();
        }
    }

    float mySign(float number) //returns -1, 0, or 1
    {
        if (number > 0.01f || number < -0.01f) //if number is not zero within floating point error
            return Mathf.Sign(number); 
        return 0;
    }

    void FollowPlayer()
    {
        float direction = mySign(player.transform.position.x - transform.position.x); 
        Vector2 newVelocity = new Vector2(moveSpeed * direction, myRb.velocity.y);

        // Update myRb.velocity
        myRb.velocity = newVelocity;
        
    } 

    void Attack()
    {
        Debug.Log("Enemy Attacking Player!");
    }
}
