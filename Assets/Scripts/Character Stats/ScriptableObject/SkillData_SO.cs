using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillData_SO : ScriptableObject
{
    [Tooltip("技能名称")]
    public string skillName = "Temp";

    [Tooltip("动画名称")]
    public string animation;

    [Tooltip("攻击范围")]
    public float attackRange = 1;

    public float attackRangeSqr => attackRange * attackRange;

    [Tooltip("攻击冷却时间")]
    public float coolDown = 2;

    [Tooltip("最小伤害")]
    public int minDamge = 3;

    [Tooltip("最大伤害")]
    public int maxDamge = 5;

    [Tooltip("击退距离")]
    public float kickForce = 0;
}
