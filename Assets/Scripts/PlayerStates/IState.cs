using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerStates
{
    public interface IState
    {
        void EnterState(PlayerController controller);
        void PerformState();
        void ExitState();
    }
}