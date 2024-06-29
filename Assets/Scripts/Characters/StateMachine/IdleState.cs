public class IdleState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter() { }

    public void OnUpdate() { }

    public void OnExit() { }
}

public class WalkState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    public WalkState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.SetBool("Walk", true);
    }

    public void OnUpdate() { }

    public void OnExit()
    {
        parameter.animator.SetBool("Walk", false);
    }
}

public class ChaseState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.SetBool("Chase", true);
    }

    public void OnUpdate() { }

    public void OnExit()
    {
        parameter.animator.SetBool("Chase", false);
    }
}

public class RunState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    public RunState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.SetBool("Run", true);
    }

    public void OnUpdate() { }

    public void OnExit()
    {
        parameter.animator.SetBool("Run", false);
    }
}

public class AttackState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.SetTrigger("Attack");
    }

    public void OnUpdate() { }

    public void OnExit() { }
}

public class HitState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    public HitState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.SetTrigger("Hit");
    }

    public void OnUpdate() { }

    public void OnExit() { }
}

public class DieState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    public DieState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.SetTrigger("Die");
    }

    public void OnUpdate() { }

    public void OnExit() { }
}