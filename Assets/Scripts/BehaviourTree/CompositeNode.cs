using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// 复合节点
    /// 类似控制流，for  if等
    /// 多个子节点
    /// </summary>
    public abstract class CompositeNode : Node
    {
        //[HideInInspector]
        public List<Node> children = new List<Node>();

        public override Node Clone(Transform t)
        {
            CompositeNode node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone(t));
            return node;
        }
    }
}