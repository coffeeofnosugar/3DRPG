using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class FSM : MonoBehaviour, IEndGameObserver
{
    [HideInInspector] public CharacterStats characterStats;

    public IState currentState;

    private BaseIdleState idleState;
    private WalkState walkState;
    private FightIdleState chaseState;
    private RunState runState;
    private AttackState attackState;
    private HitState hitState;
    private DieState dieState;
    private WinState winState;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        characterStats.animator = GetComponent<Animator>();
        characterStats.agent = GetComponent<NavMeshAgent>();
        characterStats.coll = GetComponent<Collider>();

        characterStats.originPosition = transform.position;
        characterStats.originRotation = transform.rotation;
        characterStats.lastAttackTime = characterStats.CoolDown;
    }

    private void Start()
    {
        idleState = new BaseIdleState(this);
        walkState = new WalkState(this);
        chaseState = new FightIdleState(this);
        runState = new RunState(this);
        attackState = new AttackState(this);
        hitState = new HitState(this);
        dieState = new DieState(this);
        winState = new WinState(this);

        TransitionState(StateType.BaseIdle);
    }

    private void OnEnable()
    {
        GameManager.Instance.AddObserver(this);
    }

    private void OnDisable()
    {
        GameManager.Instance?.RemoveObserver(this);
    }


    private void Update()
    {
        characterStats.lastAttackTime += Time.deltaTime;
        Debugs.Instance["parameter.lastAttackTime"] = characterStats.lastAttackTime.ToString("f2");
        currentState.OnUpdate();
    }

    /// <summary>
    /// 状态转换
    /// </summary>
    /// <param name="type"></param>
    public void TransitionState(StateType type)
    {
        IState newState = type switch
        {
            StateType.BaseIdle => idleState,
            StateType.Walk => walkState,
            StateType.FightIdle => chaseState,
            StateType.Run => runState,
            StateType.Attack => attackState,
            StateType.Hit => hitState,
            StateType.Die => dieState,
            StateType.Win => winState,
            _ => idleState
        };

        if (currentState != null)
        {
            currentState.OnExit();
            //Debug.Log($"退出 {currentState} ，进入 {newState}");
            Debugs.Instance["EnemyState"] = $"{newState}, lastState: {currentState}";
        }
        currentState = newState;
        currentState.OnEnter();
    }

    /// <summary>
    /// 判断Player是否在自己的追击范围内，并将Palyer赋值给attackTarget
    /// </summary>
    /// <returns></returns>
    public bool IsFoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, characterStats.PatrolRange, 1 << 6);
        if (colliders.Length > 0)
        {
            characterStats.attackTarget = colliders[0].gameObject;
            return true;
        }
        else
        {
            characterStats.attackTarget = null;
            return false;
        }
    }

    /// <summary>
    /// 判断Player与自己的距离是否满足攻击距离
    /// </summary>
    /// <returns></returns>
    public bool IsTargetInAttackRange()
    {
        if (characterStats.attackTarget)
        {
            // 返回与玩家之间的距离是否小于攻击距离
            return (characterStats.attackTarget.transform.position - transform.position).sqrMagnitude <= 1;
        }
        else
            return false;
    }

    /// <summary>
    /// 播放攻击动画时调用此方法
    /// </summary>
    public void Attack()
    {
        if (characterStats.attackTarget)
        {
            var targetState = characterStats.attackTarget.GetComponent<CharacterStats>();
            targetState.TakeDamage(characterStats, targetState);
            var targerAnimator = characterStats.attackTarget.GetComponent<Animator>();
            targerAnimator.SetTrigger("Hit");
        }
    }

    private void OnDrawGizmosSelected()
    {
        #region 巡逻范围，该显示主要是为编辑时展示用的，运行后显示的位置不对
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, characterStats.PatrolRange);
        #endregion

        #region 视野范围
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, characterStats.SightRadius);
        #endregion

        #region 攻击范围，不使用方块，而直接使用距离判断，舍弃方案
        //// 该方法无法设置方块的方向，永远是世界坐标的方向，故放弃
        ////Gizmos.DrawWireCube(parameter.attackPoint.position, parameter.attackSize);

        //Transform center = parameter.attackPoint;
        //Matrix4x4 oldMat = Gizmos.matrix;
        ////获取目标旋转矩阵
        //Matrix4x4 rotationMat = center.localToWorldMatrix;
        ////设置当前为旋转矩阵
        //Gizmos.matrix = rotationMat;
        //{
        //    Gizmos.color = Color.red;
        //    //这里的center是相对目标中心而言，因为旋转cube与目标位置相同所以是zero
        //    Gizmos.DrawWireCube(center: Vector3.zero, size: parameter.attackSize);
        //}
        ////重置当前矩阵
        //Gizmos.matrix = oldMat;
        #endregion

        #region 追击范围
        //Gizmos.color = Color.black;
        //Gizmos.DrawWireSphere(transform.position, characterStats.RunRange);
        #endregion
    }

    public void EndNotify()
    {
        // 失败
        // 结束移动
        // 结束动画
        TransitionState(StateType.Win);
    }
}
