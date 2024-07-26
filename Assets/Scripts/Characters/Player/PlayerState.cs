using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{/*
    public class IdleState : BaseState<PlayStateMachine.PlayerState>
    {
        private readonly PlayStateMachine _fsm;
        private readonly PlayerStats playerStats;
        public IdleState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            this._fsm = fsm;
            playerStats = fsm.characterStats as PlayerStats;
        }

        public override void EnterState()
        {
            // _characterStats.animator.SetLayerWeight(1, 0);
            playerStats.animator.SetBool("isWalk", false);
            playerStats.animator.SetBool("isRun", false);
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState()
        {
            
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (playerStats.PlayerInputController.currentMovementInput.x != 0
                || playerStats.PlayerInputController.currentMovementInput.y != 0)
            {
                if (playerStats.PlayerInputController.isRun)
                {
                    return PlayStateMachine.PlayerState.Run;
                }
                return PlayStateMachine.PlayerState.Walk;
            }
            return PlayStateMachine.PlayerState.Idle;
        }
    }

    public class WalkState : BaseState<PlayStateMachine.PlayerState>
    {
        private readonly PlayStateMachine _fsm;
        private readonly PlayerStats playerStats;
        public WalkState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            this._fsm = fsm;
            playerStats = fsm.characterStats as PlayerStats;
        }

        public override void EnterState()
        {
            playerStats.animator.SetBool("isWalk", true);
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState()
        {
            
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (playerStats.PlayerInputController.currentMovementInput.x != 0
                || playerStats.PlayerInputController.currentMovementInput.y != 0)
            {
                if (playerStats.PlayerInputController.isRun)
                {
                    return PlayStateMachine.PlayerState.Run;
                }
                return PlayStateMachine.PlayerState.Walk;
            }
            return PlayStateMachine.PlayerState.Idle;
        }
    }

    public class RunState : BaseState<PlayStateMachine.PlayerState>
    {
        private readonly PlayStateMachine _fsm;
        private readonly PlayerStats playerStats;
        public RunState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            this._fsm = fsm;
            playerStats = fsm.characterStats as PlayerStats;
        }

        public override void EnterState()
        {
            playerStats.animator.SetBool("isRun", true);
        }

        public override void ExitState()
        {
            playerStats.animator.SetBool("isRun", false);
        }

        public override void UpdateState()
        {
            
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (playerStats.PlayerInputController.currentMovementInput.x != 0
                || playerStats.PlayerInputController.currentMovementInput.y != 0)
            {
                if (playerStats.PlayerInputController.isRun)
                {
                    return PlayStateMachine.PlayerState.Run;
                }
                return PlayStateMachine.PlayerState.Walk;
            }
            return PlayStateMachine.PlayerState.Idle;
        }
    }*/
}