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

        public override void EnterState() { }
        public override void ExitState() { }
        public override void FixedUpdate() { }
        public override void UpdateState()
        {
            PlayerMove();
            PlayerRotate();
        }
        public override void LateUpdate() { }
        public override void OnAnimatorMove()
        {
            _playerStats.characterController.Move(_playerStats.animator.deltaPosition);
        }

        /// <summary>
        /// 角色移动
        /// </summary>
        private void PlayerMove()
        {
            float targetSpeed;
            if (_playerStats.playerInputController.isCrouch)
                targetSpeed = PlayerStats.CrouchSpeed;
            else if (_playerStats.playerInputController.isRun)
                targetSpeed = _playerStats.RunSpeed;
            else
                targetSpeed = _playerStats.WalkSpeed;
                
            _playerStats.animator.SetFloat(_playerStats.VerticalSpeedHash, _playerStats.playerInputController.currentMovementInput.magnitude * targetSpeed, .1f, Time.deltaTime);
        }
        
        /// <summary>
        /// 角色旋转
        /// </summary>
        private void PlayerRotate()
        {
            var forward = _playerStats.cameraTransform.forward;
            Vector3 camForwardProjection = new Vector3(forward.x, 0, forward.z).normalized;
            // 玩家输入（世界向量）
            Vector3 playerMovement = camForwardProjection * _playerStats.playerInputController.currentMovementInput.y +
                                     _playerStats.cameraTransform.right * _playerStats.playerInputController.currentMovementInput.x;
            // 玩家输入（玩家相对向量）
            playerMovement = _playerStats.transform.InverseTransformVector(playerMovement);
            
            // 获取玩家输入与角色的夹角（弧度）
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            _playerStats.animator.SetFloat(_playerStats.TurnSpeedHash, rad, .1f, Time.deltaTime);
            // 靠root motion自带的旋转角速度太慢，额外添加一个角速度
            // 在启用OnAnimatorMove后，上一行对角色的旋转就不起作用了，仅仅只起到播放动画的作用，所以需要将下面的系数变大，从180变成200
            _playerStats.transform.Rotate(0, rad * 200 * Time.deltaTime, 0f);
        }
    }
    
    public class NormalStandState : PlayerBaseState
    {
        public NormalStandState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void UpdateState()
        {
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.StandThreshold, .1f, Time.deltaTime);
            base.UpdateState();
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_playerStats.playerInputController.isCrouch)
            {
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }

    public class NormalCrouchState : PlayerBaseState
    {
        public NormalCrouchState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }
        
        public override void UpdateState()
        {
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.CrouchThreshold, .1f, Time.deltaTime);
            base.UpdateState();
        }
        
        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_playerStats.playerInputController.isCrouch)
            {
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }

    public class NormalMidairState : PlayerBaseState
    {
        public NormalMidairState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_playerStats.playerInputController.isCrouch)
            {
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }
}