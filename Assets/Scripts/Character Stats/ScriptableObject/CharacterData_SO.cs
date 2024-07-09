using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    public int id = 0000;

    public string monsterName = "Template";

    public string prefab;

    [Tooltip("���Ѫ��")]
    public int maxHealth = 30;

    [Tooltip("��������ֵ")]
    public int baseDefence = 2;

    [Tooltip("Ѳ���ٶȣ���isPatrol = false������������ã�")]
    public float walkSpeed = 1.5f;

    [Tooltip("׷���ٶ�")]
    public float runSpeed = 2.5f;

    [Tooltip("Ѳ�߷�Χ")]
    public float patrolRange = 8;

    [Tooltip("��Ұ��Χ")]
    public float sightRadius = 10;

    [Tooltip("����ֵ")]
    public float criticalMultiplier = 2;

    [Tooltip("������")]
    public float criticalChance = 0.2f;

    [Tooltip("�����󣬶��ʱ���ݻ�")]
    public int destoryTime = 5;

    public List<AttackData_SO> skillList = new List<AttackData_SO>();


#if UNITY_EDITOR
    public AttackData_SO CreateAttackData_SO()
    {
        AttackData_SO so =ScriptableObject.CreateInstance<AttackData_SO>();
        so.name = so.skillName;
        skillList.Add(so);
        AssetDatabase.AddObjectToAsset(so, this);
        AssetDatabase.SaveAssets();
        return so;
    }
#endif
}
