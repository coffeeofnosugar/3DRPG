using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.PlayerController
{
    public class Log : ActionNode
    {
        [Title("节点参数")]
        [SerializeField] public string message = "null";
        protected override void EnterState()
        {
            Debug.Log($"EnterState {message}");
            base.EnterState();
        }

        protected override void ExitState()
        {
            Debug.Log($"ExitState {message}");
            base.ExitState();
        }

        protected override State FixeUpdateState()
        {
            Debug.Log($"UpdateState {message}");
            if (child)
            {
                child.state = state;
                child.FixedUpdate();
            }
            return State.Success;
        }
    }
}
