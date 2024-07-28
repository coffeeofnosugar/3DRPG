using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterStats : CharacterStats
{
    [HideInInspector] public NavMeshAgent agent;

    /// <summary>
    /// ��Ӧ���룬��start�б������м��ܣ�ȡ������С�Ĺ�������
    /// </summary>
    public float responseDistance;
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
