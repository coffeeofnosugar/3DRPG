using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillData_SO : ScriptableObject
{
    [Tooltip("��������")]
    public string skillName = "Temp";

    [Tooltip("��������")]
    public string animation;

    [Tooltip("������Χ")]
    public float attackRange = 1;

    public float attackRangeSqr => attackRange * attackRange;

    [Tooltip("������ȴʱ��")]
    public float coolDown = 2;

    [Tooltip("��С�˺�")]
    public int minDamge = 3;

    [Tooltip("����˺�")]
    public int maxDamge = 5;

    [Tooltip("���˾���")]
    public float kickForce = 0;
}
