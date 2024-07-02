using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyParameter
{

    [Header("巡逻/站岗")]
    [Tooltip("是巡逻怪物？")]
    public bool isPatrol = true;
    [Tooltip("巡逻速度（若isPatrol = false，则该数据无用）")]
    public float walkSpeed = 1.5f;
    [Tooltip("逗留时间（若isPatrol = false，则该数据无用）")]
    public float idleTime = 2;
    [Tooltip("巡逻范围")]
    public float patrolRange = 8;


    [Header("基础属性")]
    [Tooltip("血量")]
    public int health = 100;

    [Tooltip("视野范围")]
    public float sightRadius = 10;

    [Tooltip("追击速度")]
    public float runSpeed = 2.5f;

    [Tooltip("追击范围")]
    public float runRange = 20;

    [Header("攻击")]
    [Tooltip("攻击CD")]
    public float attackCD = 2;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;

    // 出生点
    [HideInInspector] public Vector3 originPosition;
    // 攻击目标
    [HideInInspector] public GameObject attackTarget;
    // 攻击时间间隔
    [HideInInspector] public float lastAttackTime;
}

public class FSM : MonoBehaviour
{
    public EnemyParameter parameter;
    public CharacterStats characterStats;
    public IState currentState;

    private BaseIdleState idleState;
    private WalkState walkState;
    private FightIdleState chaseState;
    private RunState runState;
    private AttackState attackState;
    private HitState hitState;
    private DieState dieState;

    private void Awake()
    {
        parameter.animator = GetComponent<Animator>();
        parameter.agent = GetComponent<NavMeshAgent>();
        characterStats = GetComponent<CharacterStats>();

        parameter.originPosition = transform.position;
        parameter.lastAttackTime = parameter.attackCD;
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

        TransitionState(StateType.BaseIdle);
    }

    private void Update()
    {
        parameter.lastAttackTime += Time.deltaTime;
        Debugs.Instance["parameter.lastAttackTime"] = parameter.lastAttackTime.ToString("f2");
        currentState.OnUpdate();
    }

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
            _ => idleState
        };

        if (currentState != null)
        {
            currentState.OnExit();
            Debug.Log($"退出 {currentState} ，进入 {newState}");
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
        var colliders = Physics.OverlapSphere(transform.position, parameter.patrolRange, 1 << 6);
        if (colliders.Length > 0)
        {
            parameter.attackTarget = colliders[0].gameObject;
            return true;
        }
        else
        {
            parameter.attackTarget = null;
            return false;
        }
    }

    /// <summary>
    /// 判断Player与自己的距离是否满足攻击距离
    /// </summary>
    /// <returns></returns>
    public bool IsTargetInAttackRange()
    {
        if (parameter.attackTarget)
        {
            // 返回与玩家之间的距离是否小于攻击距离
            return (parameter.attackTarget.transform.position - transform.position).sqrMagnitude <= 1;
        }
        else
            return false;
    }


    private void OnDrawGizmosSelected()
    {
        #region 巡逻范围，该显示主要是为编辑时展示用的，运行后显示的位置不对
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, parameter.patrolRange);
        #endregion

        #region 视野范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, parameter.sightRadius);
        #endregion

        #region 攻击范围，不使用方块，而直接使用距离判断
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
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, parameter.runRange);
        #endregion
    }
}
