using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected BaseState<EState> CurrentState;

    private void Start()
    {
        CurrentState.EnterState();
    }

    public void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.StateKey))
            CurrentState.UpdateState();
        else
            TransitionToState(nextStateKey);
    }

    public void TransitionToState(EState stateKey)
    {
        CurrentState.ExitState();
        CurrentState = EnumTurnToState(stateKey);
        CurrentState.EnterState();
    }

    protected abstract BaseState<EState> EnumTurnToState(EState stateKey);
}