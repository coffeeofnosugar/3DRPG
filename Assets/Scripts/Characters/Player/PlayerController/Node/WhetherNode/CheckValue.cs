using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class CheckValue : WhetherNode
    {
        [Header("节点参数")]
        public PlayerInputController.InputParameter key;
        public Vector2 vector2Value;
        private bool _bool;
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
            switch (key)
            {
                case PlayerInputController.InputParameter.Movement:
                    _bool = playerStats.playerInputController.inputMovement == vector2Value;
                    break;
                case PlayerInputController.InputParameter.IsRun:
                    break;
                case PlayerInputController.InputParameter.IsJump:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (children[0])
            {
                if (_bool)
                {
                    children[0].state = state;
                    children[0].FixedUpdate();
                }
            }
            else if (children[1])
            {
                if (!_bool)
                {
                    children[1].state = state;
                    children[1].FixedUpdate();
                }
            }
            return state;
        }
    }
}