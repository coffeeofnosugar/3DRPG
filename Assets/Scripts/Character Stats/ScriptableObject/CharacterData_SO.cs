using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Tooltip("�Ƿ���AI")]
    public bool isOpenAI;

    [Tooltip("���Ѫ��")]
    public int maxHealth;

    [Tooltip("��ǰѪ��")]
    public int currentHealth;

    [Tooltip("��������ֵ")]
    public int baseDefence;

    [Tooltip("��ǰ����ֵ")]
    public int currentDefence;

    [Tooltip("�����󣬶��ʱ���ݻ�")]
    public float destoryTime;

    [Tooltip("��Ѳ�߹��")]
    public bool isPatrol = true;

    [Tooltip("Ѳ���ٶȣ���isPatrol = false������������ã�")]
    public float walkSpeed = 1.5f;

    [Tooltip("����ʱ�䣨��isPatrol = false������������ã�")]
    public float idleTime = 2;

    [Tooltip("Ѳ�߷�Χ")]
    public float patrolRange = 8;

    [Tooltip("��Ұ��Χ")]
    public float sightRadius = 10;

    [Tooltip("׷���ٶ�")]
    public float runSpeed = 2.5f;

    [Tooltip("׷����Χ")]
    public float runRange = 20;
}
