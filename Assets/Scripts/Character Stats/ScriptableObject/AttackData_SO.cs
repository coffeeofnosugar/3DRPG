using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Tooltip("������Χ")]
    public float attackRange;

    [Tooltip("������ȴʱ��")]
    public float coolDown;

    [Tooltip("��С�˺�")]
    public int minDamge;

    [Tooltip("����˺�")]
    public int maxDamge;

    [Tooltip("���˾���")]
    public float kickForce;
}
