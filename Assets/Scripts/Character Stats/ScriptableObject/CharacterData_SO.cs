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

    public List<SkillData_SO> skillList = new List<SkillData_SO>();


#if UNITY_EDITOR
    public SkillData_SO CreateSkill()
    {
        SkillData_SO skill =ScriptableObject.CreateInstance<SkillData_SO>();
        skill.name = skill.skillName;
        skillList.Add(skill);
        AssetDatabase.AddObjectToAsset(skill, this);
        AssetDatabase.SaveAssets();
        return skill;
    }

    public void DeleteSkill(SkillData_SO skill)
    {
        skillList.Remove(skill);

        AssetDatabase.RemoveObjectFromAsset(skill);
        AssetDatabase.SaveAssets();
    }
#endif
}
