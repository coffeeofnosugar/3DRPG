using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.PlayerController
{
    public class ChangeCharacterState : ActionNode
    {
        [Title("节点参数")]
        public string animatorParameter;
        public AnimatorControllerParameterType type;
        public bool boolValue;
        protected override void EnterState()
        {
            base.EnterState();
        }

        protected override void ExitState()
        {
            base.ExitState();
        }

        protected override State FixeUpdateState()
        {
            switch (type)
            {
                case AnimatorControllerParameterType.Float:
                    break;
                case AnimatorControllerParameterType.Int:
                    break;
                case AnimatorControllerParameterType.Bool:
                    playerStats.animator.SetBool(animatorParameter, boolValue);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    playerStats.animator.SetTrigger(animatorParameter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (child)
            {
                child.state = state;
                child.FixedUpdate();
            }
            return State.Success;
        }
    }
}