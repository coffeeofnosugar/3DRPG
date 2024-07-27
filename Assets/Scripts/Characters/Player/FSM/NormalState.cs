using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{/*
    public class NormalState : PlayerBaseState
    {
        private enum NormalStateEnum { StandState, CrouchState, Midair }

        private NormalStateEnum currentState;
        private Vector3 averageVel = Vector3.zero;
        private static readonly int CACHE_SIZE = 3;
        private Vector3[] velCache = new Vector3[CACHE_SIZE];
        private int currentChacheIndex = 0;
        
        public NormalState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void UpdateState()
        {
            SwitchState();
            CalculateGravity();
            Jump();
            PlayerMove();
            PlayerRotate();
        }

        public override void OnAnimatorMove()
        {
            if (_playerStats.characterController.isGrounded)
            {
                Vector3 playerDeltaMovement = _playerStats.animator.deltaPosition;
                playerDeltaMovement.y = _playerStats.VerticalVelocity * Time.deltaTime;
                _playerStats.characterController.Move(playerDeltaMovement);
                averageVel = AverageVel(_playerStats.animator.velocity);
            }
            else
            {
                averageVel.y = _playerStats.VerticalVelocity;
                Vector3 playerDeltaMovement = averageVel * Time.deltaTime;
                _playerStats.characterController.Move(playerDeltaMovement);
            }

            #region ����һ��ڵ�����ʱ��ȡǰ��֡��ƽ���ٶ�
            Vector3 AverageVel(Vector3 newVel)
            {
                velCache[currentChacheIndex] = newVel;
                currentChacheIndex++;
                currentChacheIndex %= CACHE_SIZE;
                Vector3 average = Vector3.zero;
                foreach (var vel in velCache)
                {
                    average += vel;
                }

                return average / CACHE_SIZE;
            }
            #endregion
        }

        private void SwitchState()
        {
            if (_playerStats.playerInputController.isJump)
                _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.MidairThreshold, .1f, Time.deltaTime);
            else if (_playerStats.playerInputController.isCrouch)
                _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.CrouchThreshold, .1f, Time.deltaTime);
            else
                _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.StandThreshold, .1f, Time.deltaTime);
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
            
            _playerStats.animator.SetFloat(_playerStats.FrontSpeedHash, _playerStats.playerInputController.currentMovementInput.magnitude * targetSpeed, .1f, Time.deltaTime);
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
        
        /// <summary>
        /// ����ģ��
        /// </summary>
        private void CalculateGravity()
        {
            if (_playerStats.characterController.isGrounded)
            {
                _playerStats.VerticalVelocity = _playerStats.Gravity * Time.deltaTime;
            }
            else
            {
                _playerStats.VerticalVelocity += _playerStats.Gravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// ��Ծ���
        /// </summary>
        private void Jump()
        {
            // �Ƿ�����Ծ && ������Ծ�¼�
            if (_playerStats.characterController.isGrounded && _playerStats.playerInputController.isJump)
            {
                _playerStats.VerticalVelocity = _playerStats.JumpVelocity;
            }

            // ������Ծ����
            if (!_playerStats.characterController.isGrounded)
            {
                _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.MidairThreshold, .1f, Time.deltaTime);
                _playerStats.animator.SetFloat(_playerStats.VerticalSpeedHash, _playerStats.VerticalVelocity, .1f, Time.deltaTime);
            }
        }
    }*/
}