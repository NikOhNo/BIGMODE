using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SwitchReactor.Switches
{
    public class PressSwitch : SwitchBase
    {
        protected override bool CollisionValid(Collision2D collision)
        {
            return base.CollisionValid(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionValid(collision))
            {
                Debug.Log("switch activated");
                GetComponent<SpriteRenderer>().color = Color.yellow;    // TEMPORARY: just to show for rn may switch sprites or something in future
                switchActivated.Invoke();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (CollisionValid(collision))
            {
                Debug.Log("switch deactivated");    
                GetComponent<SpriteRenderer>().color = Color.red;       // TEMPORARY: just to show for rn may switch sprites or something in future
                switchDeactivated.Invoke();
            }
        }
    }
}