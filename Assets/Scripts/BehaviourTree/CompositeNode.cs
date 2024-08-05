using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        [ReadOnly, BoxGroup]
        public List<Node> children = new List<Node>();

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}