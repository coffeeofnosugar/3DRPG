using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Log : DecoratorNode
    {
        [Header("节点参数")]
        [SerializeField] public string message = "null";
        protected override void EnterState()
        {
            Debug.Log($"EnterState {message}  {state}");
        }

        protected override void ExitState()
        {
            Debug.Log($"ExitState {message}  {state}");
        }

        protected override State FixeUpdateState()
        {
            Debug.Log($"UpdateState {message}  {state}");
            return State.Success;
        }
    }
}
