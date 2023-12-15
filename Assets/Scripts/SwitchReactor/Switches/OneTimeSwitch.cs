using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeSwitch : SwitchBase
{
    bool isActivated = false;   // this switch can only be activated once and will stay active

    protected override bool CollisionValid(Collision2D collision)
    {
        return base.CollisionValid(collision);
    }

    protected override bool ColliderValid(Collider2D collider)
    {
        return base.ColliderValid(collider);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (CollisionValid(collision) && !isActivated)
        {
            ActivateSwitch();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) //called when projectile collides
    {
        if (ColliderValid(collider) && !isActivated)
        {
            ActivateSwitch();
        }
    }

    private void ActivateSwitch()
    {
        isActivated = true;
        Debug.Log("switch activated");
        GetComponent<SpriteRenderer>().color = Color.green;    // TEMPORARY: just to show for rn may switch sprites or something in future
        switchActivated.Invoke();
    }
}
