using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputManagerEntry;


namespace BehaviourTree
{
    /// <summary>
    /// 行为树执行器
    /// </summary>
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private void Awake()
        {
            if (tree == null)
            {
                Debug.Log($"{transform.name} 未配置行为树");
                return;
            }
        }

        private void Start()
        {
            tree = tree.Clone();
            tree.Bind();
        }

        private void Update()
        {
            tree.Update();
        }
    }
}