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
            base.EnterState();
        }

        protected override void ExitState()
        {
            base.ExitState();
        }

        protected override State FixeUpdateState()
        {
            if (state != State.Running)
                return state;
            
            state = JudgeKey();
            
            if (children[0])
            {
                if (index == 0)
                {
                    children[0].state = State.Success;
                    children[0].FixedUpdate();
                    index = 1;
                }
            }
            else
            {
                index = 1;
            }
            

            if (children[1])
            {
                if (index == 1)
                {
                    children[1].state = state;
                    state = children[1].FixedUpdate();
                    if (state != State.Running)
                    {
                        index = 2;
                    }
                }
            }
            else
            {
                index = 2;
            }
            
            // if (state != State.Running)
            // {
            //     index = 2;
            // }
            if (children[2] && index == 2 && state != State.Running)
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