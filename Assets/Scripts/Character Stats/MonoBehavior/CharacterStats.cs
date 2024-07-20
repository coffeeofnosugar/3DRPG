using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.AI;


public class CharacterStats : MonoBehaviour
{
    [SerializeField] private CharacterData_SO templateData;

    private CharacterData_SO characterData;
    
    public Blackboard blackboard;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Collider coll;
    [HideInInspector] public new Rigidbody rigidbody;
    [HideInInspector] public PlayerInputController PlayerInputController;

    // 出生点
    [HideInInspector] public Vector3 originPosition;
    [HideInInspector] public Quaternion originRotation;

    public bool _getHit;

    public bool GetHit
    {
        get => _getHit;
        set
        {
            _getHit = value;
            if (blackboard != null)
            {
                blackboard.getHit = value;
            }
        }
    }
    public bool isDeath;
    public bool isCritical;

    public int currentHealth;
    public int currentDefence;

    public Dictionary<string, SkillData_SO> skillDict = new Dictionary<string, SkillData_SO>();

    public float responseDistance;
    public float responseDistanceSqr => responseDistance * responseDistance;

    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
        else
            Debug.LogError("未配置角色数据");
        
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        PlayerInputController = GetComponent<PlayerInputController>();

        originPosition = transform.position;
        originRotation = transform.rotation;

        currentHealth = MaxHealth;
        currentDefence = BaseDefence;
    }

    private void Start()
    {
        if (SkillList.Count != 0)
        {
            responseDistance = SkillList[0].attackRange;
            foreach (var skillData in SkillList)
            {
                responseDistance = skillData.attackRange > responseDistance ? skillData.attackRange : responseDistance;
                skillDict.Add(skillData.name, skillData);
                blackboard.lastAttackTime.Add(skillData.name, 999f);
                
            }
        }
    }

    /// <summary>
    /// 判断是否有技能的CD和技能满足攻击条件
    /// </summary>
    /// <returns></returns>
    public bool CouldAttack()
    {
        foreach (var key in blackboard.lastAttackTime.Keys)
        {
            if (blackboard.lastAttackTime[key] >= skillDict[key].coolDown && blackboard.distanceTarget <= skillDict[key].attackRange)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defener"></param>
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        defener.GetHit = true;
        // float coreDamage = Random.Range(attacker.MinDange, attacker.MaxDange);
        // if (attacker.isCritical)
        // {
        //     coreDamage *= attacker.CriticalMultiplier;
        //     Debug.Log("暴击！" + coreDamage);
        // }
        //
        // int damage = Mathf.Max((int)coreDamage - defener.currentDefence, 0);
        // defener.currentHealth = Mathf.Max(defener.currentHealth - damage, 0);
         // Update UI
         // 经验Update
    }

    #region Read from CharacterData_SO
    public int Id
    {
        get => characterData ? characterData.id : 0;
        set => characterData.id = value;
    }
    public string MonsterName
    {
        get => characterData ? characterData.monsterName : string.Empty;
        set => characterData.monsterName = value;
    }
    public int MaxHealth
    {
        get => characterData ? characterData.maxHealth : 0;
        set => characterData.maxHealth = value;
    }
    public int BaseDefence
    {
        get => characterData ? characterData.baseDefence : 0;
        set => characterData.baseDefence = value;
    }

    public float WalkSpeed
    {
        get => characterData ? characterData.walkSpeed : 0;
        set => characterData.walkSpeed = value;
    }
    public float PatrolRange
    {
        get => characterData ? characterData.patrolRange : 0;
        set => characterData.patrolRange = value;
    }
    public float SightRadius
    {
        get => characterData ? characterData.sightRadius : 0;
        set => characterData.sightRadius = value;
    }
    public float RunSpeed
    {
        get => characterData ? characterData.runSpeed : 0;
        set => characterData.runSpeed = value;
    }
    public float CriticalMultiplier
    {
        get => characterData ? characterData.criticalMultiplier : 0;
        set => characterData.criticalMultiplier = value;
    }
    public float CriticalChance
    {
        get => characterData ? characterData.criticalChance : 0;
        set => characterData.criticalChance = value;
    }
    public int DestoryTime
    {
        get => characterData ? characterData.destoryTime : 0;
        set => characterData.destoryTime = value;
    }
    public List<SkillData_SO> SkillList
    {
        get => characterData ? characterData.skillList : null;
        set => characterData.skillList = value;
    }
    #endregion
}
