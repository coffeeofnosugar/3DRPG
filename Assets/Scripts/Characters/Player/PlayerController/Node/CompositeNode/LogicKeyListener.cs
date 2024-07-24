using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class LogicKeyListener : CompositeNode
    {
        public enum KeyAction { Move, Run, Jump, Attack }

        [Header("节点参数")]
        [SerializeField] public KeyAction checkKeyAction;

        private int index;
        protected override void EnterState()
        {
            index = 0;
            state = JudgeKey();
        }

        protected override void ExitState()
        {
            
        }

        protected override State FixeUpdateState()
        {
            if (state != State.Running)
                return state;
            
            State _state = JudgeKey();
            if (children[0] && index == 0)
            {
                children[0].state = State.Success;
                children[0].FixedUpdate();
                index++;
            }

            if (children[1] && index == 1)
            {
                children[1].state = _state;
                if (_state != State.Running)
                {
                    children[1].state = _state;
                    index++;
                }
                state = children[1].FixedUpdate();
            }
            
            if (children[2] && index == 2)
            {
                children[2].state = State.Success;
                children[2].FixedUpdate();
                index++;
            }
            return state;
        }
        
        /// <summary>
        /// 判断键位
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