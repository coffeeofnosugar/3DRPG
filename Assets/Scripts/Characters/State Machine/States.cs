using UnityEngine;
using UnityEngine.AI;


/*
namespace Characters
{
    public class BaseIdleState : IState
    {
        private FSM manager;
        private CharacterStats characterStats;

        private float time = 0;

        public BaseIdleState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
        }

        public void OnEnter()
        {
            characterStats.agent.destination = manager.transform.position;
            // �����ս��״̬��ʹAnimator��Layer�ڶ���Ȩ������Ϊ0�������Ÿò�Ķ���
            characterStats.animator.SetLayerWeight(1, 0);
        }

        public void OnUpdate()
        {
            if (characterStats.getHit)
                manager.TransitionState(StateType.Hit);

            else if (manager.IsFoundPlayer())
                manager.TransitionState(StateType.FightIdle);

            else if (characterStats.IsOpenAI)
            {
                if (characterStats.IsPatrol)
                {
                    time += Time.deltaTime;
                    if (time > characterStats.IdleTime)
                        manager.TransitionState(StateType.Walk);
                }
                else
                {
                    if (characterStats.originPosition.x != manager.transform.position.x || characterStats.originPosition.z != manager.transform.position.z)
                    {
                        time += Time.deltaTime;
                        if (time > characterStats.IdleTime)
                            manager.TransitionState(StateType.Walk);
                    }
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
        private CharacterStats characterStats;

        private Vector3 wayPoint;
        private float stoppingDistance;

        public WalkState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
            this.stoppingDistance = characterStats.agent.stoppingDistance;
        }

        public void OnEnter()
        {
            characterStats.agent.speed = characterStats.WalkSpeed;
            characterStats.animator.SetBool("Walk", true);
            GetNewWayPoint();
        }

        public void OnUpdate()
        {
            if (characterStats.getHit)
                manager.TransitionState(StateType.Hit);

            // �����Ұ���е��ˣ�����ս��ģʽ
            if (manager.IsFoundPlayer())
                manager.TransitionState(StateType.FightIdle);

            // �����Ѳ�߹�
            if (characterStats.IsPatrol)
            {
                // ��ʼѲ��
                characterStats.agent.destination = wayPoint;
                if ((wayPoint - manager.transform.position).sqrMagnitude <= stoppingDistance * stoppingDistance)
                    manager.TransitionState(StateType.BaseIdle);
            }
            else
            {
                // ���س�����
                characterStats.agent.destination = characterStats.originPosition;
                if (characterStats.originPosition.x == manager.transform.position.x && characterStats.originPosition.z == manager.transform.position.z)
                {
                    float angel = Quaternion.Angle(characterStats.originRotation, manager.transform.rotation);
                    if (angel >= 0.01f)
                    {
                        manager.transform.rotation = Quaternion.Slerp(manager.transform.rotation, characterStats.originRotation, 0.01f);
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
            characterStats.animator.SetBool("Walk", false);
        }

        private void GetNewWayPoint()
        {
            float randomX = Random.Range(-characterStats.PatrolRange, characterStats.PatrolRange);
            float randomZ = Random.Range(-characterStats.PatrolRange, characterStats.PatrolRange);

            Vector3 randomPoint = new Vector3(characterStats.originPosition.x + randomX, manager.transform.position.y, characterStats.originPosition.z + randomZ);

            wayPoint = NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1, 1) ? hit.position : manager.transform.position;
        }
    }

    public class FightIdleState : IState
    {
        private FSM manager;
        private CharacterStats characterStats;

        public FightIdleState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
        }

        public void OnEnter()
        {
            // ����ս��״̬��ʹAnimator��Layer�ڶ���Ȩ������Ϊ1����ȫ����������Ķ���
            characterStats.animator.SetLayerWeight(1, 1);
        }

        public void OnUpdate()
        {
            if (characterStats.getHit)
                manager.TransitionState(StateType.Hit);

            if (characterStats.IsOpenAI && characterStats.attackTarget)
            {
                float sqrMagnitude = (characterStats.attackTarget.transform.position - manager.transform.position).sqrMagnitude;

                if (characterStats.LastSkillTime >= characterStats.SkillCoolDown && sqrMagnitude <= characterStats.SqrSkillRange)
                {
                    manager.TransitionState(StateType.Skill);
                }
                else if (characterStats.LastAttackTime >= characterStats.CoolDown && sqrMagnitude <= characterStats.SqrAttackRange)
                {
                    manager.TransitionState(StateType.Attack);
                }
                else if (sqrMagnitude >= characterStats.sqrDistance)
                {
                    manager.TransitionState(StateType.Run);
                }
            }
        }

        public void OnExit() { }
    }

    public class RunState : IState
    {
        private FSM manager;
        private CharacterStats characterStats;

        public RunState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
        }

        public void OnEnter()
        {
            characterStats.animator.SetBool("Run", true);

            characterStats.agent.speed = characterStats.RunSpeed;
        }

        public void OnUpdate()
        {
            if (characterStats.getHit)
                manager.TransitionState(StateType.Hit);

            if (manager.IsFoundPlayer())
            {
                float sqrMagnitude = (characterStats.attackTarget.transform.position - manager.transform.position).sqrMagnitude;

                if (characterStats.LastSkillTime >= characterStats.SkillCoolDown && sqrMagnitude <= characterStats.SqrSkillRange)
                {
                    manager.TransitionState(StateType.Skill);
                }
                else if (characterStats.LastAttackTime >= characterStats.CoolDown && sqrMagnitude <= characterStats.SqrAttackRange)
                {
                    manager.TransitionState(StateType.Attack);
                }
                else if (sqrMagnitude <= characterStats.sqrDistance)
                {
                    manager.TransitionState(StateType.FightIdle);
                }
                else
                    characterStats.agent.destination = characterStats.attackTarget.transform.position;
            }
            else
                manager.TransitionState(StateType.BaseIdle);
        }

        public void OnExit()
        {
            characterStats.animator.SetBool("Run", false);
        }
    }

    public class AttackState : IState
    {
        private FSM manager;
        private CharacterStats characterStats;

        private AnimatorStateInfo info;

        public AttackState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
        }

        public void OnEnter()
        {
            // �ж��Ƿ񱩻�
            characterStats.isCritical = Random.value < characterStats.CriticalChance;

            // ������ʼʱ����CD
            characterStats.LastAttackTime = 0;
            // ֹͣ�ƶ�
            characterStats.agent.destination = manager.transform.position;
            // ���򹥻�Ŀ��
            manager.transform.LookAt(characterStats.attackTarget.transform);
            // ���Ź�������
            characterStats.animator.SetTrigger("Attack");
        }

        public void OnUpdate()
        {
            if (characterStats.getHit)
                manager.TransitionState(StateType.Hit);

            info = characterStats.animator.GetCurrentAnimatorStateInfo(1);
            //Debugs.Instance["info.normalizedTime"] = info.normalizedTime.ToString("f2");
            if (info.normalizedTime >= .95f)
                manager.TransitionState(StateType.FightIdle);
        }

        public void OnExit() { }
    }
    public class SkillState : IState
    {
        private FSM manager;
        private CharacterStats characterStats;

        private AnimatorStateInfo info;

        public SkillState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
        }

        public void OnEnter()
        {
            // �ж��Ƿ񱩻�
            characterStats.isCritical = Random.value < characterStats.CriticalChance;
            //Debugs.Instance["isCritical"] = characterStats.isCritical.ToString();

            // ������ʼʱ����CD
            characterStats.LastSkillTime = 0;
            // ֹͣ�ƶ�
            characterStats.agent.destination = manager.transform.position;
            // ���򹥻�Ŀ��
            manager.transform.LookAt(characterStats.attackTarget.transform);
            // ���Ź�������
            characterStats.animator.SetTrigger("Skill");
        }

        public void OnUpdate()
        {
            if (characterStats.getHit)
                manager.TransitionState(StateType.Hit);

            info = characterStats.animator.GetCurrentAnimatorStateInfo(1);
            //Debugs.Instance["info.normalizedTime"] = info.normalizedTime.ToString("f2");
            if (info.normalizedTime >= .95f)
                manager.TransitionState(StateType.FightIdle);
        }

        public void OnExit() { }
    }

    public class HitState : IState
    {
        private FSM manager;
        private CharacterStats characterStats;

        private AnimatorStateInfo info;

        public HitState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
        }

        public void OnEnter()
        {
            characterStats.animator.SetTrigger("Hit");
        }

        public void OnUpdate()
        {
            info = characterStats.animator.GetCurrentAnimatorStateInfo(2);

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
        private CharacterStats characterStats;

        public DieState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
        }

        public void OnEnter()
        {
            characterStats.agent.enabled = false;
            characterStats.coll.enabled = false;
            characterStats.attackTarget = null;
            characterStats.animator.SetTrigger("Death");
            Object.Destroy(manager.gameObject, characterStats.DestoryTime);
        }

        public void OnUpdate() { }

        public void OnExit() { }
    }
    public class WinState : IState
    {
        private FSM manager;
        private CharacterStats characterStats;

        public WinState(FSM manager)
        {
            this.manager = manager;
            this.characterStats = manager.characterStats;
        }

        public void OnEnter()
        {
            characterStats.agent.enabled = false;
            characterStats.coll.enabled = false;
            characterStats.attackTarget = null;
            // �����ս��״̬��ʹAnimator��Layer�ڶ���Ȩ������Ϊ0�������Ÿò�Ķ���
            characterStats.animator.SetLayerWeight(1, 0);
            characterStats.animator.SetTrigger("Win");
        }

        public void OnUpdate() { }

        public void OnExit() { }
    }
}

*/