using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyParameter
{

    [Header("Ѳ��/վ��")]
    [Tooltip("��Ѳ�߹��")]
    public bool isPatrol = true;
    [Tooltip("Ѳ���ٶȣ���isPatrol = false������������ã�")]
    public float walkSpeed = 1.5f;
    [Tooltip("����ʱ�䣨��isPatrol = false������������ã�")]
    public float idleTime = 2;
    [Tooltip("Ѳ�߷�Χ")]
    public float patrolRange = 8;


    [Header("��������")]
    [Tooltip("Ѫ��")]
    public int health = 100;

    [Tooltip("��Ұ��Χ")]
    public float sightRadius = 10;

    [Tooltip("׷���ٶ�")]
    public float runSpeed = 2.5f;

    [Tooltip("׷����Χ")]
    public float runRange = 20;

    [Header("����")]
    [Tooltip("����CD")]
    public float attackCD = 2;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;

    // ������
    [HideInInspector] public Vector3 originPosition;
    // ����Ŀ��
    [HideInInspector] public GameObject attackTarget;
    // ����ʱ����
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
            Debug.Log($"�˳� {currentState} ������ {newState}");
        }
        currentState = newState;
        currentState.OnEnter();
    }

    /// <summary>
    /// �ж�Player�Ƿ����Լ���׷����Χ�ڣ�����Palyer��ֵ��attackTarget
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
    /// �ж�Player���Լ��ľ����Ƿ����㹥������
    /// </summary>
    /// <returns></returns>
    public bool IsTargetInAttackRange()
    {
        if (parameter.attackTarget)
        {
            // ���������֮��ľ����Ƿ�С�ڹ�������
            return (parameter.attackTarget.transform.position - transform.position).sqrMagnitude <= 1;
        }
        else
            return false;
    }


    private void OnDrawGizmosSelected()
    {
        #region Ѳ�߷�Χ������ʾ��Ҫ��Ϊ�༭ʱչʾ�õģ����к���ʾ��λ�ò���
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, parameter.patrolRange);
        #endregion

        #region ��Ұ��Χ
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, parameter.sightRadius);
        #endregion

        #region ������Χ����ʹ�÷��飬��ֱ��ʹ�þ����ж�
        //// �÷����޷����÷���ķ�����Զ����������ķ��򣬹ʷ���
        ////Gizmos.DrawWireCube(parameter.attackPoint.position, parameter.attackSize);

        //Transform center = parameter.attackPoint;
        //Matrix4x4 oldMat = Gizmos.matrix;
        ////��ȡĿ����ת����
        //Matrix4x4 rotationMat = center.localToWorldMatrix;
        ////���õ�ǰΪ��ת����
        //Gizmos.matrix = rotationMat;
        //{
        //    Gizmos.color = Color.red;
        //    //�����center�����Ŀ�����Ķ��ԣ���Ϊ��תcube��Ŀ��λ����ͬ������zero
        //    Gizmos.DrawWireCube(center: Vector3.zero, size: parameter.attackSize);
        //}
        ////���õ�ǰ����
        //Gizmos.matrix = oldMat;
        #endregion

        #region ׷����Χ
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, parameter.runRange);
        #endregion
    }
}
