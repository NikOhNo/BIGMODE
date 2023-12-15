using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerStates
{
    public class MeleeState : PlayableState
    {
        public override void EnterState(PlayerController controller)
        {
            base.EnterState(controller);

            //TODO
        }

        public override void PerformState()
        {
            base.PerformState();
            
            //TODO
        }

        public override void ExitState()
        {
            base.ExitState();
            
            //TODO
        }

        public override void PerformAttack()
        {
            if (Time.time - atkTimeStart >= settings.AtkDelay)
            {
                base.PerformAttack();

                Collider2D[] enemiesHit = { };

                //play sword attack animation and sfx
                Debug.Log("Sword basic attack");
                enemiesHit = Physics2D.OverlapCircleAll(controller.AttackPoint, settings.BasicMeleeRange, settings.EnemyLayers);

                HandleHitEnemies(enemiesHit, settings.BasicDamageValue);
            }
        }

        protected override void PerformSwitchAttack()
        {
            Collider2D[] enemiesHit = { };

            //play spell attack animation and sfx
            controller.AudioSource.PlayOneShot(settings.SlashSFX);
            Debug.Log("Sword switch attack");
            enemiesHit = Physics2D.OverlapCircleAll(-controller.AttackPoint, settings.SwitchMeleeRange, settings.EnemyLayers);

            HandleHitEnemies(enemiesHit, settings.SwitchDamageValue);
        }

        private void HandleHitEnemies(Collider2D[] enemiesHit, int damageVal)
        {
            foreach (Collider2D enemy in enemiesHit)
            {
                if (enemy.GetComponent<EnemyController>() != null)
                {
                    enemy.GetComponent<EnemyController>().Hurt(damageVal);
                    controller.HealthSystem.AddRecoveryCurrency(settings.MeleeRecovery);
                }
            }
            controller.AudioSource.PlayOneShot(settings.HitSFX);
        }

        public override void SwitchState()
        {
            base.SwitchState();
            controller.SwitchState(controller.MageState);
        }
    }
}