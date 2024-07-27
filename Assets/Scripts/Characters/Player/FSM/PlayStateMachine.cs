using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerInputController))]
    public class PlayStateMachine : StateManager<PlayStateMachine.PlayerState>
    {
        public enum PlayerState { NormalStand, NormalCrouch, NormalMidair }

        [HideInInspector] public PlayerStats characterStats;

        private NormalState _normalStandState;
        // private NormalCrouchState _normalCrouchState;
        // private NormalMidairState _normalMidairState;

        private void Awake()
        {
            characterStats = GetComponent<PlayerStats>();
            
            _normalStandState = new NormalState(this, PlayerState.NormalStand);
            // _normalCrouchState = new NormalCrouchState(this, PlayerState.NormalCrouch);
            // _normalMidairState = new NormalMidairState(this, PlayerState.NormalMidair);
            
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
                // PlayerState.NormalCrouch => _normalCrouchState,
                // PlayerState.NormalMidair => _normalMidairState,
                _ => _normalStandState
            };
            return newState;
        }
    }
}