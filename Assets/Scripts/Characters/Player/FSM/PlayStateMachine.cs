using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayStateMachine : StateManager<PlayStateMachine.PlayerState>
    {
        public enum PlayerState { NormalStand, NormalCrouch, NormalMidair, NormalLanding }

        [HideInInspector] public PlayerStats characterStats;

        private NormalStandState _normalStandState;
        private NormalCrouchState _normalCrouchState;
        private NormalMidairState _normalMidairState;
        private NormalLandingState _normalLandingState;

        private void Awake()
        {
            characterStats = GetComponent<PlayerStats>();
            
            _normalStandState = new NormalStandState(this, PlayerState.NormalStand);
            _normalCrouchState = new NormalCrouchState(this, PlayerState.NormalCrouch);
            _normalMidairState = new NormalMidairState(this, PlayerState.NormalMidair);
            _normalLandingState = new NormalLandingState(this, PlayerState.NormalLanding);
            
            CurrentState = EnumTurnToState(PlayerState.NormalStand);
        }

        private new void Update()
        {
            base.Update();
            Debugs.Instance["PlayerState"] = CurrentState.ToString();
            Debugs.Instance["PlayerSpeed"] = characterStats.characterController.velocity.magnitude.ToString("f2");
        }
        
        protected override BaseState<PlayerState> EnumTurnToState(PlayerState stateKey)
        {
            BaseState<PlayerState> newState = stateKey switch
            {
                PlayerState.NormalStand => _normalStandState,
                PlayerState.NormalCrouch => _normalCrouchState,
                PlayerState.NormalMidair => _normalMidairState,
                PlayerState.NormalLanding => _normalLandingState,
                _ => _normalStandState
            };
            return newState;
        }
    }
}