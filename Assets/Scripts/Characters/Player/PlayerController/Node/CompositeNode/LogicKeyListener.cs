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
            
        }

        protected override void ExitState()
        {
            
        }

        protected override void FixeUpdateState()
        {
            
        }

        protected override State UpdateState()
        {
            switch (checkKeyAction)
            {
                case KeyAction.Move:
                    var movement = _playerInputController.currentMovementInput;
                    if (movement.x != 0 || movement.y != 0)
                        return State.Success;
                    break;
                case KeyAction.Run:
                    if (_playerInputController.isRun)
                        return State.Success;
                    break;
                case KeyAction.Jump:
                    if (_playerInputController.isJump)
                        return State.Success;
                    break;
                case KeyAction.Attack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return State.Failure;
        }

        protected override void LateUpdateState()
        {
            
        }
    }
}