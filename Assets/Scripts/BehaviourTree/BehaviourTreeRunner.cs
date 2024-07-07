using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputManagerEntry;


namespace BehaviourTree
{
    /// <summary>
    /// ��Ϊ��ִ����
    /// </summary>
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private void Awake()
        {
            if (tree == null)
            {
                Debug.Log($"{transform.name} δ������Ϊ��");
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