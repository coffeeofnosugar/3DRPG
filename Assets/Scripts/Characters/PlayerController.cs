using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class PlayerController : MonoBehaviour, IEndGameObserver
{
    private NavMeshAgent agent;
    private Animator animator;
    private CharacterStats characterStats;

    private GameObject attackTarget;

    private float lastAttackTime;
    [SerializeField] private float attackCD = 0.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToPoint;
        MouseManager.Instance.OnEnemyClicked += EventAttack;

        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Update()
    {
        characterStats.isDeath = characterStats.CurrentHealth <= 0;
        animator.SetBool("Death", characterStats.isDeath);
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        float speed = agent.velocity.sqrMagnitude;
        float maxSpeed = agent.speed;
        
        animator.SetFloat("Speed", speed / (maxSpeed * maxSpeed));
    }

    #region move and attack
    private void MoveToPoint(Vector3 target)
    {
        StopAllCoroutines();
        if (characterStats.isDeath) return;
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject enemy)
    {
        if (characterStats.isDeath) return;
        if (enemy != null)
        {
            attackTarget = enemy;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    private IEnumerator MoveToAttackTarget()
    {
        transform.LookAt(attackTarget.transform);

        while ((transform.position - attackTarget.transform.position).sqrMagnitude > 1)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        if (lastAttackTime < 0)
        {
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            animator.SetBool("IsCritical", characterStats.isCritical);
            animator.SetTrigger("Attack");
            lastAttackTime = attackCD;
        }
    }

    public void Attack()
    {
        if (attackTarget)
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }
    #endregion

    public void EndNotify()
    {
        // 胜利
        // 结束移动
        // 结束动画
    }

    private void Death()
    {

    }
}
