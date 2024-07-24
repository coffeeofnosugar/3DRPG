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

        private bool _started;
        protected override void EnterState()
        {
            if (children[0])
                children[0].isTrigger = true;
            if (children[1])
                children[1].isTrigger = false;
            if (children[2])
                children[2].isTrigger = true;
            state = JudgeKey();
        }

        protected override void ExitState()
        {
            
        }

        protected override State FixeUpdateState()
        {
            if (state != State.Running)
                return state;
            
            state = JudgeKey();
            if (!_started)
            {
                children[0].FixedUpdate();
                _started = true;
            }

            if (state == State.Running)
                state = children[1].FixedUpdate();

            if (state != State.Running)
            {
                children[2].FixedUpdate();
                _started = false;
            }
            return state;
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