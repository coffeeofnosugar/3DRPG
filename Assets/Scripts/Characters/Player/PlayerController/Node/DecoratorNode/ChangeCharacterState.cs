using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class ChangeCharacterState : DecoratorNode
    {
        [Header("�ڵ����")]
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
                    _characterStats.animator.SetBool(animatorParameter, boolValue);
                    return State.Success;
                case AnimatorControllerParameterType.Trigger:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return State.Failure;
        }
    }
}