using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Tooltip("暴击值")]
    public float criticalMultiplier;

    [Tooltip("暴击率")]
    public float criticalChance;

    [Header("普通攻击")]
    [Tooltip("攻击范围")]
    public float attackRange;

    [Tooltip("攻击冷却时间")]
    public float coolDown;
    
    [Tooltip("最后普攻时间")]
    public float lastAttackTime;

    [Tooltip("最小伤害")]
    public int minDamge;

    [Tooltip("最大伤害")]
    public int maxDamge;


    [Header("技能攻击")]
    [Tooltip("技能攻击范围")]
    public float skillRange;

    [Tooltip("击退距离")]
    public float kickForce;

    [Tooltip("技能冷却时间")]
    public float skillCoolDown;

    [Tooltip("最后释放技能时间")]
    public float lastSkillTime;

    [Tooltip("技能最小伤害")]
    public float skillMinDamge;

    [Tooltip("技能最大伤害")]
    public float skillMaxDamge;
}
