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
            
        }

        protected override void ExitState()
        {
            
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
                    Debug.Log("���Ķ���");
                    _characterStats.animator.SetBool(animatorParameter, boolValue);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return state;
        }
    }
}