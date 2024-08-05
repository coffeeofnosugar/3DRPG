using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// װ�����ڵ�
    /// һ���ӽڵ�
    /// </summary>
    public abstract class DecoratorNode : Node
    {
        [ReadOnly, BoxGroup]
        public Node child;


        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}