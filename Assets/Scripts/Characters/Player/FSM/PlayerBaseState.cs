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

        public override void UpdateState()
        {
            CalculateGravity();
        }
        
        /// <summary>
        /// ����ģ��
        /// </summary>
        private void CalculateGravity()
        {
            if (_playerStats.characterController.isGrounded)
            {
                _playerStats.VerticalVelocity = PlayerStats.Gravity * Time.deltaTime;
            }
            else
            {
                _playerStats.VerticalVelocity += PlayerStats.Gravity * Time.deltaTime;
            }
        }
        
        /// <summary>
        /// ��ɫ��ת
        /// </summary>
        protected void PlayerRotate()
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
        /// ��Ծ���
        /// </summary>
        protected void Jump()
        {
            // �Ƿ�����Ծ && ������Ծ�¼�
            if (_playerStats.characterController.isGrounded && _playerStats.playerInputController.isJump)
                _playerStats.VerticalVelocity = PlayerStats.JumpVelocity;
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
            if (!_playerStats.characterController.isGrounded)
            {
                return PlayStateMachine.PlayerState.NormalMidair;
            }
            else if (_playerStats.playerInputController.isCrouch)
            {
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
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
            if (!_playerStats.characterController.isGrounded)
            {
                return PlayStateMachine.PlayerState.NormalMidair;
            }
            else if (_playerStats.playerInputController.isCrouch)
            {
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }

    public class NormalMidairState : PlayerBaseState
    {
        public NormalMidairState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(fsm, key) { }
        public override void EnterState() { }

        public override void ExitState() { }

        public override void FixedUpdate() { }

        public override void UpdateState()
        {
            // ת���ɰ����̬
            _playerStats.animator.SetFloat(_playerStats.PlayerStateHash, PlayerStats.MidairThreshold, .1f, Time.deltaTime);
            // ���ݽ�ɫ���µ��ٶȲ��Ŷ���
            _playerStats.animator.SetFloat(_playerStats.VerticalSpeedHash, _playerStats.VerticalVelocity, .1f, Time.deltaTime);
            
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
            if (!_playerStats.characterController.isGrounded)
            {
                return PlayStateMachine.PlayerState.NormalMidair;
            }
            else if (_playerStats.playerInputController.isCrouch)
            {
                return PlayStateMachine.PlayerState.NormalCrouch;
            }
            return PlayStateMachine.PlayerState.NormalStand;
        }
    }
}