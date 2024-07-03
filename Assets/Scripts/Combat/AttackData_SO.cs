using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Tooltip("攻击范围")]
    public float attackRange;

    [Tooltip("技能攻击范围")]
    public float skillRange;

    [Tooltip("攻击冷却时间")]
    public float coolDown;

    [Tooltip("最小伤害")]
    public int minDamge;

    [Tooltip("最大伤害")]
    public int maxDamge;

    [Tooltip("暴击值")]
    public float criticalMultiplier;

    [Tooltip("暴击率")]
    public float criticalChance;
}
