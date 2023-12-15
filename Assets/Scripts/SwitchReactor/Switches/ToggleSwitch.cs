using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SwitchReactor.Switches
{
    public class ToggleSwitch : SwitchBase
    {
        bool isActive = false;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

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
            if (CollisionValid(collision))
            {
                ActivateSwitch();
            }
        }
        private void OnTriggerEnter2D(Collider2D collider) //called when projectile collides
        {
            if (ColliderValid(collider))
            {
                ActivateSwitch();
            }
        }

        private void ActivateSwitch() 
        {
            audioSource.PlayOneShot(switchSFX);
            if (isActive)
            {
                isActive = false;
                Debug.Log("switch deactivated");
                GetComponent<SpriteRenderer>().color = Color.red;       // TEMPORARY: just to show for rn may switch sprites or something in future
                switchDeactivated.Invoke();
            }
            else
            {
                isActive = true;
                Debug.Log("switch activated");
                GetComponent<SpriteRenderer>().color = Color.green;     // TEMPORARY: just to show for rn may switch sprites or something in future
                switchActivated.Invoke();
            }
        }

    }
}