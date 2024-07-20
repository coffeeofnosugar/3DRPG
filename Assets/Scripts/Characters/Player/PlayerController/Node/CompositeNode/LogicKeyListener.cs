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
            if (state != State.Failure)
            {
                children[0].Update();
            }
        }

        protected override void ExitState()
        {
            
        }

        protected override void FixeUpdateState()
        {
            
        }

        protected override State UpdateState()
        {
            return state;
        }

        protected override void LateUpdateState()
        {
            
        }

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