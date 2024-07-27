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

            #region 当玩家还在地面上时获取前三帧的平均速度
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
            
            _playerStats.animator.SetFloat(_playerStats.FrontSpeedHash, _playerStats.playerInputController.currentMovementInput.magnitude * targetSpeed, .1f, Time.deltaTime);
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
        
        /// <summary>
        /// 重力模拟
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
        /// 跳跃检测
        /// </summary>
        private void Jump()
        {
            // 是否能跳跃 && 监听跳跃事件
            if (_playerStats.characterController.isGrounded && _playerStats.playerInputController.isJump)
            {
                _playerStats.VerticalVelocity = _playerStats.JumpVelocity;
            }

            // 播放跳跃动画
            if (!_playerStats.characterController.isGrounded)
            {
                _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.MidairThreshold, .1f, Time.deltaTime);
                _playerStats.animator.SetFloat(_playerStats.VerticalSpeedHash, _playerStats.VerticalVelocity, .1f, Time.deltaTime);
            }
        }
    }*/
}