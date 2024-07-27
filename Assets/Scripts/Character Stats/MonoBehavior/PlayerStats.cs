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
        public readonly float CrouchSpeed = 1.5f;
        /// <summary>
        /// 重力
        /// </summary>
        public readonly float Gravity = -9.8f;
        /// <summary>
        /// 角色向下的速度
        /// </summary>
        public float VerticalVelocity;
        /// <summary>
        /// 跳跃速度
        /// </summary>
        public float JumpVelocity = 5f;

        #region 计算前三帧的平均速度
        public Vector3 averageVel = Vector3.zero;
        public static readonly int CACHE_SIZE = 3;
        public Vector3[] velCache = new Vector3[CACHE_SIZE];
        public int currentChacheIndex = 0;
        #endregion
        
        public float CrouchThreshold { get; } = 0f;
        public float StandThreshold { get; } = 1f;
        public float MidairThreshold { get; } = 2.1f;

        public int PlayerStateHash { get; } = Animator.StringToHash("PlayerState");
        public int FrontSpeedHash { get; } = Animator.StringToHash("FrontSpeed");
        public int HorizontalSpeedHash { get; } = Animator.StringToHash("HorizontalSpeed");
        public int VerticalSpeedHash { get; } = Animator.StringToHash("VerticalSpeed");
        public int TurnSpeedHash { get; } = Animator.StringToHash("TurnSpeed");
        public int IsFighting { get; } = Animator.StringToHash("isFighting");

        protected override void Awake()
        {
            base.Awake();
            characterController = GetComponent<CharacterController>();
            playerInputController = GetComponent<PlayerInputController>();
            cameraTransform = Camera.main.transform;
        }
    }
}