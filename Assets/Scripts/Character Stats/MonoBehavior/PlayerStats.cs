using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerStats : CharacterStats
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public PlayerInputController playerInputController;
        [HideInInspector] public Transform cameraTransform;

        /// <summary>
        /// 下蹲时的移动速度
        /// </summary>
        public const float CrouchSpeed = 1.5f;

        /// <summary>
        /// 重力，地球重力加速度是-9.8，但是为了提升游戏手感需提高这个值
        /// </summary>
        public const float Gravity = -15f;

        /// <summary>
        /// 为提升跳跃手感，下落的速度需大于起跳速度
        /// </summary>
        public const float FallMultiplier = 1.5f;
        
        /// <summary>
        /// 角色向下的速度
        /// </summary>
        public float VerticalVelocity;
        
        /// <summary>
        /// 跳跃的最大高度
        /// </summary>
        public float JumpMaxHeight = 1.5f;

        /// <summary>
        /// 跳跃初速度，根据重力学公式 h = v²/2g 计算而来
        /// </summary>
        public float JumpVelocity => Mathf.Sqrt(-2 * Gravity * JumpMaxHeight);

        #region 落地检测
        
        public bool isGrounded;
        public const float GroundCheckOffset = .5f;
        #endregion

        #region 计算前三帧的平均速度
        
        public Vector3 averageVel = Vector3.zero;
        public const int CACHE_SIZE = 3;
        public Vector3[] velCache = new Vector3[CACHE_SIZE];
        public int currentCacheIndex = 0;
        #endregion

        #region 获取动画哈希值
        
        public const float CrouchThreshold = 0f;
        public const float StandThreshold = 1f;
        public const float MidairThreshold = 2.1f;
        public int PlayerStateHash { get; } = Animator.StringToHash("PlayerState");
        public int FrontSpeedHash { get; } = Animator.StringToHash("FrontSpeed");
        public int TurnSpeedHash { get; } = Animator.StringToHash("TurnSpeed");
        public int VerticalSpeedHash { get; } = Animator.StringToHash("VerticalSpeed");
        public int JumpRandomHash { get; } = Animator.StringToHash("JumpRandom");
        public int HorizontalSpeedHash { get; } = Animator.StringToHash("HorizontalSpeed");
        public int IsFighting { get; } = Animator.StringToHash("isFighting");
        #endregion

        protected override void Awake()
        {
            base.Awake();
            characterController = GetComponent<CharacterController>();
            playerInputController = GetComponent<PlayerInputController>();
            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            CheckGround();
        }

        private void CheckGround()
        {
            if (Physics.SphereCast(transform.position + (Vector3.up * GroundCheckOffset), characterController.radius, Vector3.down, out RaycastHit hit, GroundCheckOffset - characterController.radius + 2 * characterController.skinWidth))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}