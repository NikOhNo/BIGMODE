using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float lifetime = 3f;
    private float direction = 0f; //-1,1
    private int damageValue = 0;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Init(float dir, int damage) //call this after Instantiating to set direction and damage
    {
        direction = dir;
        damageValue = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>() != null)
        {
            collision.GetComponent<EnemyController>().Hurt(damageValue);
            Destroy(gameObject);
        }
    }
}
