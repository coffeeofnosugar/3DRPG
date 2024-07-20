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

        protected override void FixeUpdateState()
        {
            Debug.Log($"FixeUpdateState {message}");
        }

        protected override State UpdateState()
        {
            Debug.Log($"UpdateState {message}");
            return State.Success;
        }

        protected override void LateUpdateState()
        {
            Debug.Log($"LateUpdateState {message}");
        }
    }
}
