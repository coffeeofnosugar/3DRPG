using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float attackRange;
    /// <summary>
    /// 技能攻击范围
    /// </summary>
    public float skillRange;
    /// <summary>
    /// 攻击冷却时间
    /// </summary>
    public float coolDown;
    /// <summary>
    /// 最小伤害
    /// </summary>
    public int minDamge;
    /// <summary>
    /// 最大伤害
    /// </summary>
    public int maxDamge;
    /// <summary>
    /// 暴击值
    /// </summary>
    public float criticalMultiplier;
    /// <summary>
    /// 暴击率
    /// </summary>
    public float criticalChance;
}
