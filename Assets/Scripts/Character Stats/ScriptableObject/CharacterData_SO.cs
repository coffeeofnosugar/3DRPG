using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterData_SO : ScriptableObject
{
    public int id = 0000;

    public string monsterName = "Template";

    [Tooltip("最大血量")]
    public int maxHealth = 30;

    [Tooltip("基础防御值")]
    public int baseDefence = 2;

    [Tooltip("巡逻速度（若isPatrol = false，则该数据无用）")]
    public float walkSpeed = 1.5f;

    [Tooltip("追击速度")]
    public float runSpeed = 2.5f;

    [Tooltip("巡逻范围")]
    public float patrolRange = 8;

    [Tooltip("视野范围")]
    public float sightRadius = 10;

    [Tooltip("暴击值")]
    public float criticalMultiplier = 2;

    [Tooltip("暴击率")]
    public float criticalChance = 0.2f;

    [Tooltip("死亡后，多久时间后摧毁")]
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
