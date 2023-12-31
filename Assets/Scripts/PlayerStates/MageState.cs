﻿using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.PlayerStates
{
    public class MageState : PlayableState
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

        public override void SwitchState()
        {
            base.SwitchState();
            controller.SwitchState(controller.MeleeState);
        }

        public override void PerformAttack()
        {
            if (Time.time - atkTimeStart >= settings.AtkDelay) {
                base.PerformAttack();

                controller.SpellShooter.ShootSpell(settings.BasicDamageValue, GetDirection());
            }
        }

        protected override void PerformSwitchAttack()
        {
            Debug.Log("Spell switch attack");
            controller.SpellShooter.ShootSpell(settings.SwitchDamageValue, GetDirection());
        }

        private int GetDirection()
        {
            int direction = boolToInt[controller.Sprite.flipX];

            return direction;
        }
    }
}