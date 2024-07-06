using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;


namespace BehaviourTree
{
    /// <summary>
    /// 装饰器节点
    /// 
    /// 一个子节点
    /// </summary>
    public abstract class DecoratorNode : Node
    {
        //[HideInInspector] 
        public Node child;


        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}