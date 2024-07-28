using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected BaseState<EState> CurrentState;
    public EState LastState { get; private set; }

    private void Start() { CurrentState.EnterState(); }

    protected void FixedUpdate() { CurrentState.FixedUpdate(); }

    public void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.StateKey))
            CurrentState.UpdateState();
        else
            TransitionToState(nextStateKey);
    }

    protected void LateUpdate() { CurrentState.LateUpdate(); }

    protected void OnAnimatorMove() { CurrentState.OnAnimatorMove(); }

    public void TransitionToState(EState stateKey)
    {
        CurrentState.ExitState();
        LastState = CurrentState.StateKey;
        CurrentState = EnumTurnToState(stateKey);
        CurrentState.EnterState();
    }

    protected abstract BaseState<EState> EnumTurnToState(EState stateKey);
}