using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateData;

    [HideInInspector] public CharacterData_SO characterData;

    public SkillData_SO attackData;


    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Collider coll;

    // 出生点
    [HideInInspector] public Vector3 originPosition;
    [HideInInspector] public Quaternion originRotation;

    public bool getHit;
    public bool isDeath;
    public bool isCritical;

    public int currentHealth;
    public int currentDefence;
    public Dictionary<string, float> lastAttackTime = new Dictionary<string, float>();


    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider>();

        originPosition = transform.position;
        originRotation = transform.rotation;

        currentHealth = MaxHealth;
        currentDefence = BaseDefence;
    }




    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defener"></param>
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        defener.getHit = true;
        float coreDamage = Random.Range(attacker.MinDange, attacker.MaxDange);
        if (attacker.isCritical)
        {
            coreDamage *= attacker.CriticalMultiplier;
            Debug.Log("暴击！" + coreDamage);
        }

        int damage = Mathf.Max((int)coreDamage - defener.currentDefence, 0);
        defener.currentHealth = Mathf.Max(defener.currentHealth - damage, 0);
        // Update UI
        // 经验Update
    }

    #region Read from CharacterData_SO
    public int MaxHealth
    {
        get { if (characterData != null) { return characterData.maxHealth; } else { return 0; } }
        set { characterData.maxHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) { return characterData.baseDefence; } else { return 0; } }
        set { characterData.baseDefence = value; }
    }
    public float WalkSpeed
    {
        get { if (characterData != null) { return characterData.walkSpeed; } else { return 0; } }
        set { characterData.walkSpeed = value; }
    }
    public float PatrolRange
    {
        get { if (characterData != null) { return characterData.patrolRange; } else { return 0; } }
        set { characterData.patrolRange = value; }
    }
    public float SightRadius
    {
        get { if (characterData != null) { return characterData.sightRadius; } else { return 0; } }
        set { characterData.sightRadius = value; }
    }
    public float RunSpeed
    {
        get { if (characterData != null) { return characterData.runSpeed; } else { return 0; } }
        set { characterData.runSpeed = value; }
    }
    public float CriticalMultiplier
    {
        get { if (characterData != null) { return characterData.criticalMultiplier; } else { return 0; } }
        set { characterData.criticalMultiplier = value; }
    }
    public float CriticalChance
    {
        get { if (characterData != null) { return characterData.criticalChance; } else { return 0; } }
        set { characterData.criticalChance = value; }
    }
    public int DestoryTime
    {
        get { if (characterData != null) { return characterData.destoryTime; } else { return 0; } }
        set { characterData.destoryTime = value; }
    }
    #endregion

    #region Read fron AttackData_SO
    public float AttackRange
    {
        get { if (attackData != null) { return attackData.attackRange; } else { return 0; } }
        set { attackData.attackRange = value; }
    }
    /// <summary>
    /// AttackRange的平方
    /// </summary>
    public float SqrAttackRange
    {
        get { return AttackRange * AttackRange; }
    }
    public float CoolDown
    {
        get { if (attackData != null) { return attackData.coolDown; } else { return 0; } }
        set { attackData.coolDown = value; }
    }
    public int MinDange
    {
        get { if (attackData != null) { return attackData.minDamge; } else { return 0; } }
        set { attackData.minDamge = value; }
    }
    public int MaxDange
    {
        get { if (attackData != null) { return attackData.maxDamge; } else { return 0; } }
        set { attackData.maxDamge = value; }
    }
    public float KickForce
    {
        get { if (attackData != null) { return attackData.kickForce; } else { return 0; } }
        set { attackData.kickForce = value; }
    }
    #endregion
}
