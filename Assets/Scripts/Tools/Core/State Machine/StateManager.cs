using System;
using Sirenix.OdinInspector;
using Tools.CoffeeTools;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    [ShowInInspector, BoxGroup] public BaseState<EState> CurrentState;
    [ShowInInspector, BoxGroup] public EState LastStateKey { get; private set; }

    private void Start() { CurrentState.EnterState(); }

    protected void FixedUpdate() { CurrentState.FixedUpdate(); }

    public void Update()
    {
        CurrentState.UpdateState();
        EState nextStateKey = CurrentState.GetNextState();
        if (!nextStateKey.Equals(CurrentState.StateKey))
            TransitionToState(nextStateKey);
    }

    protected void LateUpdate() { CurrentState.LateUpdate(); }

    protected void OnAnimatorMove() { CurrentState.OnAnimatorMove(); }

    public void TransitionToState(EState stateKey)
    {
        CurrentState.ExitState();
        LastStateKey = CurrentState.StateKey;
        CurrentState = EnumTurnToState(stateKey);
        CurrentState.EnterState();
    }

    protected abstract BaseState<EState> EnumTurnToState(EState stateKey);
}