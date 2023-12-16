using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.PlayerStates
{
    public abstract class PlayableState : StateBase
    {
        protected Dictionary<bool, int> boolToInt = new Dictionary<bool, int> { { false, -1 }, { true, 1 } };
        protected bool isAttacking = false;

        private float jumpTimer = -5f; //initialized as negative to prevent jumping at frame 0
        protected float atkTimeStart = -Mathf.Infinity;

        //-- Cached References
        private Vector2 myVelocity;
        private Rigidbody2D myRb;
        private Animator myAnimator;
        private SpriteRenderer mySpriteRenderer;

        public override void EnterState(PlayerController controller)
        {
            base.EnterState(controller);

            myRb = controller.Rigidbody2D;
            myAnimator = controller.Animator;
            mySpriteRenderer = controller.Sprite;
        }

        public override void PerformState()
        {
            UpdatePlayerPosition();
        }

        public override void ExitState()
        {
            
        }

        virtual public void PerformAttack()
        {
            atkTimeStart = Time.time;
        }

        abstract protected void PerformSwitchAttack();

        virtual public void SwitchState()
        {
            controller.Animator.SetTrigger("switch");
            controller.AudioSource.PlayOneShot(settings.SwitchSFX);
            //switch animation somewhere here
            if (Time.time - atkTimeStart <= settings.SwitchWindow && Time.time - atkTimeStart >= settings.AtkDelay) //if within switch window, do switch attack
            {
                atkTimeStart = Time.time;
                PerformSwitchAttack();
            }
        }

        protected void UpdatePlayerPosition()
        {
            myVelocity = new Vector2(controller.MoveInput.x * settings.Speed, myRb.velocity.y);
            // if running backwards flip the animation 
            // Update myRb.velocity
            myRb.velocity = myVelocity;
            float absVelocityX = Mathf.Abs(myVelocity.x);
            myAnimator.SetFloat("Speed", absVelocityX);
            //if the sprite is moving make the sprite face that direction
            //otherwise keep the sprite facing in the direction we moved last
            if (absVelocityX > 0.01)
            {
                //flip character along horizontal axis
                mySpriteRenderer.flipX = myVelocity.x > 0;
            }

            myRb.velocity = new Vector2(myVelocity.x, myRb.velocity.y);

            if (Time.time - jumpTimer < settings.MaxJumpTime) //jump is held
            {
                //needed to get rid of AddForce() call to fix bug

                myRb.velocity = new Vector2(myRb.velocity.x, settings.JumpForce);
            }
        }

        public void PerformJump()
        {
            if (controller.FeetCollider.IsTouchingLayers()) //if touching any layers
            {
                jumpTimer = Time.time;
                controller.AudioSource.PlayOneShot(settings.JumpSFX);
                // Apply upward force to make the character jump
                //myRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                //now done in update() to allow for hold jump to be higher than tap
            }
        }

        public void CancelJump()
        {
            jumpTimer = -5;
        }
    }
}