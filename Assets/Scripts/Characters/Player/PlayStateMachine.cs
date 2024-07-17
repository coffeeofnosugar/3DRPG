using UnityEngine;

namespace PlayerController
{
    public class PlayStateMachine : StateManager<PlayStateMachine.PlayerState>
    {
        public enum PlayerState { Idle, Walk, Run }

        [HideInInspector] public Animator animator;
        public PlayerInputController PlayerInputController;

        private IdleState _idleState;
        private WalkState _walkState;
        private RunState _runState;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            PlayerInputController = new PlayerInputController();
            
            _idleState = new IdleState(this, PlayerState.Idle);
            _walkState = new WalkState(this, PlayerState.Walk);
            _runState = new RunState(this, PlayerState.Run);
            
            CurrentState = EnumTurnToState(PlayerState.Idle);
        }

        protected override BaseState<PlayerState> EnumTurnToState(PlayerState stateKey)
        {
            BaseState<PlayerState> newState = stateKey switch
            {
                PlayerState.Idle => _idleState,
                PlayerState.Walk => _walkState,
                PlayerState.Run => _runState,
                _ => _idleState
            };
            return newState;
        }
    }
}