using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class IdleState : BaseState<PlayStateMachine.PlayerState>
    {
        private PlayStateMachine fsm;
        public IdleState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            this.fsm = fsm;
        }

        public override void EnterState()
        {
            Debug.Log("½øÈëidle");
        }

        public override void ExitState()
        {
            Debug.Log("ÍË³öidle");
        }

        public override void UpdateState()
        {
            Debug.Log("idle");
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            return PlayStateMachine.PlayerState.Idle;
        }
    }

    public class WalkState : BaseState<PlayStateMachine.PlayerState>
    {
        public WalkState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState()
        {
            
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            throw new System.NotImplementedException();
        }
    }

    public class RunState : BaseState<PlayStateMachine.PlayerState>
    {
        public RunState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState()
        {
            
        }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}