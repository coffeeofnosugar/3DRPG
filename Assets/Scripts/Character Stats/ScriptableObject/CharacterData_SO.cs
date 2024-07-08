using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Tooltip("最大血量")]
    public int maxHealth;

    [Tooltip("基础防御值")]
    public int baseDefence;

    [Tooltip("巡逻速度（若isPatrol = false，则该数据无用）")]
    public float walkSpeed = 1.5f;

    [Tooltip("追击速度")]
    public float runSpeed = 2.5f;

    [Tooltip("巡逻范围")]
    public float patrolRange = 8;

    [Tooltip("视野范围")]
    public float sightRadius = 10;

    [Tooltip("暴击值")]
    public float criticalMultiplier;

    [Tooltip("暴击率")]
    public float criticalChance;

    [Tooltip("死亡后，多久时间后摧毁")]
    public float destoryTime;
}
