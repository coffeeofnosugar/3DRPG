using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Tools.CoffeeTools;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerStats : CharacterStats
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public PlayerInputController playerInputController;
        [ReadOnly] public Transform cameraTransform;

        public float RotationSpeed = 220;

        #region 下蹲

        [ReadOnly, TabGroup("下蹲")] public bool hasUpObstacle;
        /// <summary>
        /// 下蹲时的移动速度
        /// </summary>
        [ShowInInspector, TabGroup("下蹲")] public const float CrouchSpeed = 1.5f;
        
        /// <summary>
        /// 下蹲时玩家的高度
        /// </summary>
        [ShowInInspector, TabGroup("下蹲")] public float crouchPlayerHeight = .8f;

        #endregion

        #region 跳跃
        /// <summary>
        /// 重力，地球重力加速度是-9.8，但是为了提升游戏手感需提高这个值
        /// </summary>
        [ShowInInspector, TabGroup("跳跃")] public const float Gravity = -15f;

        /// <summary>
        /// 为提升跳跃手感，下落的速度需大于起跳速度
        /// </summary>
        [ShowInInspector, TabGroup("跳跃")] public const float FallMultiplier = 1.5f;
        
        /// <summary>
        /// 角色向下的速度
        /// </summary>
        [ReadOnly, TabGroup("跳跃")] public float VerticalVelocity;

        /// <summary>
        /// 着陆阈值，下落速度计算而来，在着陆时适当的将动画偏向蹲姿
        /// </summary>
        [ReadOnly, TabGroup("跳跃")] public float landingThreshold;
        
        /// <summary>
        /// 跳跃的最大高度
        /// </summary>
        [TabGroup("跳跃")] public float jumpMaxHeight = 1.5f;

        /// <summary>
        /// 跳跃初速度，根据重力学公式 h = v²/2g 计算而来
        /// </summary>
        [HideInInspector] public float JumpVelocity => Mathf.Sqrt(-2 * Gravity * jumpMaxHeight);
        #endregion

        #region 攀爬

        [ShowInInspector, TabGroup("攀爬")] public static LayerMask CheckoutClimbLayer = 1 << 9;

        /// <summary>
        /// 低于此高度的障碍物不检测是否进行攀爬
        /// </summary>
        [ShowInInspector, TabGroup("攀爬")] public const float LowClimbHeight = .5f;

        /// <summary>
        /// 攀爬检测距离，玩家面朝方在此距离内就能翻墙
        /// </summary>
        [ShowInInspector, TabGroup("攀爬")] public const float ClimbCheckDistance = 1f;

        /// <summary>
        /// 比较常用的角度，设置为常量
        /// </summary>
        [ShowInInspector, TabGroup("攀爬")] public const float ClimbAngel = 45f;

        /// <summary>
        /// 玩家与墙面的垂直检测距离
        /// </summary>
        [ShowInInspector, ReadOnly, TabGroup("攀爬")] public static readonly float ClimbDistance = Mathf.Cos(ClimbAngel) * ClimbCheckDistance;

        /// <summary>
        /// 墙面法线
        /// </summary>
        [ReadOnly, TabGroup("攀爬")] public Vector3 climbHitNormal;

        /// <summary>
        /// 墙面与玩家的距离，由最下面一根射线检测而来
        /// </summary>
        [ReadOnly, TabGroup("攀爬")] public float climbWallDistance;

        /// <summary>
        /// 墙壁凹凸面差值
        /// </summary>
        [ShowInInspector, TabGroup("攀爬")] public const float ClimbWallDistanceOffset = .3f;

        /// <summary>
        /// 射线检测高度距离间隔
        /// </summary>
        [ShowInInspector, TabGroup("攀爬")] public const float CheckHeightInterval = 1f;

        /// <summary>
        /// 存储四个射线集中的物体
        /// </summary>
        [ShowInInspector, ReadOnly, TabGroup("攀爬")] public readonly RaycastHit[] HitArray = new RaycastHit[4];
        
        public enum ClimbTypeEnum { Jump, Hurd, ClimbLow, ClimbHigh }

        /// <summary>
        /// 翻墙类型
        /// </summary>
        [HideInInspector] public ClimbTypeEnum climbType = ClimbTypeEnum.Jump;

        /// <summary>
        /// 攀爬的位置
        /// </summary>
        [ReadOnly, TabGroup("攀爬")] public Vector3 ledge;
        #endregion

        #region 落地检测

        [ShowInInspector, TabGroup("落地检测")] public static LayerMask CheckoutGroundLayer = 1 << 8 | 1 << 9; 
        
        /// <summary>
        /// 是否在地面上
        /// </summary>
        [ReadOnly, TabGroup("落地检测")] public bool isGrounded;
        
        /// <summary>
        /// 是否在斜坡上
        /// </summary>
        [ReadOnly, TabGroup("落地检测")] public bool isSlope;

        /// <summary>
        /// 在斜坡上时，角色向下的速度，防止下坡时角色拌脚，角色速度越快改值需更大
        /// </summary>
        [TabGroup("落地检测")] public float SlopeVerticalVelocity = 50;
        
        /// <summary>
        /// 检测射线起点向上偏移量
        /// </summary>
        [ShowInInspector, TabGroup("落地检测")] public const float GroundCheckOffset = .5f;
        #endregion

        #region 计算前三帧的平均速度
        
        [ReadOnly, TabGroup("计算前三帧的平均速度")] public Vector3 averageVel = Vector3.zero;
        [ShowInInspector, ReadOnly, TabGroup("计算前三帧的平均速度")]public const int CACHE_SIZE = 3;
        [ReadOnly, TabGroup("计算前三帧的平均速度")] public Vector3[] velCache = new Vector3[CACHE_SIZE];
        [ReadOnly, TabGroup("计算前三帧的平均速度")] public int currentCacheIndex = 0;
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
        public int IsClimbHash { get; } = Animator.StringToHash("isClimb");
        /// <summary>
        ///  0: 翻越
        ///  1: 低位攀爬
        ///  2: 高位攀爬
        /// </summary>
        public int ClimbTypeHash { get; } = Animator.StringToHash("ClimbType");
        public int IsFightingHash { get; } = Animator.StringToHash("isFighting");
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
            if (Physics.SphereCast(transform.position + (Vector3.up * GroundCheckOffset), characterController.radius, Vector3.down, out RaycastHit hit, GroundCheckOffset - characterController.radius + 10 * characterController.skinWidth, CheckoutGroundLayer))
            {
                // 如果接触到的点的法线不在(0,1,0)的方向上，那么人物就在斜坡上
                isSlope = hit.normal.y != 1f;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        // private void OnDrawGizmos()
        // {
        //     Transform transform = ((Component)this).transform;
        //     #region 地面检测
        //     // var start = transform.position + (Vector3.up * GroundCheckOffset);
        //     // var direction = Vector3.down;
        //     // float radius = characterController.radius;
        //     // float distance = GroundCheckOffset - radius + 10 * characterController.skinWidth;
        //     //
        //     //
        //     // Gizmos.color = Color.yellow;
        //     // Gizmos.DrawSphere(start, radius);
        //     // Gizmos.DrawLine(start, start + direction * distance);
        //     // Vector3 end = start + direction * distance;
        //     // Gizmos.DrawSphere(end, radius);
        //     #endregion
        //
        //     #region 翻墙检测
        //     
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawLine(transform.position + Vector3.up * LowClimbHeight, transform.position + Vector3.up * LowClimbHeight + transform.forward * ClimbDistance);
        //     Gizmos.DrawLine(transform.position + Vector3.up * (LowClimbHeight + CheckHeightInterval),
        //         transform.position + Vector3.up * (LowClimbHeight + CheckHeightInterval) + transform.forward * ClimbDistance);
        //     Gizmos.DrawLine(transform.position + Vector3.up * crouchPlayerHeight, transform.position + Vector3.up *
        //         (crouchPlayerHeight + crouchPlayerHeight));
        //
        //     #endregion
        // }
    }
}