using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Tooltip("���Ѫ��")]
    public int maxHealth;

    [Tooltip("��������ֵ")]
    public int baseDefence;

    [Tooltip("Ѳ���ٶȣ���isPatrol = false������������ã�")]
    public float walkSpeed = 1.5f;

    [Tooltip("׷���ٶ�")]
    public float runSpeed = 2.5f;

    [Tooltip("Ѳ�߷�Χ")]
    public float patrolRange = 8;

    [Tooltip("��Ұ��Χ")]
    public float sightRadius = 10;

    [Tooltip("����ֵ")]
    public float criticalMultiplier;

    [Tooltip("������")]
    public float criticalChance;

    [Tooltip("�����󣬶��ʱ���ݻ�")]
    public float destoryTime;
}
