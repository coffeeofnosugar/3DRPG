using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class IdleState : BaseState<PlayStateMachine.PlayerState>
    {
        private readonly PlayStateMachine _fsm;
        private readonly CharacterStats _characterStats;
        public IdleState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            this._fsm = fsm;
            _characterStats = fsm.characterStats;
        }

        public override void EnterState()
        {
            // _characterStats.animator.SetLayerWeight(1, 0);
            _characterStats.animator.SetBool("isWalk", false);
            _characterStats.animator.SetBool("isRun", false);
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState()
        {
            
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_characterStats.PlayerInputController.currentMovementInput.x != 0
                || _characterStats.PlayerInputController.currentMovementInput.y != 0)
            {
                if (_characterStats.PlayerInputController.isRun)
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
        private readonly CharacterStats _characterStats;
        public WalkState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            this._fsm = fsm;
            _characterStats = fsm.characterStats;
        }

        public override void EnterState()
        {
            _characterStats.animator.SetBool("isWalk", true);
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState()
        {
            
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_characterStats.PlayerInputController.currentMovementInput.x != 0
                || _characterStats.PlayerInputController.currentMovementInput.y != 0)
            {
                if (_characterStats.PlayerInputController.isRun)
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
        private readonly CharacterStats _characterStats;
        public RunState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            this._fsm = fsm;
            _characterStats = fsm.characterStats;
        }

        public override void EnterState()
        {
            _characterStats.animator.SetBool("isRun", true);
        }

        public override void ExitState()
        {
            _characterStats.animator.SetBool("isRun", false);
        }

        public override void UpdateState()
        {
            
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            if (_characterStats.PlayerInputController.currentMovementInput.x != 0
                || _characterStats.PlayerInputController.currentMovementInput.y != 0)
            {
                if (_characterStats.PlayerInputController.isRun)
                {
                    return PlayStateMachine.PlayerState.Run;
                }
                return PlayStateMachine.PlayerState.Walk;
            }
            return PlayStateMachine.PlayerState.Idle;
        }
    }
}