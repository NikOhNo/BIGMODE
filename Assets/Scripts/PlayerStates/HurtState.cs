using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerStates
{
    public class HurtState : StateBase
    {
        float elapsedTime = 0f;
        float hurtTime;

        public override void EnterState(PlayerController controller)
        {
            base.EnterState(controller);
            elapsedTime = 0f;
            hurtTime = controller.PlayerSettings.HurtTime;
            controller.Sprite.color = Color.red;
            controller.Rigidbody2D.velocity = Vector3.zero;
        }

        public override void PerformState()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= hurtTime)
            {
                controller.SwitchState(controller.PreviousState);
            }
        }

        public override void ExitState()
        {
            controller.Sprite.color = Color.white;
        }
    }
}