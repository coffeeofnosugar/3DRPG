using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// ���Ͻڵ�
    /// ���ƿ�������for  if��
    /// ����ӽڵ�
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