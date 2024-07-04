using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// ���Ͻڵ�
    /// ���ƿ�������for  if��
    /// ����ӽڵ�
    /// </summary>
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new List<Node>();
    }
}