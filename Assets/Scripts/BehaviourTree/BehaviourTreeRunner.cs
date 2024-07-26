using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputManagerEntry;


namespace BehaviourTree
{
    /// <summary>
    /// ��Ϊ��ִ����
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
                Debug.LogError($"{transform.name} δ������Ϊ��");
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
            // ÿʱÿ�̶�Ѱ����ҵ�λ��
            tree.FoundTarget();
            // ִ����Ϊ��
            tree.Update();
        }
    }
}