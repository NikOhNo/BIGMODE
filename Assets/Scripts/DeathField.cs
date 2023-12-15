using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathField : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.GetComponent<PlayerController>() != null)
        {
            collider.gameObject.GetComponent<PlayerController>().Die(); 
        }
        else
        { 
            Destroy(collider.gameObject); 
        }
    }
}
