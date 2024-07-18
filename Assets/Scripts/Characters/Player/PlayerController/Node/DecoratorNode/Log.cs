using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Log : DecoratorNode
    {
        [SerializeField] private string message;
        protected override void EnterState()
        {
            Debug.Log($"enter {message}");
        }

        protected override void ExitState()
        {
            Debug.Log($"exit {message}");
        }

        protected override void FixeUpdateState()
        {
            Debug.Log($"fixe {message}");
        }

        protected override State UpdateState()
        {
            Debug.Log($"update {message}");
            return State.Success;
        }

        protected override void LateUpdateState()
        {
            Debug.Log($"lateupdate {message}");
        }
    }
}
