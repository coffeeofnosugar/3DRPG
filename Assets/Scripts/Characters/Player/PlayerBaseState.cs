using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class PlayerBaseState : BaseState<PlayStateMachine.PlayerState>
    {
        protected readonly PlayStateMachine _fsm;
        protected readonly PlayerStats _playerStats;
        protected PlayerBaseState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            _fsm = fsm;
            _playerStats = fsm.characterStats;
        }

        // 移动
        protected void PlayerMove()
        {
            float targetSpeed = _playerStats.PlayerInputController.isRun
                ? _playerStats.RunSpeed
                : _playerStats.WalkSpeed;
                
            _playerStats.animator.SetFloat(_playerStats.VerticalSpeedHash, _playerStats.PlayerInputController.currentMovementInput.magnitude * targetSpeed, .1f, Time.deltaTime);
        }
        
        // 旋转
        protected void PlayerRotate()
        {
            var forward = _playerStats.cameraTransform.forward;
            Vector3 camForwardProjection = new Vector3(forward.x, 0, forward.z).normalized;
            // 玩家输入（世界向量）
            Vector3 playerMovement = camForwardProjection * _playerStats.PlayerInputController.currentMovementInput.y +
                                     _playerStats.cameraTransform.right * _playerStats.PlayerInputController.currentMovementInput.x;
            // 玩家输入（玩家相对向量）
            playerMovement = _playerStats.transform.InverseTransformVector(playerMovement);
            // 获取玩家输入与角色的夹角（弧度）
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            _playerStats.animator.SetFloat(_playerStats.TurnSpeedHash, rad, .1f, Time.deltaTime);
            // 靠root motion自带的旋转角速度太慢，额外添加一个角速度
            _playerStats.transform.Rotate(0, rad * 180 * Time.deltaTime, 0f);
        }
    }
    
    public class NormalStandState : PlayerBaseState
    {
        public NormalStandState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void EnterState() {  }

        public override void ExitState() {  }

        public override void UpdateState()
        {
            PlayerMove();
            PlayerRotate();
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_playerStats.PlayerInputController.isCrouch)
            {
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }

    public class NormalCrouchState : PlayerBaseState
    {
        public NormalCrouchState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void EnterState() {  }

        public override void ExitState() {  }

        public override void UpdateState()
        {
            _playerStats.animator.SetFloat(_playerStats.VerticalSpeedHash, _playerStats.PlayerInputController.playerMovement.z * _playerStats.WalkSpeed, .1f, Time.deltaTime);
            _playerStats.animator.SetFloat(_playerStats.HorizontalSpeedHash, _playerStats.PlayerInputController.playerMovement.y * _playerStats.WalkSpeed, .1f, Time.deltaTime);
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_playerStats.PlayerInputController.currentMovementInput.x != 0
                || _playerStats.PlayerInputController.currentMovementInput.y != 0)
            {
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }

    public class RunState : PlayerBaseState
    {
        public RunState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void EnterState() { }

        public override void ExitState() { }

        public override void UpdateState() { }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_playerStats.PlayerInputController.currentMovementInput.x != 0
                || _playerStats.PlayerInputController.currentMovementInput.y != 0)
            {
                if (_playerStats.PlayerInputController.isRun)
                {
                    return PlayStateMachine.PlayerState.Run;
                }
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }
}