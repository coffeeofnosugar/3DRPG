using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// װ�����ڵ�
    /// 
    /// һ���ӽڵ�
    /// </summary>
    public abstract class DecoratorNode : Node
    {
        public Node child;
    }
}