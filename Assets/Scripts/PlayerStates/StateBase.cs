using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerStates
{
    public abstract class StateBase : IState
    {
        protected PlayerController controller;
        protected PlayerSettingsSO settings;

        public virtual void EnterState(PlayerController controller)
        {
            this.controller = controller;
            this.settings = controller.PlayerSettings;
        }
        public abstract void PerformState();
        public abstract void ExitState();
    }
}