using UnityEngine;
using UnityEngine.AI;

public class BaseIdleState : IState
{
    private FSM manager;
    private EnemyParameter parameter;
    private CharacterStats characterStats;

    private float time = 0;

    public BaseIdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
    }

    public void OnEnter()
    {
        parameter.agent.destination = manager.transform.position;
        // 进入非战斗状态后，使Animator的Layer第二层权重设置为0，不播放该层的动画
        parameter.animator.SetLayerWeight(1, 0);
    }

    public void OnUpdate()
    {
        if (characterStats.getHit)
            manager.TransitionState(StateType.Hit);

        if (manager.IsFoundPlayer())
            manager.TransitionState(StateType.FightIdle);

        if (characterStats.IsOpenAI)
        {
            if (parameter.originPosition.x != manager.transform.position.x || parameter.originPosition.z != manager.transform.position.z)
            {
                time += Time.deltaTime;
                if (time > parameter.idleTime)
                    manager.TransitionState(StateType.Walk);
            }
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
    private CharacterStats characterStats;

    private Vector3 wayPoint;
    private float stoppingDistance;

    public WalkState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
        this.stoppingDistance = parameter.agent.stoppingDistance;
    }

    public void OnEnter()
    {
        parameter.agent.speed = parameter.walkSpeed;
        parameter.animator.SetBool("Walk", true);
        GetNewWayPoint();
    }

    public void OnUpdate()
    {
        if (characterStats.getHit)
            manager.TransitionState(StateType.Hit);

        // 如果视野中有敌人，进入战斗模式
        if (manager.IsFoundPlayer())
            manager.TransitionState(StateType.FightIdle);

        // 如果是巡逻怪
        if (parameter.isPatrol)
        {
            // 开始巡逻
            parameter.agent.destination = wayPoint;
            if ((wayPoint - manager.transform.position).sqrMagnitude <= stoppingDistance * stoppingDistance)
                manager.TransitionState(StateType.BaseIdle);
        }
        else
        {
            // 返回出生点
            parameter.agent.destination = parameter.originPosition;
            if (parameter.originPosition.x == manager.transform.position.x && parameter.originPosition.z == manager.transform.position.z)
            {
                float angel = Quaternion.Angle(parameter.originRotation, manager.transform.rotation);
                Debug.Log(angel);
                Debugs.Instance["angel"] = angel.ToString();
                if (angel >= 0.01f)
                {
                    manager.transform.rotation = Quaternion.Slerp(manager.transform.rotation, parameter.originRotation, 0.01f);
                }
                else
                {
                    manager.TransitionState(StateType.BaseIdle);
                }
            }
        }
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
    private CharacterStats characterStats;

    public FightIdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
    }

    public void OnEnter()
    {
        // 进入战斗状态后，使Animator的Layer第二层权重设置为1，完全覆盖其他层的动画
        parameter.animator.SetLayerWeight(1, 1);
    }

    public void OnUpdate()
    {
        if (characterStats.getHit)
            manager.TransitionState(StateType.Hit);

        if (characterStats.IsOpenAI && parameter.attackTarget)
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
    private CharacterStats characterStats;

    public RunState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
    }

    public void OnEnter()
    {
        parameter.animator.SetBool("Run", true);

        parameter.agent.speed = parameter.runSpeed;
    }

    public void OnUpdate()
    {
        if (characterStats.getHit)
            manager.TransitionState(StateType.Hit);

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

    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
    }

    public void OnEnter()
    {
        // 判断是否暴击
        characterStats.isCritical = Random.value < characterStats.CriticalChance;
        Debugs.Instance["isCritical"] = characterStats.isCritical.ToString();
        parameter.animator.SetBool("IsCritical", characterStats.isCritical);
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
        if (characterStats.getHit)
            manager.TransitionState(StateType.Hit);

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
    private CharacterStats characterStats;

    private AnimatorStateInfo info;

    public HitState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
    }

    public void OnEnter()
    {
        parameter.animator.SetTrigger("Hit");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(2);

        if (characterStats.CurrentHealth <= 0)
            manager.TransitionState(StateType.Die);
        else if (info.normalizedTime >= .95f)
            manager.TransitionState(StateType.FightIdle);
    }

    public void OnExit()
    {
        characterStats.getHit = false;
    }
}

public class DieState : IState
{
    private FSM manager;
    private EnemyParameter parameter;
    private CharacterStats characterStats;

    public DieState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
    }

    public void OnEnter()
    {
        parameter.agent.enabled = false;
        manager.coll.enabled = false;
        parameter.attackTarget = null;
        parameter.animator.SetTrigger("Death");
        Object.Destroy(manager.gameObject, characterStats.DestoryTime);
    }

    public void OnUpdate() { }

    public void OnExit() { }
}
public class WinState : IState
{
    private FSM manager;
    private EnemyParameter parameter;
    private CharacterStats characterStats;

    public WinState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.characterStats = manager.characterStats;
    }

    public void OnEnter()
    {
        parameter.agent.enabled = false;
        manager.coll.enabled = false;
        parameter.attackTarget = null;
        // 进入非战斗状态后，使Animator的Layer第二层权重设置为0，不播放该层的动画
        parameter.animator.SetLayerWeight(1, 0);
        parameter.animator.SetTrigger("Win");
    }

    public void OnUpdate() { }

    public void OnExit() { }
}