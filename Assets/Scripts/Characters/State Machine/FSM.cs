using UnityEngine;
using UnityEngine.AI;


namespace Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterStats))]
    public class FSM : MonoBehaviour, IEndGameObserver
    {
        [HideInInspector] public CharacterStats characterStats;

        public IState currentState;
        public StateType currentStateType;

        private BaseIdleState idleState;
        private WalkState walkState;
        private FightIdleState chaseState;
        private RunState runState;
        private AttackState attackState;
        private HitState hitState;
        private DieState dieState;
        private WinState winState;
        private SkillState skillState;

        private void Awake()
        {
            characterStats = GetComponent<CharacterStats>();
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
            skillState = new SkillState(this);

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
            characterStats.lastSkillTime += Time.deltaTime;

            currentState.OnUpdate();
        }

        /// <summary>
        /// ״̬ת��
        /// </summary>
        /// <param name="type"></param>
        public void TransitionState(StateType type)
        {
            currentStateType = type;
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
                StateType.Skill => skillState,
                _ => idleState
            };

            if (currentState != null)
            {
                currentState.OnExit();
                //Debug.Log($"�˳� {currentState} ������ {newState}");
                //Debugs.Instance["EnemyState"] = $"{newState}, lastState: {currentState}";
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
        /// ���Ź�������ʱ���ô˷���
        /// </summary>
        public void Attack()
        {
            if (characterStats.attackTarget)
            {
                var targetStats = characterStats.attackTarget.GetComponent<CharacterStats>();
                targetStats.TakeDamage(characterStats, targetStats);
                var targetAnimator = characterStats.attackTarget.GetComponent<Animator>();
                targetAnimator.SetTrigger("Hit");
            }
        }

        private void OnDrawGizmosSelected()
        {
            #region Ѳ�߷�Χ������ʾ��Ҫ��Ϊ�༭ʱչʾ�õģ����к���ʾ��λ�ò���
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, characterStats.PatrolRange);
            #endregion

            #region ��Ұ��Χ
            //Gizmos.color = Color.blue;
            //Gizmos.DrawWireSphere(transform.position, characterStats.SightRadius);
            #endregion

            #region ������Χ����ʹ�÷��飬��ֱ��ʹ�þ����жϣ���������
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
            //Gizmos.color = Color.black;
            //Gizmos.DrawWireSphere(transform.position, characterStats.RunRange);
            #endregion
        }

        public void EndNotify()
        {
            // ʧ��
            // �����ƶ�
            // ��������
            TransitionState(StateType.Win);
        }
    }


}
