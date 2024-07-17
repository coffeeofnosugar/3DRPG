using System;
using UnityEngine;

namespace PlayerController
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(CharacterStats))]
    [RequireComponent(typeof(Animator))]
    public class PlayStateMachine : StateManager<PlayStateMachine.PlayerState>
    {
        public enum PlayerState { Idle, Walk, Run }

        [HideInInspector] public CharacterStats characterStats;

        private IdleState _idleState;
        private WalkState _walkState;
        private RunState _runState;

        private void Awake()
        {
            characterStats = GetComponent<CharacterStats>();
            
            _idleState = new IdleState(this, PlayerState.Idle);
            _walkState = new WalkState(this, PlayerState.Walk);
            _runState = new RunState(this, PlayerState.Run);
            
            CurrentState = EnumTurnToState(PlayerState.Idle);
        }

        private new void Update()
        {
            base.Update();
            Debugs.Instance["PlayerState"] = CurrentState.ToString();
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