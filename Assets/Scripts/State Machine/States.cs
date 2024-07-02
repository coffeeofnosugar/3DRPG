using UnityEngine;
using UnityEngine.AI;

public class BaseIdleState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    private float time = 0;

    public BaseIdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.agent.destination = manager.transform.position;
        // 进入非战斗状态后，使Animator的Layer第二层权重设置为0，不播放该层的动画
        parameter.animator.SetLayerWeight(1, 0);
    }

    public void OnUpdate()
    {
        if (manager.IsFoundPlayer())
            manager.TransitionState(StateType.FightIdle);

        if (parameter.isPatrol)
        {
            time += Time.deltaTime;
            if (time > parameter.idleTime)
                manager.TransitionState(StateType.Walk);
        }
    }

    public void OnExit()
    {
        time = 0;
    }
}

public class WalkState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    private Vector3 wayPoint;

    public WalkState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.agent.speed = parameter.walkSpeed;
        parameter.animator.SetBool("Walk", true);
        GetNewWayPoint();
    }

    public void OnUpdate()
    {
        if (manager.IsFoundPlayer())
            manager.TransitionState(StateType.FightIdle);

        parameter.agent.destination = wayPoint;
        float stoppingDistance = parameter.agent.stoppingDistance;
        if ((wayPoint - manager.transform.position).sqrMagnitude <= stoppingDistance * stoppingDistance)
            manager.TransitionState(StateType.BaseIdle);
    }

    public void OnExit()
    {
        parameter.animator.SetBool("Walk", false);
    }

    private void GetNewWayPoint()
    {
        float randomX = Random.Range(-parameter.patrolRange, parameter.patrolRange);
        float randomZ = Random.Range(-parameter.patrolRange, parameter.patrolRange);

        Vector3 randomPoint = new Vector3(parameter.originPosition.x + randomX, manager.transform.position.y, parameter.originPosition.z + randomZ);

        wayPoint = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1, 1) ? hit.position : manager.transform.position;
    }
}

public class FightIdleState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    public FightIdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        // 进入战斗状态后，使Animator的Layer第二层权重设置为1，完全覆盖其他层的动画
        parameter.animator.SetLayerWeight(1, 1);
    }

    public void OnUpdate()
    {
        if (parameter.attackTarget)
        {
            if (manager.IsTargetInAttackRange())
            {
                if (parameter.lastAttackTime >= parameter.attackCD)
                {
                    manager.TransitionState(StateType.Attack);
                }
            }
            else
                manager.TransitionState(StateType.Run);
        }
    }

    public void OnExit() {  }
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

        parameter.agent.speed = parameter.runSpeed;
    }

    public void OnUpdate()
    {
        if (manager.IsFoundPlayer())
        {
            if (manager.IsTargetInAttackRange())
            {
                manager.TransitionState(StateType.FightIdle);
            }
            else
                parameter.agent.destination = parameter.attackTarget.transform.position;
        }
        else
            manager.TransitionState(StateType.BaseIdle);
    }

    public void OnExit()
    {
        parameter.animator.SetBool("Run", false);
    }
}

public class AttackState : IState
{
    private FSM manager;
    private EnemyParameter parameter;
    private CharacterStats characterStats;

    private AnimatorStateInfo info;
    private bool isCritical;

    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
    }

    public void OnEnter()
    {
        // 判断是否暴击
        isCritical = Random.value < characterStats.CriticalChance;
        Debugs.Instance["isCritical"] = isCritical.ToString();
        parameter.animator.SetBool("IsCritical", isCritical);
        // 攻击开始时重置CD
        parameter.lastAttackTime = 0;
        // 停止移动
        parameter.agent.destination = manager.transform.position;
        // 朝向攻击目标
        manager.transform.LookAt(parameter.attackTarget.transform);
        // 播放攻击动画
        parameter.animator.SetTrigger("Attack");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(1);
        Debugs.Instance["info.normalizedTime"] = info.normalizedTime.ToString("f2");
        if (info.normalizedTime >= .95f)
            manager.TransitionState(StateType.FightIdle);
    }

    public void OnExit() {  }
}

public class HitState : IState
{
    private FSM manager;
    private EnemyParameter parameter;

    private AnimatorStateInfo info;

    public HitState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.SetTrigger("Hit");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(1);
        if (info.normalizedTime >= .95f)
            manager.TransitionState(StateType.FightIdle);

        if (parameter.health <= 0)
            manager.TransitionState(StateType.Die);
        else
            manager.TransitionState(StateType.FightIdle);
    }

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