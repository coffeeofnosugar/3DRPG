using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class LogicKeyListener : CompositeNode
    {
        public enum KeyAction { Move, Run, Jump, Attack }

        [SerializeField] private KeyAction checkKeyAction;
        protected override void EnterState()
        {
            state = JudgeKey();
            
        }

        protected override void ExitState()
        {
            
        }

        protected override State FixeUpdateState()
        {
            return State.Success;
        }
        
        /// <summary>
        /// ÅÐ¶Ï¼üÎ»
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private State JudgeKey()
        {
            switch (checkKeyAction)
            {
                case KeyAction.Move:
                    var movement = _playerInputController.currentMovementInput;
                    if (movement.x != 0 || movement.y != 0)
                        return State.Running;
                    break;
                case KeyAction.Run:
                    if (_playerInputController.isRun)
                        return State.Running;
                    break;
                case KeyAction.Jump:
                    if (_playerInputController.isJump)
                    {
                        // _playerInputController.PlayerInput.CharacterControls.Jump.started += ;
                        // _playerInputController.PlayerInput.CharacterControls.Jump.canceled += onJumpInput;
                    }
                    return State.Running;
                    break;
                case KeyAction.Attack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return State.Failure;
        }
    }
}