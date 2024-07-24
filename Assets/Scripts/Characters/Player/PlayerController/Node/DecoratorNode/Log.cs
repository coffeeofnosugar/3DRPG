using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Log : DecoratorNode
    {
        [SerializeField] private string message = "null";
        protected override void EnterState()
        {
            Debug.Log($"EnterState {message}");
        }

        protected override void ExitState()
        {
            Debug.Log($"ExitState {message}");
        }

        protected override State FixeUpdateState()
        {
            Debug.Log($"UpdateState {message}");
            return State.Success;
        }
    }
}
