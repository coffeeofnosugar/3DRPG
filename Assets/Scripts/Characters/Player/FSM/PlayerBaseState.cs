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
        /// ����ģ��
        /// </summary>
        private void CalculateGravity()
        {
            if (_fsm.CurrentState.StateKey != PlayStateMachine.PlayerState.NormalMidair)
            {
                if (_playerStats.isSlope)
                    // ����ɫ��б����ʱ���������µ��ƶ��ٶ�
                    _playerStats.VerticalVelocity = PlayerStats.Gravity * 50 * Time.deltaTime;
                else
                    _playerStats.VerticalVelocity = PlayerStats.Gravity * Time.deltaTime;
            }
            else
            {
                if (_playerStats.VerticalVelocity <= 0)
                {
                    // ��ɫ������ʱ����һ��ϵ�����������ָ�
                    _playerStats.VerticalVelocity += PlayerStats.Gravity * PlayerStats.FallMultiplier * Time.deltaTime;
                }
                else
                {
                    // ����ʱ���ֲ���
                    _playerStats.VerticalVelocity += PlayerStats.Gravity * Time.deltaTime;
                }
            }
        }
        
        /// <summary>
        /// ��ɫ��ת
        /// </summary>
        protected void PlayerRotate()
        {
            Vector3 playerMovement = _playerStats.playerInputController.inputMovementPlayer;
            // ��ȡ����������ɫ�ļнǣ����ȣ�
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            _playerStats.animator.SetFloat(_playerStats.TurnSpeedHash, rad, .1f, Time.deltaTime);
            // ��root motion�Դ�����ת���ٶ�̫�����������һ�����ٶ�
            // ������OnAnimatorMove����һ�жԽ�ɫ����ת�Ͳ��������ˣ�����ֻ�𵽲��Ŷ��������ã�������Ҫ�������ϵ����󣬴�180���200
            _playerStats.transform.Rotate(0, rad * 200 * Time.deltaTime, 0f);
        }
        
        /// <summary>
        /// ��Ծ���
        /// </summary>
        protected void Jump()
        {
            // ������Ծ�¼�
            if (_playerStats.playerInputController.isJump)
            {
                _playerStats.climbType = ClimbDetect(_playerStats.transform, _playerStats.playerInputController.inputMovement, 0);
                _playerStats.VerticalVelocity = _playerStats.JumpVelocity;
            }
        }
        
        /// <summary>
        /// ��ǽ���
        /// </summary>
        /// <param name="_playerTransform">���λ��</param>
        /// <param name="_playerMovement">���λ��</param>
        /// <param name="_offset">���Ը����ٶȸı��ֵ��RunʱΪ1��WalkʱΪ0.5��idleʱΪ0</param>
        /// <returns></returns>
        private PlayerStats.ClimbTypeEnum ClimbDetect(Transform _playerTransform, Vector3 _playerMovement, float _offset)
        {
            // ����ҵĳ���������
            if (Physics.Raycast(_playerTransform.position + Vector3.up * PlayerStats.LowClimbHeight, _playerTransform.forward, out RaycastHit hit, PlayerStats.ClimbCheckDistance + _offset))
            {
                Vector3 climbHitNormal = hit.normal;
                // ����ǽ��ķ��������ǰ���������ĽǶ��Ƿ����45��  ��  ��ҵ����뷽��ͬ��
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
                    /*     �� ��
                     *     |  |һһһ ��
                     *     |  |
                     *   ǽǽǽǽһһ ��
                     *   ǽǽǽǽ
                     *   ǽǽǽǽһһ ��
                     *   ǽǽǽǽ
                     *   ǽǽǽǽһһ ��
                     *   ǽǽǽǽ  ��
                    */
                    // ������1����Ϊ���������������䴹ֱǽ���4������
                    if (Physics.Raycast(_playerTransform.position + Vector3.up * (PlayerStats.LowClimbHeight - i * PlayerStats.CheckHeightInterval), -climbHitNormal, out RaycastHit hitInfo, PlayerStats.ClimbDistance + Mathf.Cos(_offset)))
                    {
                        _playerStats.HitArray[i] = hitInfo;
                        // ���4�����߶���⵽�ˣ����޷���Խ��������Ծ
                        if (i == 3)
                            break;
                    }
                    else if (i == 0)
                    {
                        // �����һ������û���������壬���跭Խ��������Ծ
                        break;
                    }
                    // ���ߢݣ�����һ��������������Ϸ����·������ߣ���ȡ�����������λ��
                    else if (Physics.Raycast(_playerStats.HitArray[i-1].point + Vector3.up * PlayerStats.CheckHeightInterval, Vector3.down, PlayerStats.CheckHeightInterval))
                    {
                        // ���ߢޣ����ϸ�������΢��ǰ�ƶ�һ�㣬����Ƿ�Խ��������
                        if (Physics.Raycast(_playerStats.HitArray[i-1].point + Vector3.up * PlayerStats.CheckHeightInterval - climbHitNormal * .2f, Vector3.down, PlayerStats.CheckHeightInterval))
                            // ǽ���ڢٺ͢�֮���λ����
                            return i == 1 ? PlayerStats.ClimbTypeEnum.ClimbLow : PlayerStats.ClimbTypeEnum.ClimbHigh;
                        else
                            return PlayerStats.ClimbTypeEnum.Hurd;
                    }
                }
            }
            return PlayerStats.ClimbTypeEnum.Jump;
        }
        
        /// <summary>
        /// ��ȡǰ��֡��ƽ���ٶ�
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
            // ת����վ��
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, PlayerStats.StandThreshold, .1f, Time.deltaTime);
            
            // �����Ƿ��ܣ������ٶ�
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
            // ת���ɶ���
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, PlayerStats.CrouchThreshold, .1f, Time.deltaTime);
            // ���ٶ��л��ɶ����ƶ��ٶ�
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
            // ���ܻᶶ����ȡ�����Բ�ֵ
            // ת���ɰ����̬
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, PlayerStats.MidairThreshold);
            // ����ʱ�����ȡһ��ֵ��ʹ��Ծ���Ų�ͬ�Ķ������񶯻�
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
            // ���ݽ�ɫ���µ��ٶȲ��Ŷ���
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
            // �Ż����� ||�������������Ż����·���ʽ
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
            // ת����վ�ˣ� landingThreshold�������ʱ�������ٶȼ���������ʵ��Ľ�����ƫ�����
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, _playerStats.landingThreshold, .1f, Time.deltaTime);
            
            // �����Ƿ��ܣ������ٶ�
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
                Debug.Log("����վ��");
                return PlayStateMachine.PlayerState.NormalStand;
            }
            return PlayStateMachine.PlayerState.NormalClimbState;
        }
    }
}