using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayStateMachine : StateManager<PlayStateMachine.PlayerState>
    {
        public enum PlayerState { NormalStand, NormalCrouch, Run }

        [HideInInspector] public PlayerStats characterStats;

        private NormalStandState _normalStandState;
        private NormalCrouchState _normalCrouchState;
        private RunState _runState;

        private void Awake()
        {
            characterStats = GetComponent<PlayerStats>();
            
            _normalStandState = new NormalStandState(this, PlayerState.NormalStand);
            _normalCrouchState = new NormalCrouchState(this, PlayerState.NormalCrouch);
            _runState = new RunState(this, PlayerState.Run);
            
            CurrentState = EnumTurnToState(PlayerState.NormalStand);
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
                PlayerState.NormalStand => _normalStandState,
                PlayerState.NormalCrouch => _normalCrouchState,
                PlayerState.Run => _runState,
                _ => _normalStandState
            };
            return newState;
        }
    }
}