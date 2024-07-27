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

        // �ƶ�
        protected void PlayerMove()
        {
            float targetSpeed = _playerStats.PlayerInputController.isRun
                ? _playerStats.RunSpeed
                : _playerStats.WalkSpeed;
                
            _playerStats.animator.SetFloat(_playerStats.VerticalSpeedHash, _playerStats.PlayerInputController.currentMovementInput.magnitude * targetSpeed, .1f, Time.deltaTime);
        }
        
        // ��ת
        protected void PlayerRotate()
        {
            var forward = _playerStats.cameraTransform.forward;
            Vector3 camForwardProjection = new Vector3(forward.x, 0, forward.z).normalized;
            // ������루����������
            Vector3 playerMovement = camForwardProjection * _playerStats.PlayerInputController.currentMovementInput.y +
                                     _playerStats.cameraTransform.right * _playerStats.PlayerInputController.currentMovementInput.x;
            // ������루������������
            playerMovement = _playerStats.transform.InverseTransformVector(playerMovement);
            // ��ȡ����������ɫ�ļнǣ����ȣ�
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            _playerStats.animator.SetFloat(_playerStats.TurnSpeedHash, rad, .1f, Time.deltaTime);
            // ��root motion�Դ�����ת���ٶ�̫�����������һ�����ٶ�
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