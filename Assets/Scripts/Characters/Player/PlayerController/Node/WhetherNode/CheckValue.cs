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
        public bool boolValue;
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
                    _bool = _characterStats.PlayerInputController.currentMovementInput == vector2Value;
                    break;
                case PlayerInputController.InputParameter.IsRun:
                    break;
                case PlayerInputController.InputParameter.IsJump:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_bool && children[0])
            {
                children[0].state = state;
                children[0].FixedUpdate();
            }
            else if (!_bool && children[1])
            {
                children[1].state = state;
                children[1].FixedUpdate();
            }
            return state;
        }
    }
}