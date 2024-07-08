using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Tooltip("����ֵ")]
    public float criticalMultiplier;

    [Tooltip("������")]
    public float criticalChance;

    [Header("��ͨ����")]
    [Tooltip("������Χ")]
    public float attackRange;

    [Tooltip("������ȴʱ��")]
    public float coolDown;
    
    [Tooltip("����չ�ʱ��")]
    public float lastAttackTime;

    [Tooltip("��С�˺�")]
    public int minDamge;

    [Tooltip("����˺�")]
    public int maxDamge;


    [Header("���ܹ���")]
    [Tooltip("���ܹ�����Χ")]
    public float skillRange;

    [Tooltip("���˾���")]
    public float kickForce;

    [Tooltip("������ȴʱ��")]
    public float skillCoolDown;

    [Tooltip("����ͷż���ʱ��")]
    public float lastSkillTime;

    [Tooltip("������С�˺�")]
    public float skillMinDamge;

    [Tooltip("��������˺�")]
    public float skillMaxDamge;
}
