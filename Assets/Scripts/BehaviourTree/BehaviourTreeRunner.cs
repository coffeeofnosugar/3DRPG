using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputManagerEntry;


namespace BehaviourTree
{
    /// <summary>
    /// 行为树执行器
    /// </summary>
    [RequireComponent(typeof(CharacterStats))]
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private CharacterStats characterStats;

        private void Awake()
        {
            if (tree == null)
            {
                Debug.Log($"{transform.name} 未配置行为树");
                return;
            }
            characterStats = GetComponent<CharacterStats>();
        }

        private void Start()
        {
            tree = tree.Clone();
            tree.Bind(characterStats);
        }

        private void Update()
        {
            tree.DebugShow();
            // 每时每刻都寻找玩家的位置
            tree.FoundTarget();
            // 执行行为树
            tree.Update();
        }
    }
}