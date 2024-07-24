using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Log : DecoratorNode
    {
        [Header("节点参数")]
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
            return base.FixeUpdateState();
        }
    }
}
