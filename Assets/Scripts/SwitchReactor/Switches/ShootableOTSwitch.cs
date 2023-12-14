using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableOTSwitch : OneTimeSwitch
{
    is
    protected override bool CollisionValid(Collision2D collision)
    {
        return base.CollisionValid(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CollisionValid(collision) && !isActivated)
        {
            isActivated = true;
            Debug.Log("switch activated");
            GetComponent<SpriteRenderer>().color = Color.green;    // TEMPORARY: just to show for rn may switch sprites or something in future
            switchActivated.Invoke();
        }
    }
}
