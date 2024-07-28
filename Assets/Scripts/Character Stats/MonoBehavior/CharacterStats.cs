using System.Collections.Generic;
using UnityEngine;


public abstract class CharacterStats : MonoBehaviour
{
    [SerializeField] private CharacterData_SO templateData;
    private CharacterData_SO characterData;
    public Blackboard blackboard;
    
    [HideInInspector] public Animator animator;
    [HideInInspector] public new Transform transform;
    
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

    public int currentHealth;

    public Dictionary<string, SkillData_SO> SkillDict { get; private set; } = new Dictionary<string, SkillData_SO>();

    protected virtual void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
        else
            Debug.LogError("未配置角色数据");
        
        animator = GetComponent<Animator>();

        transform = ((Component)this).transform;
        
        originPosition = transform.position;
        originRotation = transform.rotation;

        currentHealth = MaxHealth;
    }

    protected virtual void Start()
    {
        if (SkillList.Count != 0)
        {
            foreach (var skillData in SkillList)
            {
                SkillDict.Add(skillData.name, skillData);
                blackboard.lastAttackTime.Add(skillData.name, 999f);
            }
        }
    }

    /// <summary>
    /// 判断是否有技能的CD和技能满足攻击条件
    /// </summary>
    /// <param name="animatorText">
    /// 传入指定技能时判断该技能是否达到释放条件;
    /// 传入null时判断所有技能，如果有一个技能达到释放条件便返回true
    /// </param>
    /// <returns></returns>
    public bool CouldAttack(string animatorText = null)
    {
        if (animatorText != null)
        {
            if (blackboard.lastAttackTime[animatorText] >= SkillDict[animatorText].coolDown && blackboard.distanceTarget <= SkillDict[animatorText].attackRange)
            {
                return true;
            }
        }
        else
        {
            foreach (var key in blackboard.lastAttackTime.Keys)
            {
                if (blackboard.lastAttackTime[key] >= SkillDict[key].coolDown && blackboard.distanceTarget <= SkillDict[key].attackRange)
                {
                    return true;
                }
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
    public int Id => characterData ? characterData.id : 0;

    public string MonsterName => characterData ? characterData.monsterName : string.Empty;

    public int MaxHealth => characterData ? characterData.maxHealth : 0;

    public int BaseDefence => characterData ? characterData.baseDefence : 0;

    public float WalkSpeed => characterData ? characterData.walkSpeed : 0;

    public float PatrolRange => characterData ? characterData.patrolRange : 0;

    public float SightRadius => characterData ? characterData.sightRadius : 0;

    public float RunSpeed => characterData ? characterData.runSpeed : 0;

    public float CriticalMultiplier => characterData ? characterData.criticalMultiplier : 0;

    public float CriticalChance => characterData ? characterData.criticalChance : 0;

    public int DestoryTime => characterData ? characterData.destoryTime : 0;

    protected List<SkillData_SO> SkillList => characterData ? characterData.skillList : null;

    #endregion
}
