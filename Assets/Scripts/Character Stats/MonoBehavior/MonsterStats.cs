using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class MonsterStats : CharacterStats
{
    [HideInInspector] public NavMeshAgent agent;

    /// <summary>
    /// 反应距离，在start中遍历所有技能，取其中最小的攻击距离
    /// </summary>
    [ReadOnly, HideInEditorMode] public float responseDistance;
    public float responseDistanceSqr => responseDistance * responseDistance;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        foreach (var skillData in SkillList)
        {
            responseDistance = skillData.attackRange > responseDistance ? skillData.attackRange : responseDistance;
        }
    }
}
