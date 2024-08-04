using System;

public abstract class BaseState<EState> where EState : Enum
{
    protected BaseState(EState key)
    {
        StateKey = key;
    }
    public EState StateKey { get; private set; }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FixedUpdate();
    public abstract void UpdateState();
    public abstract void LateUpdate();
    public abstract void OnAnimatorMove();
    public abstract EState GetNextState();
}