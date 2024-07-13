using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputManagerEntry;


namespace BehaviourTree
{
    /// <summary>
    /// ��Ϊ��ִ����
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
                Debug.Log($"{transform.name} δ������Ϊ��");
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
            // ÿʱÿ�̶�Ѱ����ҵ�λ��
            tree.FoundTarget();
            // ִ����Ϊ��
            tree.Update();
        }
    }
}