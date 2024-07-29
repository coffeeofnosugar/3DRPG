using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public abstract class PlayerBaseState : BaseState<PlayStateMachine.PlayerState>
    {
        protected readonly PlayStateMachine _fsm;
        protected readonly PlayerStats _playerStats;
        
        protected PlayerBaseState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            _fsm = fsm;
            _playerStats = fsm.playerStats;
        }

        public override void UpdateState()
        {
            CalculateGravity();
        }
        
        /// <summary>
        /// 重力模拟
        /// </summary>
        private void CalculateGravity()
        {
            if (_fsm.CurrentState.StateKey != PlayStateMachine.PlayerState.NormalMidair)
            {
                if (_playerStats.isSlope)
                    // 当角色在斜坡上时，增加向下的移动速度
                    _playerStats.VerticalVelocity = PlayerStats.Gravity * 50 * Time.deltaTime;
                else
                    _playerStats.VerticalVelocity = PlayerStats.Gravity * Time.deltaTime;
            }
            else
            {
                if (_playerStats.VerticalVelocity <= 0)
                {
                    // 角色在下落时乘以一个系数，以提升手感
                    _playerStats.VerticalVelocity += PlayerStats.Gravity * PlayerStats.FallMultiplier * Time.deltaTime;
                }
                else
                {
                    // 上升时保持不变
                    _playerStats.VerticalVelocity += PlayerStats.Gravity * Time.deltaTime;
                }
            }
        }
        
        /// <summary>
        /// 角色旋转
        /// </summary>
        protected void PlayerRotate()
        {
            Vector3 playerMovement = _playerStats.playerInputController.inputMovementPlayer;
            // 获取玩家输入与角色的夹角（弧度）
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            _playerStats.animator.SetFloat(_playerStats.TurnSpeedHash, rad, .1f, Time.deltaTime);
            // 靠root motion自带的旋转角速度太慢，额外添加一个角速度
            // 在启用OnAnimatorMove后，上一行对角色的旋转就不起作用了，仅仅只起到播放动画的作用，所以需要将下面的系数变大，从180变成200
            _playerStats.transform.Rotate(0, rad * 200 * Time.deltaTime, 0f);
        }
        
        /// <summary>
        /// 跳跃检测
        /// </summary>
        protected void Jump()
        {
            // 监听跳跃事件
            if (_playerStats.playerInputController.isJump)
            {
                _playerStats.climbType = ClimbDetect(_playerStats.transform, _playerStats.playerInputController.inputMovement, 0);
                _playerStats.VerticalVelocity = _playerStats.JumpVelocity;
            }
        }
        
        /// <summary>
        /// 翻墙检测
        /// </summary>
        /// <param name="_playerTransform">玩家位置</param>
        /// <param name="_playerMovement">玩家位移</param>
        /// <param name="_offset">可以根据速度改变差值，Run时为1，Walk时为0.5，idle时为0</param>
        /// <returns></returns>
        private PlayerStats.ClimbTypeEnum ClimbDetect(Transform _playerTransform, Vector3 _playerMovement, float _offset)
        {
            // 朝玩家的朝向发射射线
            if (Physics.Raycast(_playerTransform.position + Vector3.up * PlayerStats.LowClimbHeight, _playerTransform.forward, out RaycastHit hit, PlayerStats.ClimbCheckDistance + _offset))
            {
                Vector3 climbHitNormal = hit.normal;
                // 计算墙体的法线与玩家前方的向量的角度是否大于45度  或  玩家的输入方向同理
                bool playerAndNormal =
                    Vector3.Angle(-climbHitNormal, _playerTransform.forward) > PlayerStats.ClimbAngel;
                var angel = Vector3.Angle(-climbHitNormal, _playerMovement);
                bool inputAndNormal = angel > PlayerStats.ClimbAngel + Mathf.Cos(_offset);
                if (Vector3.Angle(-climbHitNormal, _playerTransform.forward) > PlayerStats.ClimbAngel || Vector3.Angle(-climbHitNormal, _playerMovement) > PlayerStats.ClimbAngel + Mathf.Cos(_offset))
                {
                    return PlayerStats.ClimbTypeEnum.Jump;
                }
                Array.Clear(_playerStats.HitArray, 0, _playerStats.HitArray.Length);
                for (int i = 0; i < 4; i++)
                {
                    /*     ⑥ ⑤
                     *     |  |一一一 ④
                     *     |  |
                     *   墙墙墙墙一一 ③
                     *   墙墙墙墙
                     *   墙墙墙墙一一 ②
                     *   墙墙墙墙
                     *   墙墙墙墙一一 ①
                     *   墙墙墙墙  人
                    */
                    // 以向上1距离为间隔，玩家连续发射垂直墙面的4个射线
                    if (Physics.Raycast(_playerTransform.position + Vector3.up * (PlayerStats.LowClimbHeight - i * PlayerStats.CheckHeightInterval), -climbHitNormal, out RaycastHit hitInfo, PlayerStats.ClimbDistance + Mathf.Cos(_offset)))
                    {
                        _playerStats.HitArray[i] = hitInfo;
                        // 如果4条射线都检测到了，就无法翻越，正常跳跃
                        if (i == 3)
                            break;
                    }
                    else if (i == 0)
                    {
                        // 如果第一个射线没有射中物体，无需翻越，正常跳跃
                        break;
                    }
                    // 射线⑤：从上一次命中物体的正上方朝下发射射线，获取到玩家攀爬的位置
                    else if (Physics.Raycast(_playerStats.HitArray[i-1].point + Vector3.up * PlayerStats.CheckHeightInterval, Vector3.down, PlayerStats.CheckHeightInterval))
                    {
                        // 射线⑥：将上个射线稍微向前移动一点，检测是翻越还是攀爬
                        if (Physics.Raycast(_playerStats.HitArray[i-1].point + Vector3.up * PlayerStats.CheckHeightInterval - climbHitNormal * .2f, Vector3.down, PlayerStats.CheckHeightInterval))
                            // 墙面在①和②之间低位攀爬
                            return i == 1 ? PlayerStats.ClimbTypeEnum.ClimbLow : PlayerStats.ClimbTypeEnum.ClimbHigh;
                        else
                            return PlayerStats.ClimbTypeEnum.Hurd;
                    }
                }
            }
            return PlayerStats.ClimbTypeEnum.Jump;
        }
        
        /// <summary>
        /// 获取前三帧的平均速度
        /// </summary>
        /// <param name="newVel"></param>
        /// <returns></returns>
        protected Vector3 AverageVel(Vector3 newVel)
        {
            _playerStats.velCache[_playerStats.currentCacheIndex] = newVel;
            _playerStats.currentCacheIndex++;
            _playerStats.currentCacheIndex %= PlayerStats.CACHE_SIZE;
            Vector3 average = Vector3.zero;
            foreach (var vel in _playerStats.velCache)
            {
                average += vel;
            }

            return average / PlayerStats.CACHE_SIZE;
        }
    }

    public class NormalStandState : PlayerBaseState
    {
        private float targetSpeed;
        public NormalStandState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void EnterState() { }

        public override void ExitState() { }

        public override void FixedUpdate() { }

        public override void UpdateState()
        {
            // 转换成站姿
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, PlayerStats.StandThreshold, .1f, Time.deltaTime);
            
            // 根据是否奔跑，设置速度
            targetSpeed = _playerStats.playerInputController.isRun ? _playerStats.RunSpeed : _playerStats.WalkSpeed;
            _playerStats.animator.SetFloat(_playerStats.FrontSpeedHash, _playerStats.playerInputController.inputMovement.magnitude * targetSpeed, .1f, Time.deltaTime);
            
            base.UpdateState();
            Jump();
            PlayerRotate();
        }

        public override void LateUpdate() { }

        public override void OnAnimatorMove()
        {
            Vector3 playerDeltaMovement = _playerStats.animator.deltaPosition;
            playerDeltaMovement.y = _playerStats.VerticalVelocity * Time.deltaTime;
            _playerStats.characterController.Move(playerDeltaMovement);
            _playerStats.averageVel = AverageVel(_playerStats.animator.velocity);
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (!_playerStats.isGrounded)
            {
                switch (_playerStats.climbType)
                {
                    case PlayerStats.ClimbTypeEnum.Jump:
                        return PlayStateMachine.PlayerState.NormalMidair;
                    case PlayerStats.ClimbTypeEnum.Hurd:
                        return PlayStateMachine.PlayerState.NormalClimbState;
                    case PlayerStats.ClimbTypeEnum.ClimbLow:
                        return PlayStateMachine.PlayerState.NormalClimbState;
                    case PlayerStats.ClimbTypeEnum.ClimbHigh:
                        return PlayStateMachine.PlayerState.NormalClimbState;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (_playerStats.playerInputController.isCrouch)
                return PlayStateMachine.PlayerState.NormalCrouch;
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }

    public class NormalCrouchState : PlayerBaseState
    {
        public NormalCrouchState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }
        public override void EnterState() { }

        public override void ExitState() { }

        public override void FixedUpdate() { }

        public override void UpdateState()
        {
            // 转换成蹲姿
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, PlayerStats.CrouchThreshold, .1f, Time.deltaTime);
            // 将速度切换成蹲姿移动速度
            _playerStats.animator.SetFloat(_playerStats.FrontSpeedHash, _playerStats.playerInputController.inputMovement.magnitude * PlayerStats.CrouchSpeed, .1f, Time.deltaTime);
            
            base.UpdateState();
            PlayerRotate();
        }

        public override void LateUpdate() { }

        public override void OnAnimatorMove()
        {
            Vector3 playerDeltaMovement = _playerStats.animator.deltaPosition;
            playerDeltaMovement.y = _playerStats.VerticalVelocity * Time.deltaTime;
            _playerStats.characterController.Move(playerDeltaMovement);
            _playerStats.averageVel = AverageVel(_playerStats.animator.velocity);
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (!_playerStats.isGrounded)
                return PlayStateMachine.PlayerState.NormalMidair;
            else if (_playerStats.playerInputController.isCrouch)
                return PlayStateMachine.PlayerState.NormalCrouch;
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }

    public class NormalMidairState : PlayerBaseState
    {
        public NormalMidairState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void EnterState()
        {
            // 可能会抖动，取消线性插值
            // 转换成半空姿态
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, PlayerStats.MidairThreshold);
            // 起跳时，随机取一个值，使跳跃播放不同的动画或镜像动画
            float feetTween = Mathf.Repeat(_playerStats.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
            feetTween = feetTween < .5f ? 1 : -1;
            if (_fsm.LastState == PlayStateMachine.PlayerState.NormalStand)
            {
                if (_playerStats.playerInputController.isRun)
                    feetTween *= 3;
                else if (_playerStats.playerInputController.inputMovement.magnitude != 0)
                    feetTween *= 2;
                else
                    feetTween *= Random.Range(.5f, 1f);
            }
            else
                feetTween *= Random.Range(0, .5f);
            _playerStats.animator.SetFloat(_playerStats.JumpRandomHash, feetTween);
        }

        public override void ExitState() { }

        public override void FixedUpdate() { }

        public override void UpdateState()
        {
            // 根据角色向下的速度播放动画
            _playerStats.animator.SetFloat(_playerStats.VerticalSpeedHash, _playerStats.VerticalVelocity);
            base.UpdateState();
            PlayerRotate();
        }

        public override void LateUpdate() { }

        public override void OnAnimatorMove()
        {
            // Vector3 playerDeltaMovement = _playerStats.averageVel * Time.deltaTime;
            // playerDeltaMovement.y = _playerStats.VerticalVelocity * Time.deltaTime;
            // _playerStats.characterController.Move(playerDeltaMovement);
            // 优化代码 ||，将上面代码可优化成下方形式
            //         V
            _playerStats.averageVel.y = _playerStats.VerticalVelocity;
            Vector3 playerDeltaMovement = _playerStats.averageVel * Time.deltaTime;
            _playerStats.characterController.Move(playerDeltaMovement);
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (!_playerStats.isGrounded)
                return PlayStateMachine.PlayerState.NormalMidair;
            else
            {
                _playerStats.landingThreshold = Mathf.Clamp(_playerStats.VerticalVelocity, -10, 0);
                _playerStats.landingThreshold /= 20f;
                _playerStats.landingThreshold += 1f;
                return PlayStateMachine.PlayerState.NormalLanding;
            }
        }
    }

    public class NormalLandingState : PlayerBaseState
    {
        private const float JumpCoolDown = .15f;
        private float enterTime;
        
        private float targetSpeed;
        public NormalLandingState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void EnterState()
        {
            enterTime = Time.time;
        }

        public override void ExitState() { }

        public override void FixedUpdate() { }
        public override void UpdateState()
        {
            // 转换成站姿， landingThreshold根据落地时的下落速度计算而来，适当的将动画偏向蹲姿
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.landingThreshold, .1f, Time.deltaTime);
            
            // 根据是否奔跑，设置速度
            targetSpeed = _playerStats.playerInputController.isRun ? _playerStats.RunSpeed : _playerStats.WalkSpeed;
            _playerStats.animator.SetFloat(_playerStats.FrontSpeedHash, _playerStats.playerInputController.inputMovement.magnitude * targetSpeed, .1f, Time.deltaTime);
            
            base.UpdateState();
            // Jump();
            PlayerRotate();
        }

        public override void LateUpdate() { }

        public override void OnAnimatorMove()
        {
            Vector3 playerDeltaMovement = _playerStats.animator.deltaPosition;
            playerDeltaMovement.y = _playerStats.VerticalVelocity * Time.deltaTime;
            _playerStats.characterController.Move(playerDeltaMovement);
            _playerStats.averageVel = AverageVel(_playerStats.animator.velocity);
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (Time.time - enterTime >= JumpCoolDown)
            {
                if (!_playerStats.isGrounded)
                    return PlayStateMachine.PlayerState.NormalMidair;
                else if (_playerStats.playerInputController.isCrouch)
                    return PlayStateMachine.PlayerState.NormalCrouch;
                else
                    return PlayStateMachine.PlayerState.NormalStand;
            }
            return PlayStateMachine.PlayerState.NormalLanding;
        }
    }

    public class NormalClimbState : PlayerBaseState
    {
        public NormalClimbState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }

        public override void EnterState()
        {
            switch (_playerStats.climbType)
            {
                case PlayerStats.ClimbTypeEnum.Jump:
                    throw new ArgumentOutOfRangeException();
                case PlayerStats.ClimbTypeEnum.Hurd:
                    _playerStats.animator.SetInteger(_playerStats.ClimbTypeHash, 0);
                    break;
                case PlayerStats.ClimbTypeEnum.ClimbLow:
                    _playerStats.animator.SetInteger(_playerStats.ClimbTypeHash, 1);
                    break;
                case PlayerStats.ClimbTypeEnum.ClimbHigh:
                    _playerStats.animator.SetInteger(_playerStats.ClimbTypeHash, 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _playerStats.animator.SetTrigger(_playerStats.IsClimbHash);
        }

        public override void ExitState() { }

        public override void FixedUpdate() { }

        public override void LateUpdate() { }

        public override void OnAnimatorMove() { }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_playerStats.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f)
            {
                Debug.Log("返回站姿");
                return PlayStateMachine.PlayerState.NormalStand;
            }
            return PlayStateMachine.PlayerState.NormalClimbState;
        }
    }
}