using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    private PlayerController controller;
    private PlayerHealthSystem healthSystem;
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float lifetime = 3f;
    [SerializeField]
    private float acceleration = 0;
    private float direction = 0f; //-1,1
    private int damageValue = 0;


    // Update is called once per frame
    void Update()
    {
        speed += acceleration * Time.deltaTime;
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Init(PlayerController controller, float dir, int damage, float speedX = -1, float accelerationX = 0, float lifetimeX = -1)//call this after Instantiating to set direction and damage
    {
        this.healthSystem = controller.HealthSystem;
        direction = dir;
        damageValue = damage;
        if (speedX >= 0)
        {
            speed = speedX;
        }
        if (accelerationX > 0.01 || accelerationX < -0.01) //if nonzero
        {
            acceleration = accelerationX;
        }
        if (lifetimeX > 0.01)
        {
            lifetime = lifetimeX;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>() != null)
        {
            collision.GetComponent<EnemyController>().Hurt(damageValue);
            healthSystem.AddRecoveryCurrency(controller.PlayerSettings.RangedRecovery);

            Destroy(gameObject);
        }
    }
}
