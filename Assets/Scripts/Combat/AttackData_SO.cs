using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    /// <summary>
    /// ������Χ
    /// </summary>
    public float attackRange;
    /// <summary>
    /// ���ܹ�����Χ
    /// </summary>
    public float skillRange;
    /// <summary>
    /// ������ȴʱ��
    /// </summary>
    public float coolDown;
    /// <summary>
    /// ��С�˺�
    /// </summary>
    public int minDamge;
    /// <summary>
    /// ����˺�
    /// </summary>
    public int maxDamge;
    /// <summary>
    /// ����ֵ
    /// </summary>
    public float criticalMultiplier;
    /// <summary>
    /// ������
    /// </summary>
    public float criticalChance;
}
