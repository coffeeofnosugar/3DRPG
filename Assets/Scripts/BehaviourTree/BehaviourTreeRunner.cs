using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputManagerEntry;


namespace BehaviourTree
{
    /// <summary>
    /// 行为树执行器
    /// </summary>
    [RequireComponent(typeof(MonsterStats))]
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private MonsterStats monsterStats;

        private void Awake()
        {
            if (tree == null)
            {
                Debug.LogError($"{transform.name} 未配置行为树");
                return;
            }
            monsterStats = GetComponent<MonsterStats>();
        }

        private void Start()
        {
            tree = tree.Clone();
            tree.Bind(monsterStats);
        }

        private void Update()
        {
            // 每时每刻都寻找玩家的位置
            tree.FoundTarget();
            // 执行行为树
            tree.Update();
        }
    }
}