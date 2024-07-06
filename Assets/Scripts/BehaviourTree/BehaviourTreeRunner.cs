using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// ��Ϊ��ִ����
    /// </summary>
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private void Start()
        {
            tree = tree.Clone();
        }

        private void Update()
        {
            tree.Update();
        }
    }
}