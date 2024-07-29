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
            _playerStats = fsm.characterStats;
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
            Vector3 playerMovement = _playerStats.playerInputController.playerMovement;
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
                _playerStats.VerticalVelocity = _playerStats.JumpVelocity;
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
            _playerStats.animator.SetFloat(_playerStats.FrontSpeedHash, _playerStats.playerInputController.currentMovementInput.magnitude * targetSpeed, .1f, Time.deltaTime);
            
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
                return PlayStateMachine.PlayerState.NormalMidair;
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
            _playerStats.animator.SetFloat(_playerStats.FrontSpeedHash, _playerStats.playerInputController.currentMovementInput.magnitude * PlayerStats.CrouchSpeed, .1f, Time.deltaTime);
            
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
                else if (_playerStats.playerInputController.currentMovementInput.magnitude != 0)
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
            // 转换成站姿
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.landingThreshold, .1f, Time.deltaTime);
            
            // 根据是否奔跑，设置速度
            targetSpeed = _playerStats.playerInputController.isRun ? _playerStats.RunSpeed : _playerStats.WalkSpeed;
            _playerStats.animator.SetFloat(_playerStats.FrontSpeedHash, _playerStats.playerInputController.currentMovementInput.magnitude * targetSpeed, .1f, Time.deltaTime);
            
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
}