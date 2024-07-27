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
        /// ��ɫ�ƶ�
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
        /// ��ɫ��ת
        /// </summary>
        private void PlayerRotate()
        {
            var forward = _playerStats.cameraTransform.forward;
            Vector3 camForwardProjection = new Vector3(forward.x, 0, forward.z).normalized;
            // ������루����������
            Vector3 playerMovement = camForwardProjection * _playerStats.playerInputController.currentMovementInput.y +
                                     _playerStats.cameraTransform.right * _playerStats.playerInputController.currentMovementInput.x;
            // ������루������������
            playerMovement = _playerStats.transform.InverseTransformVector(playerMovement);
            
            // ��ȡ����������ɫ�ļнǣ����ȣ�
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            _playerStats.animator.SetFloat(_playerStats.TurnSpeedHash, rad, .1f, Time.deltaTime);
            // ��root motion�Դ�����ת���ٶ�̫�����������һ�����ٶ�
            // ������OnAnimatorMove����һ�жԽ�ɫ����ת�Ͳ��������ˣ�����ֻ�𵽲��Ŷ��������ã�������Ҫ�������ϵ����󣬴�180���200
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