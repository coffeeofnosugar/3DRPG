using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateData;

    //[HideInInspector]
    public CharacterData_SO characterData;

    public AttackData_SO attackData;


    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Collider coll;

    // 出生点
    [HideInInspector] public Vector3 originPosition;
    [HideInInspector] public Quaternion originRotation;
    // 攻击目标
    [HideInInspector] public GameObject attackTarget;
    // 攻击时间间隔
    [HideInInspector] public float lastAttackTime;

    [HideInInspector] public bool isCritical;

    [HideInInspector] public bool getHit;

    [HideInInspector] public bool isDeath;



    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
    }


    #region Read from CharacterData_SO
    public bool IsOpenAI
    {
        get { if (characterData != null) { return characterData.isOpenAI; } else { return true; } }
        set { characterData.isOpenAI = value; }
    }
    public int MaxHealth
    {
        get { if (characterData != null) { return characterData.maxHealth; } else { return 0; } }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData != null) { return characterData.currentHealth; } else { return 0; } }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) { return characterData.baseDefence; } else { return 0; } }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData != null) { return characterData.currentDefence; } else { return 0; } }
        set { characterData.baseDefence = value; }
    }
    public float DestoryTime
    {
        get { if (characterData != null) { return characterData.destoryTime; } else { return 0; } }
        set { characterData.destoryTime = value; }
    }
    public bool IsPatrol
    {
        get { if (characterData != null) { return characterData.isPatrol; } else { return true; } }
        set { characterData.isPatrol = value; }
    }
    public float WalkSpeed
    {
        get { if (characterData != null) { return characterData.walkSpeed; } else { return 0; } }
        set { characterData.walkSpeed = value; }
    }
    public float IdleTime
    {
        get { if (characterData != null) { return characterData.idleTime; } else { return 0; } }
        set { characterData.idleTime = value; }
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
    public float RunRange
    {
        get { if (characterData != null) { return characterData.runRange; } else { return 0; } }
        set { characterData.runRange = value; }
    }
    #endregion

    #region Read fron AttackData_SO
    public float AttackRange
    {
        get { if (attackData != null) { return attackData.attackRange; } else { return 0; } }
        set { attackData.attackRange = value; }
    }
    public float SkillRange
    {
        get { if (attackData != null) { return attackData.skillRange; } else { return 0; } }
        set { attackData.skillRange = value; }
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
    public float CriticalMultiplier
    {
        get { if (attackData != null) { return attackData.criticalMultiplier; } else { return 0; } }
        set { attackData.criticalMultiplier = value; }
    }
    public float CriticalChance
    {
        get { if (attackData != null) { return attackData.criticalChance; } else { return 0; } }
        set { attackData.criticalChance = value; }
    }
    #endregion




    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        defener.getHit = true;
        float coreDamage = Random.Range(attacker.MinDange, attacker.MaxDange);
        if (attacker.isCritical)
        {
            coreDamage *= attacker.CriticalMultiplier;
            Debug.Log("暴击！" + coreDamage);
        }

        int damage = Mathf.Max((int)coreDamage - defener.CurrentDefence, 0);
        defener.CurrentHealth = Mathf.Max(defener.CurrentHealth - damage, 0);
        // Update UI
        // 经验Update
    }
}
